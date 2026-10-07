using KLang.Compiler.Ast;
using KLang.Compiler.Lexing;

namespace KLang.Compiler.Parsing;

public sealed class Parser
{
    private readonly IReadOnlyList<Token> _tokens;
    private int _current;

    public Parser(IReadOnlyList<Token> tokens)
    {
        _tokens = tokens;
    }

    public ProgramNode ParseProgram()
    {
        var statements = new List<StatementNode>();

        while (!Check(TokenType.EndOfFile))
        {
            statements.Add(ParseStatement());
        }

        Consume(
            TokenType.EndOfFile,
            "Expected end of file.");

        return new ProgramNode(statements);
    }

    private StatementNode ParseStatement()
    {
        if (Match(TokenType.PrintKeyword))
        {
            return ParsePrintStatement();
        }

        return ParseExpressionStatement();
    }

    private PrintStatementNode ParsePrintStatement()
    {
        Consume(
            TokenType.LeftParen,
            "Expected '(' after 'print'.");

        var expression = ParseExpression();

        Consume(
            TokenType.RightParen,
            "Expected ')' after print expression.");

        Consume(
            TokenType.Semicolon,
            "Expected ';' after print statement.");

        return new PrintStatementNode(expression);
    }

    private ExpressionStatementNode ParseExpressionStatement()
    {
        var expression = ParseExpression();

        Consume(
            TokenType.Semicolon,
            "Expected ';' after expression.");

        return new ExpressionStatementNode(expression);
    }

    private ExpressionNode ParseExpression()
    {
        return ParseEquality();
    }

    private ExpressionNode ParseEquality()
    {
        var expression = ParseAdditive();

        while (Match(
            TokenType.EqualEqual,
            TokenType.NotEqual))
        {
            var operatorToken = Previous;
            var right = ParseAdditive();

            expression = new BinaryExpressionNode(
                expression,
                operatorToken.Lexeme,
                right);
        }

        return expression;
    }

    private ExpressionNode ParseAdditive()
    {
        var expression = ParseMultiplicative();

        while (Match(
            TokenType.Plus,
            TokenType.Minus))
        {
            var operatorToken = Previous;
            var right = ParseMultiplicative();

            expression = new BinaryExpressionNode(
                expression,
                operatorToken.Lexeme,
                right);
        }

        return expression;
    }

    private ExpressionNode ParseMultiplicative()
    {
        var expression = ParseUnary();

        while (Match(
            TokenType.Star,
            TokenType.Slash))
        {
            var operatorToken = Previous;
            var right = ParseUnary();

            expression = new BinaryExpressionNode(
                expression,
                operatorToken.Lexeme,
                right);
        }

        return expression;
    }

    private ExpressionNode ParseUnary()
    {
        if (Match(
            TokenType.Bang,
            TokenType.Minus,
            TokenType.Plus))
        {
            var operatorToken = Previous;
            var operand = ParseUnary();

            return new UnaryExpressionNode(
                operatorToken.Lexeme,
                operand);
        }

        return ParseCall();
    }

    private ExpressionNode ParseCall()
    {
        var expression = ParsePrimary();

        while (Match(TokenType.LeftParen))
        {
            expression = FinishCall(expression);
        }

        return expression;
    }

    private CallExpressionNode FinishCall(
        ExpressionNode callee)
    {
        var arguments = new List<ExpressionNode>();

        if (!Check(TokenType.RightParen))
        {
            do
            {
                arguments.Add(ParseExpression());
            }
            while (Match(TokenType.Comma));
        }

        Consume(
            TokenType.RightParen,
            "Expected ')' after arguments.");

        return new CallExpressionNode(
            callee,
            arguments);
    }

    private ExpressionNode ParsePrimary()
    {
        if (Match(TokenType.IntegerLiteral))
        {
            var token = Previous;

            if (!int.TryParse(
                    token.Lexeme,
                    out var value))
            {
                throw Error(
                    token,
                    $"Invalid integer literal '{token.Lexeme}'.");
            }

            return new IntegerLiteralExpressionNode(value);
        }

        if (Match(TokenType.StringLiteral))
        {
            return new StringLiteralExpressionNode(
                Previous.Lexeme);
        }

        if (Match(TokenType.TrueKeyword))
        {
            return new BooleanLiteralExpressionNode(true);
        }

        if (Match(TokenType.FalseKeyword))
        {
            return new BooleanLiteralExpressionNode(false);
        }

        if (Match(TokenType.Identifier))
        {
            return new IdentifierExpressionNode(
                Previous.Lexeme);
        }

        if (Match(TokenType.LeftParen))
        {
            var expression = ParseExpression();

            Consume(
                TokenType.RightParen,
                "Expected ')' after expression.");

            return expression;
        }

        throw Error(
            Current,
            $"Unexpected token '{Current.Lexeme}'. " +
            "Expected an expression.");
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (!Check(type))
            {
                continue;
            }

            Advance();
            return true;
        }

        return false;
    }

    private Token Consume(
        TokenType type,
        string message)
    {
        if (Check(type))
        {
            return Advance();
        }

        throw Error(Current, message);
    }

    private bool Check(TokenType type)
    {
        return Current.Type == type;
    }

    private Token Advance()
    {
        if (!Check(TokenType.EndOfFile))
        {
            _current++;
        }

        return Previous;
    }

    private Token Current =>
        _tokens[_current];

    private Token Previous =>
        _tokens[_current - 1];

    private ParserException Error(
        Token token,
        string message)
    {
        return new ParserException(
            $"{message} Position: {token.Position}.");
    }
}