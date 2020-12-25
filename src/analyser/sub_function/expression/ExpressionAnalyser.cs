using System;
using System.Collections.Generic;
using compiler_c0.symbol_manager;
using compiler_c0.symbol_manager.instruction;
using compiler_c0.symbol_manager.symbol;
using compiler_c0.symbol_manager.symbol.value_type;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;
using compiler_c0.tokenizer.token.extensions;
using ValueType = compiler_c0.symbol_manager.symbol.value_type.ValueType;

namespace compiler_c0.analyser.sub_function.expression
{
    public static class ExpressionAnalyser
    {
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;
        public static ExpressionValue AnalyseExpression()
        {
            var lValue = ParsePrimary();
            return ParseOpg(lValue, 0);
        }

        private static readonly Tokenizer Tokenizer = Tokenizer.Instance;

        private static ExpressionValue ParsePrimary()
        {
            ExpressionValue value;
            if (Tokenizer.PeekToken().Is(TokenType.Minus))
            {
                value = AnalyseNegExpression();
            }
            else if (Tokenizer.PeekToken().Is(TokenType.LParen))
            {
                Tokenizer.ExpectToken(TokenType.LParen);
                value = AnalyseExpression();
                Tokenizer.ExpectToken(TokenType.RParen);
            }
            else if (Tokenizer.PeekToken().Is(TokenType.Identifier))
            {
                value = AnalyseIdentOrCallExpression();
            }
            else if (Tokenizer.PeekToken().IsLiteral())
            {
                value = AnalyseLiteralExpression();
            }
            else if (Tokenizer.PeekToken().Is(TokenType.Semicolon))
            {
                value = new ExpressionValue(ValueType.Void);
            }
            else
            {
                throw new Exception("unreachable code");
            }

            // analyse As
            while (Tokenizer.PeekToken().Is(TokenType.As))
            {
                value =  AnalyseAsExpression(value);
            }
            
            return value;
        }

        private static ExpressionValue ParseOpg(ExpressionValue lValue, int priority)
        {
            while (Tokenizer.PeekToken().IsBinaryOperator() && Tokenizer.PeekToken().GetPriority() >= priority)
            {
                var op = Tokenizer.NextToken();
                var rValue = ParsePrimary();
                while (Tokenizer.PeekToken().IsBinaryOperator() &&
                       (Tokenizer.PeekToken().GetPriority() > op.GetPriority() ||
                        Tokenizer.PeekToken().IsRightAssoc() &&
                        Tokenizer.PeekToken().GetPriority() == op.GetPriority()))
                {
                    lValue = ParseOpg(lValue, Tokenizer.PeekToken().GetPriority());
                }

                lValue = ExpressionCombiner.Combine(lValue, op, rValue);
            }
 
            return lValue;
        }

        private static ExpressionValue AnalyseNegExpression()
        {
            Tokenizer.ExpectToken(TokenType.Minus);
            var value =  ParsePrimary();
            switch (value.ValueType)
            {
                case ValueType.Int:
                    SymbolManager.AddInstruction(new Instruction(InstructionType.NegI));
                    break;
                case ValueType.Float:
                    SymbolManager.AddInstruction(new Instruction(InstructionType.NegF));
                    break;
            }
            
            return value;
        }

        private static ExpressionValue AnalyseIdentOrCallExpression()
        {
            var token = Tokenizer.PeekToken();
            var symbol = SymbolManager.FindSymbol((string) token.Value);

            // classify whether symbol is a FUNCTION or a Variable
            if (symbol is Function)
                return AnalyseCallExpression();
            if (symbol is Variable || symbol is Param)
                return AnalyseIdentExpression();
            throw new Exception("symbol not found");
        }

