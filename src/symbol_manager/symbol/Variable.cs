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
        public ValueType ValueType { get; set; }
        private byte[] _value;

        public Variable(ValueType type, bool isConst)
        {
            ValueType = type;
            _isConst = isConst ? 1 : 0;
        }

        public bool IsConst => _isConst == 1;

        public void SetValue(string v)
        {
            _value = Encoding.ASCII.GetBytes(v);
        }

        public void SetValue(long v)
        {
            
        }

        public void SetValue(double v)
        {
            
        }
        
        public IEnumerable<byte> ToBytes()
        {
            // todo string不需要reverse
            return new[]{_isConst}.Concat(_value.Reverse());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tVariable:");
            sb.AppendLine($"\t\tIsConst: {_isConst}");
            sb.AppendLine($"\t\tValueType: {ValueType}");
            
            // todo output value
            switch (ValueType)
            {
                case ValueType.String:
                    sb.Append("\t\tValue:");
                    foreach (var b in _value)
                    {
                        sb.Append($" {b:X2}");
                    }

                    sb.AppendLine();
                    
                    break;
            }
            
            return sb.ToString();
        }
    }
}