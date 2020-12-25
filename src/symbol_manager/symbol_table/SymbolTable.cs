using System;
using System.Collections.Generic;
using compiler_c0.symbol_manager.symbol;
using ValueType = compiler_c0.symbol_manager.symbol.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol_table
{
    public class SymbolTable
    {
        protected readonly List<KeyValuePair<string, Variable>> Variables = new();

        public int VariableCount => Variables.Count;

        // ReSharper disable once InconsistentNaming
        private readonly List<KeyValuePair<string, Param>> Params = new();
        
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;

        public virtual void AddSymbol(string name, Symbol symbol)
        {
            if (symbol is Variable variable)
            {
                Variables.Add(new KeyValuePair<string, Variable>(name, variable));
                SymbolManager.CurFunction.AddVariable(variable.ValueType);
            }
            else if (symbol is Param param)
            {
                Params.Add(new KeyValuePair<string, Param>(name, param));
                SymbolManager.CurFunction.AddParam(param.ValueType);
            }
            else
            {
                throw new Exception("unexpected symbol type in common symbol table");
            }
        }

        public void SetReturnParam(ValueType type)
        {
            Params.Insert(0, new KeyValuePair<string, Param>("return()", new Param(type, false)));
        }

        /// <summary>
        /// find a variable, return false when not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Symbol FindSymbol(string name)
        {
            return (Symbol) FindVariable(name, out _) ?? FindParam(name, out _);
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

        public Param FindParam(string name, out int offset)
        {
            for (var i = 0; i < Params.Count; i++)
            {
                if (Params[i].Key == name)
                {
                    offset = i;
                    return Params[i].Value;
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
        
        public int FindParam(Param variable)
        {
            for (var i = 0; i < Params.Count; i++)
            {
                if (Params[i].Value == variable)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}