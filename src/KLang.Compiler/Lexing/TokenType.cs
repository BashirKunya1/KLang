namespace KLang.Compiler.Lexing;

public enum TokenType
{
    // Keywords
    PrintKeyword,
    TrueKeyword,
    FalseKeyword,

    // Literals
    IntegerLiteral,
    StringLiteral,

    // Identifiers
    Identifier,

    // Operators
    Plus,
    Minus,
    Star,
    Slash,

    Bang,
    EqualEqual,
    NotEqual,

    // Punctuation
    LeftParen,
    RightParen,
    Comma,
    Semicolon,

    // Special
    EndOfFile
}
