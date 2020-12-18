using System;
using System.Collections.Generic;
using System.Linq;
using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class GlobalSymbolTable: SymbolTable
    {
        private readonly List<KeyValuePair<string, Variable>> Variables = new();
        private readonly List<KeyValuePair<string, Function>> Functions = new();
        
        public override void AddSymbol(string name, Symbol symbol)
        {
            Symbols.Add(name, symbol);
            
            if (symbol is Variable variable)
            {
                Variables.Add(new KeyValuePair<string, Variable>(name, variable));
            }
            else if (symbol is Function function)
            {
                Functions.Add(new KeyValuePair<string, Function>(name, function));
            }
            else
            {
                throw new Exception("unexpected symbol type in global symbol table");
            }
        }

        public int FindVariable(string name)
        {
            for (var i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].Key == name)
                {
                    return i;
                }
            }

            return -1;
        }

        public int FindFunction(string name)
        {
            for (var i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].Key == name)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            //todo
            return Enumerable.Empty<byte>();
        }
    }
}