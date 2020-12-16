using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;

namespace compiler_c0.analyser.sub_function
{
    public static class FunctionAnalyser
    {
        private static readonly Tokenizer Tokenizer = Tokenizer.Instance;
        
        public static void AnalyseFunction()
        {
            Tokenizer.ExpectToken(TokenType.Fn);
            Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.LParen);
            AnalyseParamList();
            Tokenizer.ExpectToken(TokenType.RParen);
            Tokenizer.ExpectToken(TokenType.Arrow);
            Tokenizer.ExpectToken(TokenType.Identifier);
            StatementAnalyser.AnalyseBlockStatement();
        }

        private static void AnalyseParamList()
        {
            AnalyseParam();
            while (Tokenizer.PeekToken().Is(TokenType.Comma))
            {
                Tokenizer.ExpectToken(TokenType.Comma);
                AnalyseParam();
            }
        }

        private static void AnalyseParam()
        {
            if (Tokenizer.PeekToken().Is(TokenType.RParen))
                return;
            if (Tokenizer.PeekToken().Is(TokenType.Const))
            {
                Tokenizer.ExpectToken(TokenType.Const);
            }

            var ident = Tokenizer.ExpectToken(TokenType.Identifier);
        }
    }
}