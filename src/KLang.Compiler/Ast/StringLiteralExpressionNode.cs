namespace KLang.Compiler.Ast;

public sealed record StringLiteralExpressionNode(
    string Value) : ExpressionNode;
