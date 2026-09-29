namespace KLang.Compiler.Ast;

public sealed record PrintStatementNode(
    ExpressionNode Expression) : StatementNode;
