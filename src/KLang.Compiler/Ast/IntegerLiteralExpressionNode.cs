namespace KLang.Compiler.Ast;

public sealed record IntegerLiteralExpressionNode(
    int Value) : ExpressionNode;
