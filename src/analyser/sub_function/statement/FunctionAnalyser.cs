using compiler_c0.symbol_manager;
using compiler_c0.symbol_manager.instruction;
using compiler_c0.symbol_manager.symbol.value_type;
using compiler_c0.tokenizer;
using compiler_c0.tokenizer.token;

namespace compiler_c0.analyser.sub_function.statement
{
    public static class FunctionAnalyser
    {
        private static readonly Tokenizer Tokenizer = Tokenizer.Instance;
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;
        
        public static void AnalyseFunction()
        {
            Tokenizer.ExpectToken(TokenType.Fn);
            var fun = Tokenizer.ExpectToken(TokenType.Identifier);
            var funDef = SymbolManager.NewFunction((string) fun.Value);
            SymbolManager.CreateSymbolTable();
            
            Tokenizer.ExpectToken(TokenType.LParen);
            AnalyseParamList();
            Tokenizer.ExpectToken(TokenType.RParen);
            Tokenizer.ExpectToken(TokenType.Arrow);
            
            var returnType = Tokenizer.ExpectToken(TokenType.Identifier);
            funDef.SetReturnType(returnType.ToValueType());
            StatementAnalyser.AnalyseBlockStatement(false);
            
            SymbolManager.AddInstruction(new Instruction(InstructionType.Ret));
            SymbolManager.DeleteSymbolTable();
            
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
            var isConst = false;
            if (Tokenizer.PeekToken().Is(TokenType.Const))
            {
                isConst = true;
                Tokenizer.ExpectToken(TokenType.Const);
            }

            var ident = Tokenizer.ExpectToken(TokenType.Identifier);
            Tokenizer.ExpectToken(TokenType.Colon);
            var type = Tokenizer.ExpectToken(TokenType.Identifier);

            SymbolManager.NewParam((string) ident.Value, type.ToValueType(), isConst);
        }
    }
}