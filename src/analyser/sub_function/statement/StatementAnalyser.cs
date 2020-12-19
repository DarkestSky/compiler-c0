using System;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;
using compiler_c0.analyser.sub_function;
using compiler_c0.analyser.sub_function.expression;
using compiler_c0.instruction;
using compiler_c0.symbol_manager;
using compiler_c0.symbol_manager.value_type;

namespace compiler_c0.analyser.sub_function
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
            ExpressionAnalyser.AnalyseExpression();
            AnalyseBlockStatement(true);
            if (Tokenizer.PeekToken().Is(TokenType.Else))
            {
                Tokenizer.ExpectToken(TokenType.Else);
                if (Tokenizer.PeekToken().Is(TokenType.LBrace))
                {
                    AnalyseBlockStatement(true);
                }
                else
                {
                    AnalyseIfStatement();
                }
            }
        }

        public static void AnalyseWhileStatement()
        {
            Tokenizer.ExpectToken(TokenType.While);
            ExpressionAnalyser.AnalyseExpression();
            AnalyseBlockStatement(true);
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
            ExpressionAnalyser.AnalyseExpression();
            Tokenizer.ExpectToken(TokenType.Semicolon);
        }

        public static void AnalyseEmptyStatement()
        {
            Tokenizer.ExpectToken(TokenType.Semicolon);
        }
    }
}