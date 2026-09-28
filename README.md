# KLang

KLang is a small educational programming language and compiler built to learn how compilers, data structures, intermediate representations, native code generation, and runtimes work.

The project will intentionally grow in milestones.

## Milestone 1

Current pipeline:

```text
KLang source
    |
    v
  Lexer (C#)
    |
    v
  Tokens
```

Supported source form:

```text
print(42);
```

Supported tokens:

- `print`
- integer literals
- `(` and `)`
- `;`
- end-of-file

## Run

```powershell
dotnet test
dotnet run --project .\src\KLang.Cli
```
