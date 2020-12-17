using System;
using compiler_c0.tokenizer.token;

namespace compiler_c0.symbol_manager.value_type
{
    public static class entension
    {
        public static ValueType ToValueType(this Token t)
        {
            return (string) t.Value switch
            {
                "int"    => ValueType.Int,
                "double" => ValueType.Double,
                "void"   => ValueType.Void,
                _ => throw new Exception("undefined value type")
            };
        }
    }
}