# KLang

KLang is a small educational programming language and compiler written in C#. The project explores how compilers work, from lexical analysis and parsing to abstract syntax trees, semantic analysis, intermediate representations, native code generation, and runtimes.

Development is organized into incremental milestones. Each milestone introduces a small, testable part of the language and compiler pipeline.

## Milestone 1 — Lexer

The first milestone introduced lexical analysis, which converts source code into a sequence of tokens.

### Pipeline

```text
KLang source
     |
     v
   Lexer
     |
     v
   Tokens
```

### Supported tokens

The lexer recognizes:

- **Keywords:** `print`, `true`, `false`
- **Literals:** integer and string literals
- **Identifiers:** variable and function names
- **Arithmetic operators:** `+`, `-`, `*`, `/`, `%`
- **Comparison operators:** `<`, `<=`, `>`, `>=`
- **Equality operators:** `==`, `!=`
- **Logical operators:** `&&`, `||`, `!`
- **Assignment operator:** `=`
- **Punctuation:** `(`, `)`, `,`, `;`
- **End of file:** `EndOfFile`

The lexer also supports whitespace, single-line comments (`//`), multiline comments (`/* ... */`), and string escape sequences such as `\n`, `\r`, `\t`, `\\`, and `\"`.

Invalid characters, malformed escape sequences, unterminated strings, and unterminated block comments produce lexer errors.

## Milestone 2 — Parser and Abstract Syntax Tree

The second milestone introduces syntax analysis and an Abstract Syntax Tree (AST).

The parser consumes tokens produced by the lexer and constructs a tree representing the structure of a KLang program.

### Pipeline

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

### Example

Source:

```klang
print(42);
print(100);
```

AST representation:

```text
ProgramNode
├── PrintStatementNode
│   └── IntegerLiteralExpressionNode
│       └── Value = 42
└── PrintStatementNode
    └── IntegerLiteralExpressionNode
        └── Value = 100
```

### AST structure

The current AST includes the following node types:

**Program and statements**

- `AstNode`
- `ProgramNode`
- `StatementNode`
- `PrintStatementNode`
- `ExpressionStatementNode`

**Expressions**

- `ExpressionNode`
- `IntegerLiteralExpressionNode`
- `StringLiteralExpressionNode`
- `BooleanLiteralExpressionNode`
- `IdentifierExpressionNode`
- `BinaryExpressionNode`
- `UnaryExpressionNode`
- `CallExpressionNode`

These nodes represent programs, statements, literal values, identifiers, operators, and function calls.

### Supported syntax

The lexer and parser support:

- `print(...)` statements
- Integer, string, and Boolean literals
- Identifiers and expression statements
- Multiple statements
- Parenthesized expressions
- Function calls with multiple arguments
- Nested function calls
- Unary expressions
- Arithmetic expressions
- Comparison and equality expressions
- Logical expressions
- Operator precedence and left associativity
- Semicolon and parenthesis validation
- End-of-file handling
- Lexer and parser errors with source positions

The assignment operator `=` is recognized by the lexer, but assignment statements and variable declarations are not yet implemented.

### Operator precedence

The parser uses recursive-descent parsing with precedence levels.

From lowest to highest:

| Level | Operators | Meaning |
|---|---|---|
| 1 | `||` | Logical OR |
| 2 | `&&` | Logical AND |
| 3 | `==`, `!=` | Equality |
| 4 | `<`, `<=`, `>`, `>=` | Comparison |
| 5 | `+`, `-` | Addition and subtraction |
| 6 | `*`, `/`, `%` | Multiplication, division, modulo |
| 7 | `!`, unary `-`, unary `+` | Unary expressions |
| 8 | Function calls, literals, parentheses | Primary expressions and calls |

For example:

```klang
print(1 + 2 * 3);
```

The parser represents multiplication as a child of addition, reflecting the higher precedence of `*`.

```text
PrintStatementNode
└── BinaryExpressionNode (+)
    ├── IntegerLiteralExpressionNode (1)
    └── BinaryExpressionNode (*)
        ├── IntegerLiteralExpressionNode (2)
        └── IntegerLiteralExpressionNode (3)
```

Parsing constructs the AST; it does not yet execute these expressions.

### Parser tests

The automated tests cover the lexer-to-parser pipeline:

```text
Source code
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

Tests cover token recognition, literals, identifiers, arithmetic and logical operators, comments, string escapes, multiple statements, nested calls, operator precedence, associativity, malformed expressions, missing delimiters, and unexpected characters.

The test suite is maintained in `tests/KLang.Compiler.Tests`.

## Running KLang

### Run the tests

From the repository root:

```powershell
dotnet test
```

### Run the CLI

```powershell
dotnet run --project .\src\KLang.Cli
```

## Roadmap

The next milestones will extend the compiler incrementally.

### Milestone 3 — Semantic Analysis

Planned capabilities:

- Symbol tables and identifier resolution
- Undefined identifier detection
- Variable and function declarations
- Scope management
- Expression type inference and type checking
- Semantic diagnostics

### Milestone 4 — Intermediate Representation

Introduce an intermediate representation (IR) between semantic analysis and code generation.

### Milestone 5 — Code Generation

Explore translating validated programs or intermediate representations into executable code or native object code.

### Milestone 6 — Runtime

Explore runtime support, program execution, and integration with generated code.

### Future language features

As the compiler evolves, the language may introduce:

- Variable declarations and assignment
- Functions and parameters
- Control flow and conditional statements
- Additional data types
- Semantic validation
- Intermediate representation
- Native code generation
- Runtime concepts

## Project philosophy

KLang is a learning project intended to make compiler construction practical and understandable.

The guiding principles are:

- Incremental development
- Clear separation of compiler stages
- Small, testable implementations
- Automated regression tests
- Explicit language and compiler design decisions

The long-term goal is to build a working compiler while understanding each stage of the compilation process.
