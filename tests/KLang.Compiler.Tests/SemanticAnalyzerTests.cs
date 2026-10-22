
using KLang.Compiler.Ast;
using KLang.Compiler.Lexing;
using KLang.Compiler.Parsing;
using KLang.Compiler.Semantics;
using Xunit;

namespace KLang.Compiler.Tests;

public sealed class SemanticAnalyzerTests
{
    [Fact]
    public void InferType_IntegerLiteral_ReturnsInteger()
    {
        var expression = new IntegerLiteralExpressionNode(42);

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Integer, actual);
    }

    [Fact]
    public void InferType_StringLiteral_ReturnsString()
    {
        var expression = new StringLiteralExpressionNode("hello");

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.String, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void InferType_BooleanLiteral_ReturnsBoolean(bool value)
    {
        var expression = new BooleanLiteralExpressionNode(value);

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Boolean, actual);
    }

    [Fact]
    public void InferType_Identifier_ReturnsUnknown()
    {
        var expression = new IdentifierExpressionNode("counter");

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Unknown, actual);
    }


    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("*")]
    [InlineData("/")]
    [InlineData("%")]
    public void InferType_IntegerArithmetic_ReturnsInteger(
        string op)
    {
        var expression = new BinaryExpressionNode(
            new IntegerLiteralExpressionNode(10),
            op,
            new IntegerLiteralExpressionNode(2));

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Integer, actual);
    }

    [Theory]
    [InlineData("<")]
    [InlineData("<=")]
    [InlineData(">")]
    [InlineData(">=")]
    [InlineData("==")]
    [InlineData("!=")]
    public void InferType_IntegerComparison_ReturnsBoolean(
        string op)
    {
        var expression = new BinaryExpressionNode(
            new IntegerLiteralExpressionNode(10),
            op,
            new IntegerLiteralExpressionNode(20));

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Boolean, actual);
    }

    [Theory]
    [InlineData("&&")]
    [InlineData("||")]
    public void InferType_LogicalOperator_ReturnsBoolean(
        string op)
    {
        var expression = new BinaryExpressionNode(
            new BooleanLiteralExpressionNode(true),
            op,
            new BooleanLiteralExpressionNode(false));

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Boolean, actual);
    }

    [Theory]
    [InlineData("-")]
    [InlineData("+")]
    public void InferType_NumericUnaryOperator_ReturnsInteger(
        string op)
    {
        var expression = new UnaryExpressionNode(
            op,
            new IntegerLiteralExpressionNode(42));

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Integer, actual);
    }

    [Fact]
    public void InferType_LogicalNot_ReturnsBoolean()
    {
        var expression = new UnaryExpressionNode(
            "!",
            new BooleanLiteralExpressionNode(true));

        var actual = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Boolean, actual);
    }



    [Theory]
    [InlineData("\"hello\" + 10;")]
    [InlineData("true - false;")]
    [InlineData("10 && 20;")]
    [InlineData("!42;")]
    [InlineData("\"hello\" < \"world\";")]
    public void InferType_WithInvalidOperands_ReturnsUnknown(string source)
    {
        var expression = ParseExpression(source);

        var result = SemanticAnalyzer.InferType(expression);

        Assert.Equal(SemanticType.Unknown, result);
    }



    private static ExpressionNode ParseExpression(string source)
    {
        var tokens = new Lexer(source).Tokenize();
        var program = new Parser(tokens).ParseProgram();

        var statement = Assert.IsType<ExpressionStatementNode>(
            Assert.Single(program.Statements));

        return statement.Expression;
    }



}
