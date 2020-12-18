using compiler_c0.symbol_manager.value_type;
using compiler_c0.tokenizer;

namespace compiler_c0.analyser.sub_function.expression
{
    public class ExpressionValue
    {
        public ValueType ValueType { get; set; }

        public ExpressionValue(ValueType t)
        {
            ValueType = t;
        }
        
        public bool Is(ValueType t)
        {
            return ValueType == t;
        }
    }
}