using System.Collections.Generic;
using System.Linq;
using compiler_c0.symbol_manager.value_type;

namespace compiler_c0.symbol_manager.symbol
{
    public class Variable : Symbol
    {
        private byte is_const { get; set; }
        public ValueType ValueType { get; set; }
        private byte[] _value;

        public IEnumerable<byte> ToBytes()
        {
            // todo string不需要reverse
            return new[]{is_const}.Concat(_value.Reverse());
        }
    }
}