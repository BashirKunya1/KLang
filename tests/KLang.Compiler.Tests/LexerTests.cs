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
    public void RejectsUnknownIdentifier()
    {
        var exception = Assert.Throws<LexerException>(() => new Lexer("hello;").Tokenize());

        Assert.Contains("Unknown identifier 'hello'", exception.Message);
    }
}
