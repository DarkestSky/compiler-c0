using System;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class LibFunction: Function
    {
        public LibFunction(ValueType returnType, params ValueType[] paramType)
        {
            switch (ReturnType = returnType)
            {
                case ValueType.Void:
                    ReturnSlot = 0;
                    break;
                case ValueType.Int:
                case ValueType.Float:
                    ReturnSlot = 1;
                    break;
                default:
                    throw new Exception("not allowed return type");
            }
            
            Params.AddRange(paramType);

            if (ReturnType != ValueType.Void)
            {
                Params.Insert(0, ReturnType);
            }
        }
    }
}