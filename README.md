# KLang

KLang is a small educational programming language and compiler built to learn how compilers, data structures, abstract syntax trees, intermediate representations, native code generation, and runtimes work.

The project will intentionally grow in milestones, with each milestone adding a small, testable part of the language and compiler pipeline.

## Milestone 1 — Lexer

The first milestone introduced lexical analysis.

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

* `print`
* integer literals
* `(` and `)`
* `;`
* end-of-file

## Milestone 2 — Parser and AST

The second milestone adds syntax analysis and an Abstract Syntax Tree (AST).

Current pipeline:

```text
KLang source
    |
    v
  Lexer
    |
    v
  Tokens
    |
    v
  Parser
    |
    v
  AST
```

For example:

```text
print(42);
```

is represented as:

```text
ProgramNode
└── PrintStatementNode
    └── IntegerLiteralExpressionNode
        └── Value = 42
```

### AST Structure

The AST currently contains:

* `ProgramNode`
* `StatementNode`
* `PrintStatementNode`
* `ExpressionStatementNode`
* `ExpressionNode`
* `IntegerLiteralExpressionNode`
* `StringLiteralExpressionNode`
* `BooleanLiteralExpressionNode`
* `IdentifierExpressionNode`
* `BinaryExpressionNode`
* `UnaryExpressionNode`
* `CallExpressionNode`

The current lexer/parser implementation actively supports:

* `print` statements
* integer literals
* multiple statements
* parentheses
* semicolons
* parser error reporting with source positions
* end-of-file handling

The additional AST nodes are defined in preparation for future language features.

### Example

Source:

```text
print(42);
print(100);
```

AST:

```text
ProgramNode
├── PrintStatementNode
│   └── IntegerLiteralExpressionNode
│       └── Value = 42
└── PrintStatementNode
    └── IntegerLiteralExpressionNode
        └── Value = 100
```

### Parser Tests

Parser tests exercise the complete pipeline:

```text
Source
  |
  v
Lexer
  |
  v
Tokens
  |
  v
Parser
  |
  v
AST
```

The tests cover:

* parsing a `print` statement
* integer literals
* multiple statements
* whitespace
* missing semicolons
* missing closing parentheses
* missing expressions
* unexpected tokens
* end-of-file handling

## Run

From the repository root:

```powershell
dotnet test
```

Run the CLI:

```powershell
dotnet run --project .\src\KLang.Cli
```

## Roadmap

Future milestones will progressively introduce:

```text
Lexer
  |
  v
Parser
  |
  v
AST
  |
  v
Semantic Analysis
  |
  v
Intermediate Representation
  |
  v
Code Generation
  |
  v
Runtime
```

The language will be expanded incrementally with features such as:

* expressions
* variables
* operators
* functions
* control flow
* semantic analysis
* intermediate representation
* code generation
* runtime concepts

The goal is to keep each milestone small, understandable, testable, and representative of how real language implementations evolve.
