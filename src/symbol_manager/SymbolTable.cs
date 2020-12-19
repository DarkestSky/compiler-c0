using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using compiler_c0.symbol_manager.symbol;

namespace compiler_c0.symbol_manager
{
    public class SymbolTable
    {
        protected readonly List<KeyValuePair<string, Variable>> Variables = new();
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;

        public virtual void AddSymbol(string name, Symbol symbol)
        {
            if (symbol is Variable variable)
            {
                Variables.Add(new KeyValuePair<string, Variable>(name, variable));
                SymbolManager.CurFunction.AddVariable(variable.ValueType);
            }
            else
            {
                throw new Exception("unexpected symbol type in common symbol table");
            }
        }

        /// <summary>
        /// find a variable, return false when not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Symbol FindSymbol(string name)
        {
            return FindVariable(name, out _);
        }
        
        public Variable FindVariable(string name, out int offset)
        {
            for (var i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].Key == name)
                {
                    offset = i;
                    return Variables[i].Value;
                }
            }

            offset = -1;
            return null;
        }
        
        public int FindVariable(Variable variable)
        {
            for (var i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].Value == variable)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}