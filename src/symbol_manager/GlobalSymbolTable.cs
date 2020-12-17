using System;
using System.Collections.Generic;
using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class GlobalSymbolTable: SymbolTable
    {
        private readonly List<Variable> Variables = new();
        private readonly List<Function> Functions = new();
        
        public override void AddSymbol(string name, Symbol symbol)
        {
            Symbols.Add(name, symbol);
            
            if (symbol is Variable variable)
            {
                Variables.Add(variable);
            }
            else if (symbol is Function function)
            {
                Functions.Add(function);
            }
            else
            {
                throw new Exception("unexpected symbol type in global symbol table");
            }
        }
    }
}