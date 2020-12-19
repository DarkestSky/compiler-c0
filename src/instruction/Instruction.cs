using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_c0.instruction.extension;

namespace compiler_c0.instruction
{
    public class Instruction
    {
        private readonly InstructionType _type;
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
            SetParam(param);
        }

        public void SetParam(uint param)
        {
            if ((_valueSize = _type.GetParamSize()) != 1)
            {
                throw new Exception("not matched instruction type");
            }

            _value = BitConverter.GetBytes(param);
        }

        public Instruction(InstructionType type, int param) : this(type, (uint) param)
        {
        }

        public void SetParam(int param) => SetParam((uint) param);

        public Instruction(InstructionType type, ulong param)
        {
            _type = type;
            SetParam(param);
        }

        public void SetParam(ulong param)
        {
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
            if (_type.ToString().Length < 3)
                s.Append('\t');
            switch (_valueSize)
            {
                case 1:
                    s.Append("\tParamType: U32Int\tParam: ");
                    if (_type.IsSigned())
                        s.Append(BitConverter.ToInt32(_value));
                    else
                        s.Append(BitConverter.ToUInt32(_value));

                    break;
                case 2:
                    s.Append($"\tParamType: U64Int\tParam: {BitConverter.ToUInt64(_value)}");
                    break;
            }

            return s.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            var result = Enumerable.Empty<byte>();
            result = result.Append((byte) _type);
            
            switch (_valueSize)
            {
                case 1:
                case 2:
                    result = result.Concat(_value.Reverse());
                    break;
            }

            return result;
        }
    }
}