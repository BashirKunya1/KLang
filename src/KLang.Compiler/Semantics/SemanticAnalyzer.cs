
using KLang.Compiler.Ast;

namespace KLang.Compiler.Semantics;

/// <summary>
/// Performs initial semantic analysis on KLang expressions.
/// </summary>
public static class SemanticAnalyzer
{
    public static SemanticType InferType(ExpressionNode expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return expression switch
        {
            IntegerLiteralExpressionNode =>
                SemanticType.Integer,

            StringLiteralExpressionNode =>
                SemanticType.String,

            BooleanLiteralExpressionNode =>
                SemanticType.Boolean,

            IdentifierExpressionNode =>
                SemanticType.Unknown,

            UnaryExpressionNode unary =>
                InferUnaryType(unary),

            BinaryExpressionNode binary =>
                InferBinaryType(binary),

            _ => SemanticType.Unknown
        };
    }

    private static SemanticType InferUnaryType(
        UnaryExpressionNode expression)
    {
        var operandType = InferType(expression.Operand);

        return (expression.Operator, operandType) switch
        {
            ("+" or "-", SemanticType.Integer) =>
                SemanticType.Integer,

            ("!", SemanticType.Boolean) =>
                SemanticType.Boolean,

            _ => SemanticType.Unknown
        };
    }

    private static SemanticType InferBinaryType(
        BinaryExpressionNode expression)
    {
        var leftType = InferType(expression.Left);
        var rightType = InferType(expression.Right);

        // Arithmetic operators require integer operands.
        if (leftType == SemanticType.Integer &&
            rightType == SemanticType.Integer)
        {
            return expression.Operator switch
            {
                "+" or "-" or "*" or "/" or "%" =>
                    SemanticType.Integer,

                "<" or "<=" or ">" or ">=" =>
                    SemanticType.Boolean,

                "==" or "!=" =>
                    SemanticType.Boolean,

                _ => SemanticType.Unknown
            };
        }

        // Equality is supported for matching known types.
        if (expression.Operator is "==" or "!=" &&
            leftType == rightType &&
            leftType is SemanticType.String or SemanticType.Boolean)
        {
            return SemanticType.Boolean;
        }

        // Logical operators require Boolean operands.
        if (leftType == SemanticType.Boolean &&
            rightType == SemanticType.Boolean &&
            expression.Operator is "&&" or "||")
        {
            return SemanticType.Boolean;
        }

        return SemanticType.Unknown;
    }
}
