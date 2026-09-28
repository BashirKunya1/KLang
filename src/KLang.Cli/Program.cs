using KLang.Compiler.Lexing;

const string source = "print(42);";

Console.WriteLine("KLang Milestone 1");
Console.WriteLine($"Source: {source}");
Console.WriteLine();

var lexer = new Lexer(source);
var tokens = lexer.Tokenize();

foreach (var token in tokens)
    Console.WriteLine($"{token.Type,-16} Lexeme='{token.Lexeme}' Position={token.Position}");
