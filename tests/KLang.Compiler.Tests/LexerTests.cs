using KLang.Compiler.Lexing;
using Xunit;

namespace KLang.Compiler.Tests;

public sealed class LexerTests
{
    [Fact]
    public void Tokenizes_Print42()
    {
        var tokens = new Lexer("print(42);").Tokenize();

        Assert.Collection(tokens,
            token => Assert.Equal((TokenType.PrintKeyword, "print", 0), (token.Type, token.Lexeme, token.Position)),
            token => Assert.Equal((TokenType.LeftParen, "(", 5), (token.Type, token.Lexeme, token.Position)),
            token => Assert.Equal((TokenType.IntegerLiteral, "42", 6), (token.Type, token.Lexeme, token.Position)),
            token => Assert.Equal((TokenType.RightParen, ")", 8), (token.Type, token.Lexeme, token.Position)),
            token => Assert.Equal((TokenType.Semicolon, ";", 9), (token.Type, token.Lexeme, token.Position)),
            token => Assert.Equal((TokenType.EndOfFile, "", 10), (token.Type, token.Lexeme, token.Position)));
    }

    [Fact]
    public void IgnoresWhitespace()
    {
        var tokens = new Lexer("  print (  7 );\n").Tokenize();

        Assert.Equal(6, tokens.Count);
        Assert.Equal(TokenType.IntegerLiteral, tokens[2].Type);
        Assert.Equal("7", tokens[2].Lexeme);
    }

    [Fact]
    public void RejectsUnknownCharacter()
    {
        var exception = Assert.Throws<LexerException>(() => new Lexer("print(@);").Tokenize());

        Assert.Contains("Unexpected character '@'", exception.Message);
    }

    [Fact]
    public void Tokenize_WithStringLiteral_ReturnsStringToken()
    {
        var lexer = new Lexer("print(\"Hello\");");

        var tokens = lexer.Tokenize();

        Assert.Equal(
            TokenType.StringLiteral,
            tokens[2].Type);

        Assert.Equal(
            "Hello",
            tokens[2].Lexeme);
    }

    [Fact]
    public void Tokenize_WithBooleanLiterals_ReturnsBooleanTokens()
    {
        var lexer = new Lexer("print(true); print(false);");

        var tokens = lexer.Tokenize();

        Assert.Equal(
            TokenType.TrueKeyword,
            tokens[2].Type);

        Assert.Equal(
            TokenType.FalseKeyword,
            tokens[7].Type);
    }

    [Fact]
    public void Tokenize_WithIdentifier_ReturnsIdentifierToken()
    {
        var lexer = new Lexer("print(myVariable);");

        var tokens = lexer.Tokenize();

        Assert.Equal(
            TokenType.Identifier,
            tokens[2].Type);

        Assert.Equal(
            "myVariable",
            tokens[2].Lexeme);
    }

    [Fact]
    public void Tokenize_WithOperators_ReturnsOperatorTokens()
    {
        var lexer = new Lexer(
            "1 + 2 - 3 * 4 / 5 == 6 != 7");

        var tokens = lexer.Tokenize();

        var types = tokens
            .Select(token => token.Type)
            .ToArray();

        Assert.Equal(
            new[]
            {
            TokenType.IntegerLiteral,
            TokenType.Plus,
            TokenType.IntegerLiteral,
            TokenType.Minus,
            TokenType.IntegerLiteral,
            TokenType.Star,
            TokenType.IntegerLiteral,
            TokenType.Slash,
            TokenType.IntegerLiteral,
            TokenType.EqualEqual,
            TokenType.IntegerLiteral,
            TokenType.NotEqual,
            TokenType.IntegerLiteral,
            TokenType.EndOfFile
            },
            types);
    }

    [Fact]
    public void Tokenize_WithComma_ReturnsCommaToken()
    {
        var lexer = new Lexer("calculate(10, 20);");

        var tokens = lexer.Tokenize();

        Assert.Contains(
            tokens,
            token => token.Type == TokenType.Comma);
    }

    [Fact]
    public void Tokenize_WithUnknownIdentifier_DoesNotThrow()
    {
        var lexer = new Lexer("myVariable");

        var tokens = lexer.Tokenize();

        Assert.Equal(
            TokenType.Identifier,
            tokens[0].Type);

        Assert.Equal(
            "myVariable",
            tokens[0].Lexeme);
    }

    [Fact]
    public void Tokenize_WithUnterminatedString_ThrowsLexerException()
    {
        var lexer = new Lexer("print(\"Hello);");

        var exception = Assert.Throws<LexerException>(
            () => lexer.Tokenize());

        Assert.Contains(
            "Unterminated string literal",
            exception.Message);
    }
}