        private static ExpressionValue AnalyseIdentExpression()
        {
            var ident = Tokenizer.ExpectToken(TokenType.Identifier);
            var symbol = SymbolManager.FindSymbol((string) ident.Value);
            SymbolManager.AddLoadAddressInstruction(symbol);

            if (symbol is Variable variable)
            {
                if (!Tokenizer.PeekToken().Is(TokenType.Assign))
                {
                    SymbolManager.AddInstruction(new Instruction(InstructionType.Load64));
                }
                else
                {
                    variable.TryAssign();
                }
                return new ExpressionValue(variable.ValueType);
            }

            if (symbol is Param param)
            {
                if (!Tokenizer.PeekToken().Is(TokenType.Assign))
                {
                    SymbolManager.AddInstruction(new Instruction(InstructionType.Load64));
                }
                else
                {
                    param.TryAssign();
                }
                return new ExpressionValue(param.ValueType);
            }

            throw new Exception("unreachable code");
        }

        private static ExpressionValue AnalyseCallExpression()
        {
            var name = Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.LParen);
            var func = (Function) SymbolManager.FindSymbol((string) name.Value);
            SymbolManager.AddInstruction(new Instruction(InstructionType.StackAlloc, func.ReturnSlot));
            
            AnalyseCallParamList(func);
            Tokenizer.ExpectToken(TokenType.RParen);

            int offset;
            if (func is LibFunction)
            {
                SymbolManager.GlobalSymbolTable.FindVariable($"lib({(string) name.Value})", out offset);
            }
            else
            {
                SymbolManager.GlobalSymbolTable.FindVariable($"fun({(string) name.Value})", out offset);
            }

            SymbolManager.AddInstruction(new Instruction(InstructionType.Callname, (uint) offset));

            return new ExpressionValue(func.ReturnType);
        }

        private static void AnalyseCallParamList(Function func)
        {
            var l = new List<ValueType>();
            if (!Tokenizer.PeekToken().Is(TokenType.RParen))
            {
                AnalyseCallParam(l);
                while (Tokenizer.PeekToken().Is(TokenType.Comma))
                {
                    Tokenizer.ExpectToken(TokenType.Comma);
                    AnalyseCallParam(l);
                }
            }

            if (!func.CheckParamList(l))
            {
                throw new Exception("params not match");
            }
        }

        private static void AnalyseCallParam(ICollection<ValueType> valueTypes)
        {
            var type = AnalyseExpression();
            valueTypes.Add(type.ValueType);
        }

        private static ExpressionValue AnalyseLiteralExpression()
        {
            var token = Tokenizer.NextToken();
            switch (token.TokenType)
            {
                case TokenType.LiteralNumber:
                    SymbolManager.CurFunction.AddInstruction(
                        new Instruction(InstructionType.Push, (ulong) token.Value));
                    return new ExpressionValue(ValueType.Int);
                case TokenType.LiteralDouble:
                    var value = BitConverter.ToUInt64(BitConverter.GetBytes((double) token.Value));
                    SymbolManager.CurFunction.AddInstruction(
                        new Instruction(InstructionType.Push, value));
                    return new ExpressionValue(ValueType.Float);
                case TokenType.LiteralString:
                    var i = SymbolManager.GlobalSymbolTable.NewGlobalString((string) token.Value);
                    SymbolManager.AddInstruction(new Instruction(InstructionType.Push, (ulong) i));
                    return new ExpressionValue(ValueType.Int);
            }

            throw new Exception("unreachable code");
        }

        private static ExpressionValue AnalyseAsExpression(ExpressionValue lValue)
        {
            Tokenizer.ExpectToken(TokenType.As);
            var type = Tokenizer.ExpectToken(TokenType.Identifier).ToValueType();
            if (lValue.ValueType == ValueType.Int  && type == ValueType.Float)
            {
                SymbolManager.AddInstruction(new Instruction(InstructionType.ItoF));
            }
            else if (lValue.ValueType == ValueType.Float && type == ValueType.Int)
            {
                SymbolManager.AddInstruction(new Instruction(InstructionType.FtoI));
            }


            return new ExpressionValue(type);
        }
    }
}