namespace compiler_c0.tokenizer.token
{
    public enum TokenType
    {
        // Other
        Eof,
        Unknown,
        
        // Keyword
        Fn,
        Let,
        Const,
        As,
        While,
        If,
        Else,
        Return,
        Break,
        Continue,
        
        
        // Identifier
        Identifier,
        
        // Literal
        LiteralNumber,
        LiteralString,
        LiteralDouble,
        LiteralCharacter,
        
        // Operator
        Plus,
        Minus,
        Mul,
        Div,
        Assign,
        Eq,
        Neq,
        Lt,
        Gt,
        Le,
        Ge,
        LParen,
        RParen,
        LBrace,
        RBrace,
        Arrow,
        Comma,
        Colon,
        Semicolon,
    }
}