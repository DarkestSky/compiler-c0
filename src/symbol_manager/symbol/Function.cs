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
        private uint name { get; set; }

        public uint returnSlot { get; set; }

        private uint param_slots { get; set; }

        private uint loc_slots { get; set; }
        private ValueType ReturnType { get; set; }

        private List<Instruction> Instructions;

        public void SetReturnType(ValueType valueType)
        {
            ReturnType = valueType;
            switch (valueType)
            {
                case ValueType.Void:
                    returnSlot = 0;
                    break;
                case ValueType.Int:
                case ValueType.Double:
                case ValueType.String:
                    returnSlot = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name:");
            sb.AppendLine($"ReturnSlot: {returnSlot}");
            sb.AppendLine($"ParamSlot:");
            sb.AppendLine($"LocSlot:");

            sb.AppendLine("Instructions:");
            foreach (var instruction in Instructions)
            {
                sb.AppendLine(instruction.ToString());
            }

            return sb.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            var nameByte = BitConverter.GetBytes(name).Reverse();
            var returnByte = BitConverter.GetBytes(returnSlot).Reverse();
            var paramByte = BitConverter.GetBytes(param_slots).Reverse();
            var locByte = BitConverter.GetBytes(loc_slots).Reverse();

            var bodyByte = BitConverter.GetBytes(Instructions.Count).AsEnumerable();
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