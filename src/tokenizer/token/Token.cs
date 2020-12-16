using System.Linq;
using compiler_c0.char_parser;

namespace compiler_c0.tokenizer.token
{
    public class Token
    {
        public Token(TokenType type, Pos pos = null)
        {
            TokenType = type;
            Pos = pos ?? new Pos(-1, -1);
        }

        public TokenType TokenType { get; }
        private Pos Pos { get; }
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{TokenType} at {Pos}";
        }

        public bool Is(params TokenType[] type)
        {
            return type.Contains(TokenType);
        }

        public bool IsLiteral()
        {
            return Is(TokenType.LiteralCharacter, TokenType.LiteralDouble,
                TokenType.LiteralNumber, TokenType.LiteralString);
        }
    }
}