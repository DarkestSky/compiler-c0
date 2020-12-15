using System;

namespace compiler_c0.tokenizer.token.token_extension
{
    public static class TokenTypeExtensions
    {
        public static bool IsBinaryOperator(this TokenType t)
        {
            return t switch
            {
                TokenType.Unknown => false,
                _ => true,
            };
        }

        public static int GetPriority(this TokenType t)
        {
            return t switch
            {
                TokenType.Plus   => 3,
                TokenType.Minus  => 3,
                TokenType.Mul    => 4,
                TokenType.Div    => 4,
                TokenType.Assign => 1,
                TokenType.Eq     => 2,
                TokenType.Neq    => 2,
                TokenType.Lt     => 2,
                TokenType.Gt     => 2,
                TokenType.Ge     => 2,
                _ => throw new Exception("cannot get opg-priority of a non-binary-operator")
            };
        }
    }
}