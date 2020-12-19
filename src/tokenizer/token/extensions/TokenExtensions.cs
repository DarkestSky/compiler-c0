using System;
using System.Collections.Generic;

namespace compiler_c0.tokenizer.token.extensions
{
    public static class TokenExtensions
    {
        private static readonly HashSet<TokenType> BinaryOperatorSet = new()
        {
            TokenType.Plus, TokenType.Minus, TokenType.Mul, TokenType.Div, TokenType.Assign,
            TokenType.Eq, TokenType.Neq, TokenType.Lt, TokenType.Le, TokenType.Gt, TokenType.Ge
        };
        
        public static bool IsBinaryOperator(this Token t)
        {
            return BinaryOperatorSet.Contains(t.TokenType);
        }

        public static bool IsRightAssoc(this Token t)
        {
            return t.TokenType switch
            {
                TokenType.Assign => true,
                _ => false
            };
        }

        public static int GetPriority(this Token t)
        {
            return t.TokenType switch
            {
                TokenType.Plus   => 3,
                TokenType.Minus  => 3,
                TokenType.Mul    => 4,
                TokenType.Div    => 4,
                TokenType.Assign => 1,
                TokenType.Eq     => 2,
                TokenType.Neq    => 2,
                TokenType.Lt     => 2,
                TokenType.Le     => 2,
                TokenType.Gt     => 2,
                TokenType.Ge     => 2,
                _ => throw new Exception("cannot get opg-priority of a non-binary-operator")
            };
        }
    }
}