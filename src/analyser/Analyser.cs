using compiler_c0.analyser.sub_function.statement;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;

namespace compiler_c0.analyser
{
    public class Analyser
    {
        private Analyser()
        {
        }

        private static Analyser _instance;

        public static Analyser Instance
        {
            get { return _instance ??= new Analyser(); }
        }

        public void Analyse()
        {
            var tokenizer = Tokenizer.Instance;

            while (tokenizer.PeekToken().Is(TokenType.Let, TokenType.Const))
            {
                StatementAnalyser.AnalyseDeclarationStatement();
            }

            while (tokenizer.PeekToken().Is(TokenType.Fn))
            {
                FunctionAnalyser.AnalyseFunction();
            }

            tokenizer.ExpectToken(TokenType.Eof);
        }
    }
}