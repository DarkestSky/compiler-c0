using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class Variable : Symbol
    {
        private readonly byte _isConst;

        private bool _initialed;
        public ValueType ValueType { get; set; }
        private byte[] _value;

        public Variable(ValueType type, bool isConst)
        {
            ValueType = type;
            _isConst = isConst ? 1 : 0;
            if (ValueType == ValueType.Int || ValueType == ValueType.Float)
            {
                _value = new byte[8];
            }
        }

        public bool IsConst => _isConst == 1;

        public bool Initialed => _initialed;

        public void Initial()
        {
            _initialed = true;
        }

        public void SetValue(string v)
        {
            _value = Encoding.ASCII.GetBytes(v);
        }

        public void TryAssign()
        {
            if (IsConst)
                throw new Exception("assign is not allowed with const variable");
        }
        
        
        public IEnumerable<byte> ToBytes()
        {
            var result = Enumerable.Empty<byte>();
            result = result.Append(_isConst);

            result = result.Concat(BitConverter.GetBytes(_value.Length).Reverse());
            if (ValueType == ValueType.String)
            {
                result = result.Concat(_value);
            }
            else
            {
                result = result.Concat(_value.Reverse());
            }
            
            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tVariable:");
            sb.AppendLine($"\t\tIsConst: {_isConst}");
            sb.AppendLine($"\t\tValueType: {ValueType}");
            
            switch (ValueType)
            {
                case ValueType.String:
                    sb.Append("\t\tValue:");
                    foreach (var b in _value)
                    {
                        sb.Append($" {b:X2}");
                    }

                    sb.Append($" ({Encoding.Default.GetString(_value)})");
                    
                    sb.AppendLine();
                    
                    break;
            }
            
            return sb.ToString();
        }
    }
}