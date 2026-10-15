
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

    // Arithmetic operators
    Plus,
    Minus,
    Star,
    Slash,
    Percent,

    // Comparison and equality operators
    Equal,
    EqualEqual,
    NotEqual,
    Less,
    LessEqual,
    Greater,
    GreaterEqual,

    // Logical operators
    Bang,
    AndAnd,
    OrOr,

    // Punctuation
    LeftParen,
    RightParen,
    Comma,
    Semicolon,

    // Special
    EndOfFile
}
