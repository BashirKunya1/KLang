
using System.Text;

namespace KLang.Compiler.Lexing;

public sealed class Lexer
{
    private readonly string _source;
    private int _position;

    public Lexer(string source)
    {
        ArgumentNullException.ThrowIfNull(source);
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

            if (Current == '/' && Peek() == '/')
            {
                SkipLineComment();
                continue;
            }

            if (Current == '/' && Peek() == '*')
            {
                SkipBlockComment();
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

            // Process two-character operators before single-character ones.
            if (Current == '!' && Peek() == '=')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.NotEqual, start));
                continue;
            }

            if (Current == '=' && Peek() == '=')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.EqualEqual, start));
                continue;
            }

            if (Current == '<' && Peek() == '=')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.LessEqual, start));
                continue;
            }

            if (Current == '>' && Peek() == '=')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.GreaterEqual, start));
                continue;
            }

            if (Current == '&' && Peek() == '&')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.AndAnd, start));
                continue;
            }

            if (Current == '|' && Peek() == '|')
            {
                tokens.Add(ReadTwoCharacterToken(
                    TokenType.OrOr, start));
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
                '%' => new Token(TokenType.Percent, "%", start),

                '!' => new Token(TokenType.Bang, "!", start),
                '=' => new Token(TokenType.Equal, "=", start),
                '<' => new Token(TokenType.Less, "<", start),
                '>' => new Token(TokenType.Greater, ">", start),

                _ => throw new LexerException(
                    $"Unexpected character '{Current}' at position {start}.")
            };

            Advance();
            tokens.Add(token);
        }

        tokens.Add(new Token(
            TokenType.EndOfFile,
            string.Empty,
            _position));

        return tokens;
    }

    private Token ReadInteger(int start)
    {
        while (!IsAtEnd && char.IsDigit(Current))
        {
            Advance();
        }

        return new Token(
            TokenType.IntegerLiteral,
            _source[start.._position],
            start);
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

        return new Token(type, lexeme, start);
    }

    private Token ReadString(int start)
    {
        // Consume the opening quote.
        Advance();

        var value = new StringBuilder();

        while (!IsAtEnd && Current != '"')
        {
            if (Current == '\n' || Current == '\r')
            {
                throw new LexerException(
                    $"Unterminated string literal at position {start}.");
            }

            if (Current == '\\')
            {
                Advance();

                if (IsAtEnd)
                {
                    throw new LexerException(
                        $"Unterminated string literal at position {start}.");
                }

                var escapedCharacter = Current switch
                {
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    '\\' => '\\',
                    '"' => '"',

                    _ => throw new LexerException(
                        $"Invalid escape sequence '\\{Current}' " +
                        $"at position {_position - 1}.")
                };

                value.Append(escapedCharacter);
                Advance();
                continue;
            }

            value.Append(Current);
            Advance();
        }

        if (IsAtEnd)
        {
            throw new LexerException(
                $"Unterminated string literal at position {start}.");
        }

        // Consume the closing quote.
        Advance();

        return new Token(
            TokenType.StringLiteral,
            value.ToString(),
            start);
    }

    private void SkipLineComment()
    {
        // Consume //
        Advance();
        Advance();

        while (!IsAtEnd && Current != '\n')
        {
            Advance();
        }
    }

    private void SkipBlockComment()
    {
        var start = _position;

        // Consume /*
        Advance();
        Advance();

        while (!IsAtEnd)
        {
            if (Current == '*' && Peek() == '/')
            {
                // Consume */
                Advance();
                Advance();
                return;
            }

            Advance();
        }

        throw new LexerException(
            $"Unterminated block comment at position {start}.");
    }

    private Token ReadTwoCharacterToken(
        TokenType type,
        int start)
    {
        var lexeme = _source.Substring(start, 2);

        Advance();
        Advance();

        return new Token(type, lexeme, start);
    }

    private char Peek()
    {
        return _position + 1 < _source.Length
            ? _source[_position + 1]
            : '\0';
    }

    private bool IsAtEnd => _position >= _source.Length;

    private char Current => _source[_position];

    private void Advance() => _position++;
}
