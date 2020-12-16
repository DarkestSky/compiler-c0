using System;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;
using compiler_c0.analyser.sub_function;
using compiler_c0.analyser.sub_function.expression;

namespace compiler_c0.analyser.sub_function
{
    public static class StatementAnalyser
    {
        private static readonly Tokenizer Tokenizer = Tokenizer.Instance;

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
                    AnalyseBlockStatement();
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
            Tokenizer.ExpectToken(TokenType.Assign);
            ExpressionAnalyser.AnalyseExpression();
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
            AnalyseBlockStatement();
            if (Tokenizer.PeekToken().Is(TokenType.Else))
            {
                Tokenizer.ExpectToken(TokenType.Else);
                if (Tokenizer.PeekToken().Is(TokenType.LBrace))
                {
                    AnalyseBlockStatement();
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
            AnalyseBlockStatement();
        }

        public static void AnalyseBlockStatement()
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