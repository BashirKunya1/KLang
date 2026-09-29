namespace KLang.Compiler.Ast;

public sealed record ExpressionStatementNode(
    ExpressionNode Expression) : StatementNode;
