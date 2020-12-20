using System;
using compiler_c0.analyser.sub_function.expression;
using compiler_c0.instruction;
using compiler_c0.symbol_manager;
using compiler_c0.symbol_manager.value_type;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.analyser.sub_function.statement
{
    public static class StatementAnalyser
    {
        private static readonly Tokenizer Tokenizer = Tokenizer.Instance;
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;

        public static void AnalyseStatement()
        {
            switch (Tokenizer.PeekToken().TokenType)
            {
                case TokenType.Const:
                    AnalyseConstStatement();
                    break;
                case TokenType.Let:
                    AnalyseLetStatement();
                    break;
                case TokenType.If:
                    AnalyseIfStatement();
                    break;
                case TokenType.While:
                    AnalyseWhileStatement();
                    break;
                case TokenType.Return:
                    AnalyseReturnStatement();
                    break;
                case TokenType.LBrace:
                    AnalyseBlockStatement(true);
                    break;
                case TokenType.Semicolon:
                    AnalyseEmptyStatement();
                    break;
                default:
                    AnalyseExpressionStatement();
                    break;
            }
        }

        public static void AnalyseExpressionStatement()
        {
            ExpressionAnalyser.AnalyseExpression();
            Tokenizer.ExpectToken(TokenType.Semicolon);
        }

        public static void AnalyseDeclarationStatement()
        {
            if (Tokenizer.PeekToken().Is(TokenType.Let))
            {
                AnalyseLetStatement();
            }
            else if (Tokenizer.PeekToken().Is(TokenType.Const))
            {
                AnalyseConstStatement();
            }
            else
            {
                throw new Exception("unreachable code.");
            }
        }

        public static void AnalyseLetStatement()
        {
            Tokenizer.ExpectToken(TokenType.Let);
            var ident = Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.Colon);
            var type = Tokenizer.ExpectToken(TokenType.Identifier);
            var variable = SymbolManager.NewVariable((string) ident.Value, type.ToValueType());
            if (Tokenizer.PeekToken().Is(TokenType.Assign))
            {
                Tokenizer.ExpectToken(TokenType.Assign);

                // load symbol address
                SymbolManager.AddLoadAddressInstruction(variable);

                var e = ExpressionAnalyser.AnalyseExpression();

                if (variable.ValueType != e.ValueType)
                {
                    throw new Exception("value type not match");
                }

                SymbolManager.CurFunction.AddInstruction(new Instruction(InstructionType.Store64));
                variable.Initial();
            }

            Tokenizer.ExpectToken(TokenType.Semicolon);
        }

        public static void AnalyseConstStatement()
        {
            Tokenizer.ExpectToken(TokenType.Const);
            var ident = Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.Colon);
            var type = Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.Assign);
            ExpressionAnalyser.AnalyseExpression();
            Tokenizer.ExpectToken(TokenType.Semicolon);
        }

        public static void AnalyseIfStatement()
        {
            Tokenizer.ExpectToken(TokenType.If);
            var condition = ExpressionAnalyser.AnalyseExpression();

            if (condition.ValueType != ValueType.Int)
                throw new Exception("invalid condition expression");
            
            SymbolManager.AddInstruction(new Instruction(InstructionType.BrTrue, 1));
            var iThrough = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
            AnalyseBlockStatement(true);
            // todo: iThrough should jump to here
            iThrough.SetParam(SymbolManager.GetInstructionOffset(iThrough) + 1);
            
            if (Tokenizer.PeekToken().Is(TokenType.Else))
            {
                var iInnerThrough = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
                Tokenizer.ExpectToken(TokenType.Else);
                if (Tokenizer.PeekToken().Is(TokenType.LBrace))
                {
                    AnalyseBlockStatement(true);
                }
                else
                {
                    AnalyseIfStatement();
                }
                
                // todo: iThrough should jump to here
                iInnerThrough.SetParam(SymbolManager.GetInstructionOffset(iInnerThrough) + 1);
            }
        }

        public static void AnalyseWhileStatement()
        {
            Tokenizer.ExpectToken(TokenType.While);
            
            var brStart = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
            var condition = ExpressionAnalyser.AnalyseExpression();
            if (condition.ValueType != ValueType.Int)
                throw new Exception("invalid condition expression");
            SymbolManager.AddInstruction(new Instruction(InstructionType.BrTrue, 1));
            var brForward = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
            AnalyseBlockStatement(true);
            
            var brBack = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
            brBack.SetParam(SymbolManager.GetInstructionOffset(brBack, brStart));
            brForward.SetParam(SymbolManager.GetInstructionOffset(brForward, brBack) + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createTable">Weather To Create A New Symbol Table</param>
        public static void AnalyseBlockStatement(bool createTable)
        {
            Tokenizer.ExpectToken(TokenType.LBrace);
            while (!Tokenizer.PeekToken().Is(TokenType.RBrace))
            {
                AnalyseStatement();
            }

            Tokenizer.ExpectToken(TokenType.RBrace);
        }

        public static void AnalyseReturnStatement()
        {
            Tokenizer.ExpectToken(TokenType.Return);
            if (SymbolManager.CurFunction.ReturnType != ValueType.Void)
            {
                var symbol = SymbolManager.FindSymbol("return()");
                SymbolManager.AddLoadAddressInstruction(symbol);
            }

            var e = ExpressionAnalyser.AnalyseExpression();
            if (e.ValueType != SymbolManager.CurFunction.ReturnType)
            {
                throw new Exception("unexpected return value");
            }

            if (e.ValueType != ValueType.Void)
            {
                SymbolManager.AddInstruction(new Instruction(InstructionType.Store64));
            }
            Tokenizer.ExpectToken(TokenType.Semicolon);
            SymbolManager.AddInstruction(new Instruction(InstructionType.Ret));
        }

        public static void AnalyseEmptyStatement()
        {
            Tokenizer.ExpectToken(TokenType.Semicolon);
        }
    }
}