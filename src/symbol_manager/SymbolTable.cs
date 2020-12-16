using System;
using System.Collections.Generic;
using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class SymbolTable
    {
        protected readonly Dictionary<string, Symbol> Symbols = new();

        public void AddVariable()
        {
            
        }

        public virtual void AddSymbol(string name, Symbol symbol)
        {
        }

        /// <summary>
        /// find a variable, return false when not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol FindSymbol(string name)
        {
            return Symbols.GetValueOrDefault(name, null);
        }
    }
}