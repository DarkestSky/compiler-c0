using System;

namespace compiler_c0.symbol_manager.symbol
{
    public class Param : Symbol
    {
        public bool IsConst { get; set; }
        
        public ValueType ValueType { get; set; }
    }
}