using System.Collections.Generic;

namespace compiler_c0.tokenizer.token.token_extension
{
    public static class CharExtensions
    {
        private static HashSet<char> _firstSet = new HashSet<char>() {'=', '!', '<', '>', '-'};
        
        public static bool IsFirstOfOperator(this char c)
        {
            return _firstSet.Contains(c);
        }
    }
}