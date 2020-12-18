using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_c0.instruction.extension;

namespace compiler_c0.instruction
{
    public class Instruction
    {
        private InstructionType _type;
        private int _valueSize;
        private byte[] _value;

        public Instruction(InstructionType type)
        {
            _type = type;
            if ((_valueSize = _type.GetParamSize()) != 0)
            {
                throw new Exception("not matched instruction type");
            }

            _value = null;
        }

        public Instruction(InstructionType type, uint param)
        {
            _type = type;
            if ((_valueSize = _type.GetParamSize()) != 1)
            {
                throw new Exception("not matched instruction type");
            }

            _value = BitConverter.GetBytes(param);
        }

        public Instruction(InstructionType type, ulong param)
        {
            _type = type;
            if ((_valueSize = _type.GetParamSize()) != 2)
            {
                throw new Exception("not matched instruction type");
            }

            _value = BitConverter.GetBytes(param);
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append($"Instruction: {_type}");
            switch (_valueSize)
            {
                case 1:
                    s.Append($", ParamType: U32Int, Param:{BitConverter.ToUInt32(_value)}");
                    break;
                case 2:
                    s.Append($", ParamType: U64Int, Param:{BitConverter.ToUInt64(_value)}");
                    break;
            }

            return s.Append('.').ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            return new[] {(byte) _type}.Concat(_value.Reverse());
        }
    }
}