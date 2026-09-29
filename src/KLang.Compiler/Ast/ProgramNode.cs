namespace KLang.Compiler.Ast;

public sealed record ProgramNode(
    IReadOnlyList<StatementNode> Statements) : AstNode;
