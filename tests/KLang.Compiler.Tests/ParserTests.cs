using KLang.Compiler.Ast;
using KLang.Compiler.Lexing;
using KLang.Compiler.Parsing;
using Xunit;

namespace KLang.Compiler.Tests;

public sealed class ParserTests
{
    private static ProgramNode Parse(string source)
    {
        var lexer = new Lexer(source);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);

        return parser.ParseProgram();
    }

    [Fact]
    public void ParseProgram_WithPrintIntegerLiteral_CreatesPrintStatement()
    {
        var program = Parse("print(42);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<IntegerLiteralExpressionNode>(
                statement.Expression);

        Assert.Equal(42, expression.Value);
    }

    [Fact]
    public void ParseProgram_WithStringLiteral_CreatesStringLiteral()
    {
        var program = Parse("print(\"Hello\");");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<StringLiteralExpressionNode>(
                statement.Expression);

        Assert.Equal("Hello", expression.Value);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void ParseProgram_WithBooleanLiteral_CreatesBooleanLiteral(
        string source,
        bool expected)
    {
        var program = Parse($"print({source});");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<BooleanLiteralExpressionNode>(
                statement.Expression);

        Assert.Equal(expected, expression.Value);
    }

    [Fact]
    public void ParseProgram_WithIdentifier_CreatesIdentifierExpression()
    {
        var program = Parse("print(myVariable);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<IdentifierExpressionNode>(
                statement.Expression);

        Assert.Equal("myVariable", expression.Name);
    }

    [Fact]
    public void ParseProgram_WithAddition_CreatesBinaryExpression()
    {
        var program = Parse("print(10 + 20);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<BinaryExpressionNode>(
                statement.Expression);

        Assert.Equal("+", expression.Operator);

        Assert.Equal(
            10,
            Assert.IsType<IntegerLiteralExpressionNode>(
                expression.Left).Value);

        Assert.Equal(
            20,
            Assert.IsType<IntegerLiteralExpressionNode>(
                expression.Right).Value);
    }

    [Fact]
    public void ParseProgram_WithOperatorPrecedence_ParsesMultiplicationBeforeAddition()
    {
        var program = Parse("print(1 + 2 * 3);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var addition =
            Assert.IsType<BinaryExpressionNode>(
                statement.Expression);

        Assert.Equal("+", addition.Operator);

        Assert.Equal(
            1,
            Assert.IsType<IntegerLiteralExpressionNode>(
                addition.Left).Value);

        var multiplication =
            Assert.IsType<BinaryExpressionNode>(
                addition.Right);

        Assert.Equal("*", multiplication.Operator);

        Assert.Equal(
            2,
            Assert.IsType<IntegerLiteralExpressionNode>(
                multiplication.Left).Value);

        Assert.Equal(
            3,
            Assert.IsType<IntegerLiteralExpressionNode>(
                multiplication.Right).Value);
    }

    [Fact]
    public void ParseProgram_WithParentheses_OverridesOperatorPrecedence()
    {
        var program = Parse("print((1 + 2) * 3);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var multiplication =
            Assert.IsType<BinaryExpressionNode>(
                statement.Expression);

        Assert.Equal("*", multiplication.Operator);

        var addition =
            Assert.IsType<BinaryExpressionNode>(
                multiplication.Left);

        Assert.Equal("+", addition.Operator);

        Assert.Equal(
            1,
            Assert.IsType<IntegerLiteralExpressionNode>(
                addition.Left).Value);

        Assert.Equal(
            2,
            Assert.IsType<IntegerLiteralExpressionNode>(
                addition.Right).Value);

        Assert.Equal(
            3,
            Assert.IsType<IntegerLiteralExpressionNode>(
                multiplication.Right).Value);
    }

    [Fact]
    public void ParseProgram_WithUnaryMinus_CreatesUnaryExpression()
    {
        var program = Parse("print(-42);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<UnaryExpressionNode>(
                statement.Expression);

        Assert.Equal("-", expression.Operator);

        Assert.Equal(
            42,
            Assert.IsType<IntegerLiteralExpressionNode>(
                expression.Operand).Value);
    }

    [Fact]
    public void ParseProgram_WithUnaryBang_CreatesUnaryExpression()
    {
        var program = Parse("print(!true);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<UnaryExpressionNode>(
                statement.Expression);

        Assert.Equal("!", expression.Operator);

        Assert.True(
            Assert.IsType<BooleanLiteralExpressionNode>(
                expression.Operand).Value);
    }

    [Fact]
    public void ParseProgram_WithFunctionCall_CreatesCallExpression()
    {
        var program = Parse("print(calculate(10, 20));");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var call =
            Assert.IsType<CallExpressionNode>(
                statement.Expression);

        var callee =
            Assert.IsType<IdentifierExpressionNode>(
                call.Callee);

        Assert.Equal("calculate", callee.Name);

        Assert.Equal(2, call.Arguments.Count);

        Assert.Equal(
            10,
            Assert.IsType<IntegerLiteralExpressionNode>(
                call.Arguments[0]).Value);

        Assert.Equal(
            20,
            Assert.IsType<IntegerLiteralExpressionNode>(
                call.Arguments[1]).Value);
    }

    [Fact]
    public void ParseProgram_WithNoArguments_CreatesCallExpression()
    {
        var program = Parse("print(getValue());");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var call =
            Assert.IsType<CallExpressionNode>(
                statement.Expression);

        var callee =
            Assert.IsType<IdentifierExpressionNode>(
                call.Callee);

        Assert.Equal("getValue", callee.Name);
        Assert.Empty(call.Arguments);
    }

    [Fact]
    public void ParseProgram_WithExpressionStatement_CreatesExpressionStatement()
    {
        var program = Parse("calculate(10, 20);");

        var statement =
            Assert.IsType<ExpressionStatementNode>(
                Assert.Single(program.Statements));

        var call =
            Assert.IsType<CallExpressionNode>(
                statement.Expression);

        Assert.Equal(2, call.Arguments.Count);
    }

    [Fact]
    public void ParseProgram_WithEquality_CreatesBinaryExpression()
    {
        var program = Parse("print(10 == 10);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<BinaryExpressionNode>(
                statement.Expression);

        Assert.Equal("==", expression.Operator);
    }

    [Fact]
    public void ParseProgram_WithNotEqual_CreatesBinaryExpression()
    {
        var program = Parse("print(10 != 20);");

        var statement = Assert.IsType<PrintStatementNode>(
            Assert.Single(program.Statements));

        var expression =
            Assert.IsType<BinaryExpressionNode>(
                statement.Expression);

        Assert.Equal("!=", expression.Operator);
    }

    [Fact]
    public void ParseProgram_WithMultipleStatements_CreatesMultipleStatements()
    {
        var program = Parse("""
            print(42);
            print("Hello");
            print(true);
            """);

        Assert.Equal(3, program.Statements.Count);
    }

    [Fact]
    public void ParseProgram_WithMissingSemicolon_ThrowsParserException()
    {
        var exception = Assert.Throws<ParserException>(
            () => Parse("print(42)"));

        Assert.Contains(
            "Expected ';' after print statement.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithMissingClosingParenthesis_ThrowsParserException()
    {
        var exception = Assert.Throws<ParserException>(
            () => Parse("print(42;"));

        Assert.Contains(
            "Expected ')' after print expression.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithMissingExpression_ThrowsParserException()
    {
        var exception = Assert.Throws<ParserException>(
            () => Parse("print();"));

        Assert.Contains(
            "Expected an expression.",
            exception.Message);
    }

    [Fact]
    public void ParseProgram_WithTrailingComma_ThrowsParserException()
    {
        var exception = Assert.Throws<ParserException>(
            () => Parse("print(calculate(10,));"));

        Assert.Contains(
            "Expected an expression.",
            exception.Message);
    }
}