using System;
using compiler_c0.tokenizer.token;

namespace compiler_c0.analyser.sub_function.expression
{
    public static class ExpressionCombiner
    {
        public static ExpressionValue Combine(ExpressionValue lValue, Token token, ExpressionValue rValue)
        {
            switch (token.TokenType)
            {
                case TokenType.Plus:
                    return CombinePlus(lValue, rValue);
                case TokenType.Minus:
                    return CombineMinus(lValue, rValue);
                case TokenType.Mul:
                    return CombineMul(lValue, rValue);
                case TokenType.Div:
                    return CombineDiv(lValue, rValue);
                case TokenType.Assign:
                    return CombineAssign(lValue, rValue);
                case TokenType.Eq:
                    return CombineEq(lValue, rValue);
                case TokenType.Neq:
                    return CombineNeq(lValue, rValue);
                case TokenType.Lt:
                    return CombineLt(lValue, rValue);
                case TokenType.Le:
                    return CombineLe(lValue, rValue);
                case TokenType.Gt:
                    return CombineGt(lValue, rValue);
                case TokenType.Ge:
                    return CombineGe(lValue, rValue);
            }

            throw new Exception("unexpected operator type");
        }

        private static ExpressionValue CombinePlus(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineMinus(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineMul(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineDiv(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineAssign(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineEq(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineNeq(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineLt(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineLe(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineGt(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
        
        private static ExpressionValue CombineGe(ExpressionValue lValue, ExpressionValue rValue)
        {
            return lValue;
        }
    }
}