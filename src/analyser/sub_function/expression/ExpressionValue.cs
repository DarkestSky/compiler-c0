using compiler_c0.symbol_manager.symbol.value_type;

namespace compiler_c0.analyser.sub_function.expression
{
    public class ExpressionValue
    {
        public ValueType ValueType { get; }

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