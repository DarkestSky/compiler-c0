using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_c0.instruction;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class Function : Symbol
    {
        public uint Name { get; set; }

        public uint returnSlot { get; set; }

        private uint param_slots { get; set; }

        private uint loc_slots { get; set; }
        private ValueType ReturnType { get; set; }

        private List<Instruction> Instructions = new();

        private readonly List<Variable> _localVariables = new();

        private readonly List<Variable> _params = new();

        public int FindVariable(Variable variable)
        {
            for (var i = 0; i < _localVariables.Count; i++)
            {
                if (variable == _localVariables[i])
                {
                    return i;
                }
            }

            return -1;
        }

        public void AddVariable(Variable variable)
        {
            _localVariables.Add(variable);
        }
        
        public void SetReturnType(ValueType valueType)
        {
            ReturnType = valueType;
            switch (valueType)
            {
                case ValueType.Void:
                    returnSlot = 0;
                    break;
                case ValueType.Int:
                case ValueType.Float:
                case ValueType.String:
                    returnSlot = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }

        public void AddInstruction(Instruction instruction)
        {
            Instructions.Add(instruction);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tFunction:");
            sb.AppendLine($"\t\tName: {Name}");
            sb.AppendLine($"\t\tReturnSlot: {returnSlot}");
            sb.AppendLine($"\t\tParamSlot: {param_slots}");
            sb.AppendLine($"\t\tLocSlot: {loc_slots}");

            sb.AppendLine("\t\tInstructions:");
            foreach (var instruction in Instructions)
            {
                sb.AppendLine($"\t\t\t{instruction}");
            }

            return sb.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            var nameByte = BitConverter.GetBytes(Name).Reverse();
            var returnByte = BitConverter.GetBytes(returnSlot).Reverse();
            var paramByte = BitConverter.GetBytes(param_slots).Reverse();
            var locByte = BitConverter.GetBytes(loc_slots).Reverse();

            var bodyByte = BitConverter.GetBytes(Instructions.Count).Reverse();
            foreach (var instruction in Instructions)
            {
                bodyByte = bodyByte.Concat(instruction.ToBytes());
            }

            return Enumerable.Empty<byte>()
                .Concat(nameByte)
                .Concat(returnByte)
                .Concat(paramByte)
                .Concat(locByte)
                .Concat(bodyByte);
        }
    }
}