# KLang — Long-Term Learning and Engineering Roadmap

## Vision

Build KLang, a small programming language and compiler, from source-code tokenisation through semantic analysis, intermediate representation, native code generation, linking, and runtime execution.

Use the project to learn compiler engineering and systems programming through practical implementation, automated tests, documented design decisions, and incremental pull requests.

## Implementation languages

- **C#:** Primary implementation of the compiler and its initial reference pipeline.
- **Go:** Applied selectively to useful toolchain utilities, build orchestration, testing tools, or a justified runtime experiment.
- **Rust:** Applied to selected low-level runtime or VM components and to learn ownership, borrowing, memory safety, and FFI.
- **Assembly:** Used to understand machine instructions, calling conventions, stack frames, generated code, and the path from assembly to object files and executables.

Language boundaries must be explicit. Avoid introducing cross-language dependencies without a concrete learning or engineering benefit.

## Milestones

### 1. Lexer
Convert source characters into tokens. Cover literals, identifiers, operators, punctuation, comments, and lexical diagnostics.

### 2. Parser and AST
Parse tokens into an abstract syntax tree. Cover expressions, calls, precedence, associativity, statements, and syntax diagnostics.

### 3. Semantic analysis
Implement symbol tables, name resolution, declaration validation, scope rules, initial type checking, and semantic diagnostics. Associate resolved symbol and type information with AST nodes where appropriate.

### 4. Intermediate representation
Design a target-independent IR and lower the AST into it. Learn control flow, basic blocks, and the foundations of optimisation.

### 5. Interpreter or virtual machine
Execute a limited KLang program through an interpreter or bytecode VM. Establish a working execution model before native code generation.

### 6. Assembly and native code generation
Choose an initial architecture and ABI. Generate assembly, assemble it into object files, and inspect the output. Document the target-specific assumptions.

### 7. Linking and executable generation
Link object files with required libraries and produce an executable that can run independently of the compiler.

### 8. Runtime and standard library
Implement basic output, runtime errors, value handling, and selected standard-library functionality. Investigate Rust for suitable low-level components.

### 9. Go integration
Identify a real need for Go in the toolchain and implement a useful, testable integration. Possible areas include standalone tooling, build orchestration, or a comparative runtime experiment.

### 10. Optimisation and cross-platform targets
Add measured optimisations, benchmarks, and additional target support. Learn the differences between Windows and Linux executable formats, ABIs, linking, and operating-system interfaces.

## Engineering principles

- Build incrementally; do not skip foundational stages.
- Keep compiler stages separated by clear interfaces.
- Prefer automated tests and reproducible examples.
- Test invalid programs as well as valid programs.
- Document why important architectural decisions were made.
- Use pull requests to keep each change focused and reviewable.
- Measure optimisation improvements instead of assuming they help.
- Introduce Go, Rust, and Assembly when they advance a specific learning or engineering goal.
- Keep the first working end-to-end implementation small before expanding the language.

## Definition of long-term success

A user can write a small KLang program, compile it through a documented pipeline, generate target-specific object code, link it, and run the resulting executable on supported platforms. The repository also demonstrates the compiler concepts and systems-programming techniques learned along the way.