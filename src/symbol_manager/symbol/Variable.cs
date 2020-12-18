using System.Collections.Generic;
using System.Linq;
using compiler_c0.symbol_manager.value_type;

namespace compiler_c0.symbol_manager.symbol
{
    public class Variable : Symbol
    {
        private byte is_const { get; set; }
        public ValueType ValueType { get; set; }


        public IEnumerable<byte> ToBytes()
        {
            // todo
            return Enumerable.Empty<byte>();
        }
    }
}