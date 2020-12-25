using System;
using ValueType = compiler_c0.symbol_manager.symbol.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class Param : Symbol
    {
        private bool IsConst { get; set; }
        
        public ValueType ValueType { get; private set; }

        public void TryAssign()
        {
            if (IsConst)
                throw new Exception("assign is invalid with const param");
        }

        public Param(ValueType type, bool isConst)
        {
            ValueType = type;
            IsConst = isConst;
        }
    }
}