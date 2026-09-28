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

            var token = Current switch
            {
                '(' => new Token(TokenType.LeftParen, "(", start),
                ')' => new Token(TokenType.RightParen, ")", start),
                ';' => new Token(TokenType.Semicolon, ";", start),
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
        while (!IsAtEnd && (char.IsLetterOrDigit(Current) || Current == '_'))
            Advance();

        var lexeme = _source[start.._position];
        var type = lexeme switch
        {
            "print" => TokenType.PrintKeyword,
            _ => throw new LexerException($"Unknown identifier '{lexeme}' at position {start}.")
        };

        return new Token(type, lexeme, start);
    }

    private bool IsAtEnd => _position >= _source.Length;
    private char Current => _source[_position];

    private void Advance() => _position++;
}
