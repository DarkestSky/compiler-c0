using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var sb = new StringBuilder();

            sb.AppendLine($"Magic: {0x72303B3Eu}");
            sb.AppendLine($"Version: {0x1}");
            sb.AppendLine($"GlobalDef(Count: {Variables.Count}):");
            foreach (var pair in Variables)
            {
                sb.Append(pair.Value);
            }

            sb.AppendLine($"Functions(Count: {Functions.Count})");
            foreach (var pair in Functions)
            {
                sb.Append(pair.Value);
            }

            return sb.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            var head =  Enumerable.Empty<byte>()
                .Concat(BitConverter.GetBytes(0x72303b3Eu).Reverse())
                .Concat(BitConverter.GetBytes(1).Reverse());

            var globalDef = BitConverter.GetBytes(Variables.Count).Reverse();
            foreach (var pair in Variables)
            {
                globalDef = globalDef.Concat(pair.Value.ToBytes());
            }

            var functionDef = BitConverter.GetBytes(Functions.Count).Reverse();
            foreach (var pair in Functions)
            {
                functionDef = functionDef.Concat(pair.Value.ToBytes());
            }

            return head
                .Concat(globalDef)
                .Concat(functionDef);
        }
    }
}