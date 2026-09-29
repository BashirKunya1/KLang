namespace KLang.Compiler.Ast;

// For expressions such as:
// 10 + 20
// x * 5
// a == b
// x && y

public sealed record BinaryExpressionNode(
    ExpressionNode Left,
    string Operator,
    ExpressionNode Right) : ExpressionNode;
