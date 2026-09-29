
using global::KLang.Compiler.Ast;
using global::KLang.Compiler.Lexing;

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

        throw Error(
            Current,
            $"Unexpected token '{Current.Lexeme}'. Expected a statement.");
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

    private ExpressionNode ParseExpression()
    {
        return ParsePrimaryExpression();
    }

    private ExpressionNode ParsePrimaryExpression()
    {
        if (Match(TokenType.IntegerLiteral))
        {
            var token = Previous;

            if (!int.TryParse(token.Lexeme, out var value))
            {
                throw Error(
                    token,
                    $"Invalid integer literal '{token.Lexeme}'.");
            }

            return new IntegerLiteralExpressionNode(value);
        }

        throw Error(
            Current,
            $"Unexpected token '{Current.Lexeme}'. Expected an expression.");
    }

    private bool Match(TokenType type)
    {
        if (!Check(type))
            return false;

        Advance();
        return true;
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
            return Advance();

        throw Error(Current, message);
    }

    private bool Check(TokenType type)
    {
        return Current.Type == type;
    }

    private Token Advance()
    {
        if (!Check(TokenType.EndOfFile))
            _current++;

        return Previous;
    }

    private Token Current => _tokens[_current];

    private Token Previous => _tokens[_current - 1];

    private ParserException Error(Token token, string message)
    {
        return new ParserException(
            $"{message} Position: {token.Position}.");
    }
}
