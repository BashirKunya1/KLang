namespace KLang.Compiler.Ast;

// UnaryExpressionNode
// -42
// -x
// !enabled
public sealed record UnaryExpressionNode(
    string Operator,
    ExpressionNode Operand) : ExpressionNode;
