using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_c0.symbol_manager.symbol;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager
{
    public class GlobalSymbolTable : SymbolTable
    {
        // private readonly List<KeyValuePair<string, Variable>> Variables = new();

        // ReSharper disable once InconsistentNaming
        private readonly List<KeyValuePair<string, Function>> Functions = new();

        // ReSharper disable once InconsistentNaming
        private readonly List<KeyValuePair<string, LibFunction>> LibFunctions = new();

        public override void AddSymbol(string name, Symbol symbol)
        {
            if (symbol is Variable variable)
            {
                Variables.Add(new KeyValuePair<string, Variable>(name, variable));
            }
            else if (symbol is LibFunction libFunction)
            {
                LibFunctions.Add(new KeyValuePair<string, LibFunction>(name, libFunction));
                libFunction.Name = (uint) Variables.Count - 1;
            }
            else if (symbol is Function function)
            {
                Functions.Add(new KeyValuePair<string, Function>(name, function));
                function.Name = (uint) Variables.Count - 1;
            }
            else
            {
                throw new Exception("unexpected symbol type in global symbol table");
            }
        }

        public int NewGlobalString(string value)
        {
            var n = $"str({Variables.Count})";
            var v = new Variable(ValueType.String, true);
            v.SetValue(value);
            
            Variables.Add(new KeyValuePair<string, Variable>(n, v));
            return Variables.Count - 1;
        }

        public int FindVariable(Variable variable)
        {
            for (var i = 0; i < Variables.Capacity; i++)
            {
                if (Variables[i].Value == variable)
                {
                    return i;
                }
            }

            return -1;
        }

        public Function FindFunction(string name, out int offset)
        {
            foreach (var pair in LibFunctions)
            {
                if (pair.Key == name)
                {
                    offset = -1;
                    return pair.Value;
                }
            }
            
            for (var i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].Key == name)
                {
                    offset = i;
                    return Functions[i].Value;
                }
            }

            offset = -1;
            return null;
        }

        public override Symbol FindSymbol(string name)
        {
            // todo find lib function
            return (Symbol) FindVariable(name, out _) ?? FindFunction(name, out _);
        }

        public void NewLibFunction(string name, ValueType returnType, params ValueType[] paramType)
        {
            var variable = new Variable(ValueType.String, true);
            Variables.Add(new KeyValuePair<string, Variable>($"lib({name})", variable));
            variable.SetValue(name);

            var libFunction = new LibFunction(returnType, paramType);
            AddSymbol(name, libFunction);
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
            var head = Enumerable.Empty<byte>()
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