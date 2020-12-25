using System;
using compiler_c0.tokenizer.token;
using ValueType = compiler_c0.symbol_manager.symbol.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol.value_type
{
    public static class Extension
    {
        public static ValueType ToValueType(this Token t)
        {
            return (string) t.Value switch
            {
                "int"    => ValueType.Int,
                "double" => ValueType.Float,
                "void"   => ValueType.Void,
                _ => throw new Exception("undefined value type")
            };
        }
    }
}