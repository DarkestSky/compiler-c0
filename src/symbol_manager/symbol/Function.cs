using System;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class Function : Symbol
    {
        public uint returnSlot { get; set; }
        private ValueType ReturnType { get; set; }

        public void SetReturnType(ValueType valueType)
        {
            ReturnType = valueType;
            switch (valueType)
            {
                case ValueType.Void:
                    returnSlot = 0;
                    break;
                case ValueType.Int:
                case ValueType.Double:
                case ValueType.String:
                    returnSlot = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }
    }
}