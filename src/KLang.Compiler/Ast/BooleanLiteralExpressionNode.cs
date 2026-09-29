namespace KLang.Compiler.Ast;

public sealed record BooleanLiteralExpressionNode(
    bool Value) : ExpressionNode;
