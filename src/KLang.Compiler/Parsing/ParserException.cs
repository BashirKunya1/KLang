namespace KLang.Compiler.Parsing;

public sealed class ParserException : Exception
{
    public ParserException(string message)
        : base(message)
    {
    }
}
