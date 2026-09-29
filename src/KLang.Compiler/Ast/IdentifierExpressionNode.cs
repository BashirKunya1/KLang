namespace KLang.Compiler.Ast;

//This represents things such as:
//name
//counter
//userName
public sealed record IdentifierExpressionNode(
    string Name) : ExpressionNode;
