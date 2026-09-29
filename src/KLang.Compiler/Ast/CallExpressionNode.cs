namespace KLang.Compiler.Ast;

// For function calls such as:
// print(42)
// calculate(x, 10)
// getName()
// object.method(42)
// getFunction()(42)
public sealed record CallExpressionNode(
    ExpressionNode Callee,
    IReadOnlyList<ExpressionNode> Arguments) : ExpressionNode;
