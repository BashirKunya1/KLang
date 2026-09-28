namespace KLang.Compiler.Lexing;

public sealed record Token(TokenType Type, string Lexeme, int Position);
