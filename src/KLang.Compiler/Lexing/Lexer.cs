namespace KLang.Compiler.Lexing;

public sealed class Lexer
{
    private readonly string _source;
    private int _position;

    public Lexer(string source)
    {
        _source = source;
    }

    public IReadOnlyList<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (!IsAtEnd)
        {
            if (char.IsWhiteSpace(Current))
            {
                Advance();
                continue;
            }

            var start = _position;

            if (char.IsDigit(Current))
            {
                tokens.Add(ReadInteger(start));
                continue;
            }

            if (char.IsLetter(Current) || Current == '_')
            {
                tokens.Add(ReadIdentifierOrKeyword(start));
                continue;
            }

            if (Current == '"')
            {
                tokens.Add(ReadString(start));
                continue;
            }

            var token = Current switch
            {
                '(' => new Token(TokenType.LeftParen, "(", start),
                ')' => new Token(TokenType.RightParen, ")", start),
                ',' => new Token(TokenType.Comma, ",", start),
                ';' => new Token(TokenType.Semicolon, ";", start),
                '+' => new Token(TokenType.Plus, "+", start),
                '-' => new Token(TokenType.Minus, "-", start),
                '*' => new Token(TokenType.Star, "*", start),
                '/' => new Token(TokenType.Slash, "/", start),
                '!' when Peek() == '=' =>
                    ReadTwoCharacterToken(
                    TokenType.NotEqual,
                    start),

                '!' => new Token(TokenType.Bang, "!", start),

                '=' when Peek() == '=' =>
                    ReadTwoCharacterToken(
                        TokenType.EqualEqual,
                        start),
                _ => throw new LexerException($"Unexpected character '{Current}' at position {start}.")
            };

            Advance();
            tokens.Add(token);
        }

        tokens.Add(new Token(TokenType.EndOfFile, string.Empty, _position));
        return tokens;
    }

    private Token ReadInteger(int start)
    {
        while (!IsAtEnd && char.IsDigit(Current))
            Advance();

        return new Token(TokenType.IntegerLiteral, _source[start.._position], start);
    }

    private Token ReadIdentifierOrKeyword(int start)
    {
        while (!IsAtEnd &&
               (char.IsLetterOrDigit(Current) || Current == '_'))
        {
            Advance();
        }

        var lexeme = _source[start.._position];

        var type = lexeme switch
        {
            "print" => TokenType.PrintKeyword,
            "true" => TokenType.TrueKeyword,
            "false" => TokenType.FalseKeyword,
            _ => TokenType.Identifier
        };

        return new Token(
            type,
            lexeme,
            start);
    }

    private Token ReadString(int start)
    {
        // Opening quote.
        Advance();

        var valueStart = _position;

        while (!IsAtEnd && Current != '"')
        {
            if (Current == '\\')
            {
                Advance();

                if (IsAtEnd)
                {
                    throw new LexerException(
                        $"Unterminated string literal at position {start}.");
                }
            }

            Advance();
        }

        if (IsAtEnd)
        {
            throw new LexerException(
                $"Unterminated string literal at position {start}.");
        }

        var lexeme = _source[valueStart.._position];

        // Closing quote.
        Advance();

        return new Token(
            TokenType.StringLiteral,
            lexeme,
            start);
    }

    private Token ReadTwoCharacterToken(
    TokenType type,
    int start)
    {
        var first = Current;
        Advance();

        var second = Current;
        Advance();

        return new Token(
            type,
            $"{first}{second}",
            start);
    }

    private char Peek()
    {
        if (_position + 1 >= _source.Length)
        {
            return '\0';
        }

        return _source[_position + 1];
    }

    private bool IsAtEnd => _position >= _source.Length;
    private char Current => _source[_position];

    private void Advance() => _position++;
}
