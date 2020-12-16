using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class GlobalSymbolTable: SymbolTable
    {
        public override void AddSymbol(string name, Symbol symbol)
        {
            Symbols.Add(name, symbol);
        }
    }
}