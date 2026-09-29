using KLang.Compiler.Ast;
using KLang.Compiler.Lexing;
using KLang.Compiler.Parsing;
using Xunit;

namespace KLang.Compiler.Tests;

public sealed class ParserTests
{
    [Fact]
    public void ParseProgram_WithPrintIntegerLiteral_CreatesPrintStatement()
    {
        var source = "print(42);";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);
        var program = parser.ParseProgram();

        Assert.Single(program.Statements);

        var statement = Assert.IsType<PrintStatementNode>(
            program.Statements[0]);

        var expression = Assert.IsType<IntegerLiteralExpressionNode>(
            statement.Expression);

        Assert.Equal(42, expression.Value);
    }

    [Fact]
    public void ParseProgram_WithMultiplePrintStatements_CreatesMultipleStatements()
    {
        var source = """
                     print(42);
                     print(100);
                     print(999);
                     """;

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);
        var program = parser.ParseProgram();

        Assert.Equal(3, program.Statements.Count);

        var first = Assert.IsType<PrintStatementNode>(
            program.Statements[0]);

        var second = Assert.IsType<PrintStatementNode>(
            program.Statements[1]);

        var third = Assert.IsType<PrintStatementNode>(
            program.Statements[2]);

        Assert.Equal(
            42,
            Assert.IsType<IntegerLiteralExpressionNode>(
                first.Expression).Value);

        Assert.Equal(
            100,
            Assert.IsType<IntegerLiteralExpressionNode>(
                second.Expression).Value);

        Assert.Equal(
            999,
            Assert.IsType<IntegerLiteralExpressionNode>(
                third.Expression).Value);
    }

    [Fact]
    public void ParseProgram_WithWhitespace_ParsesSuccessfully()
    {
        var source = """
                     
                     print(   42   );
                     
                     """;

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);
        var program = parser.ParseProgram();

        Assert.Single(program.Statements);

        var statement = Assert.IsType<PrintStatementNode>(
            program.Statements[0]);

        var expression = Assert.IsType<IntegerLiteralExpressionNode>(
            statement.Expression);

        Assert.Equal(42, expression.Value);
    }

    [Fact]
    public void ParseProgram_WithMissingSemicolon_ThrowsParserException()
    {
        var source = "print(42)";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);

        var exception = Assert.Throws<ParserException>(
            () => parser.ParseProgram());

        Assert.Contains(
            "Expected ';' after print statement.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithMissingClosingParenthesis_ThrowsParserException()
    {
        var source = "print(42;";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);

        var exception = Assert.Throws<ParserException>(
            () => parser.ParseProgram());

        Assert.Contains(
            "Expected ')' after print expression.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithMissingExpression_ThrowsParserException()
    {
        var source = "print();";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);

        var exception = Assert.Throws<ParserException>(
            () => parser.ParseProgram());

        Assert.Contains(
            "Expected an expression.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithUnexpectedToken_ThrowsParserException()
    {
        var source = "42;";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);

        var exception = Assert.Throws<ParserException>(
            () => parser.ParseProgram());

        Assert.Contains(
            "Expected a statement.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_ConsumesEndOfFile()
    {
        var source = "print(42);";

        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        Assert.Equal(
            TokenType.EndOfFile,
            tokens[^1].Type);

        var parser = new Parser(tokens);
        var program = parser.ParseProgram();

        Assert.Single(program.Statements);
    }
}
