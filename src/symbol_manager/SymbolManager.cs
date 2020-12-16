using System;
using System.Collections.Generic;
using System.Linq;
using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class SymbolManager
    {
        private readonly List<SymbolTable> _symbolTables = new();

        private SymbolTable CurSymbolTable => _symbolTables.Last();

        public SymbolManager()
        {
            // put a global symbol table to the bottom
            _symbolTables.Add(new GlobalSymbolTable());
        }
        
        public Symbol FindSymbol(string name)
        {
            for (var i = _symbolTables.Count; i >= 0; i--)
            {
                if (_symbolTables[i].FindSymbol(name) != null)
                {
                    return _symbolTables[i].FindSymbol(name);
                }
            }

            return null;
        }

        public bool IsDefined(string name)
        {
            return CurSymbolTable.FindSymbol(name) != null;
        }

        public Variable NewVariable(string name, ValueType type)
        {
            // alloc offset according to TYPE
            var variable = new Variable();
            
            CurSymbolTable.AddSymbol(name, variable);

            return variable;
        }

        public Function NewFunction(string name, ValueType returnType)
        {
            if (CurSymbolTable ! is GlobalSymbolTable)
            {
                throw new Exception("function is not allowed defining here");
            }

            var function = new Function();
            
            CurSymbolTable.AddSymbol(name, function);
            
            return function;
        }
    }
}