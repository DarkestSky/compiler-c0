namespace compiler_c0.tokenizer.token.extensions
{
    public static class StringExtensions
    {
        public static TokenType ToKeyWord(this string s)
        {
            return s switch
            {
                "fn"       => TokenType.Fn,
                "let"      => TokenType.Let,
                "const"    => TokenType.Const,
                "as"       => TokenType.As,
                "while"    => TokenType.While,
                "if"       => TokenType.If,
                "else"     => TokenType.Else,
                "return"   => TokenType.Return,
                "break"    => TokenType.Break,
                "continue" => TokenType.Continue,
                _          => TokenType.Unknown
            };
        }

        public static TokenType ToOperator(this string s)
        {
            return s switch
            {
                "+"  => TokenType.Plus,
                "-"  => TokenType.Minus,
                "*"  => TokenType.Mul,
                "/"  => TokenType.Div,
                "="  => TokenType.Assign,
                "==" => TokenType.Eq,
                "!=" => TokenType.Neq,
                "<"  => TokenType.Lt,
                ">"  => TokenType.Gt,
                "<=" => TokenType.Le,
                ">=" => TokenType.Ge,
                "("  => TokenType.LParen,
                ")"  => TokenType.RParen,
                "{"  => TokenType.LBrace,
                "}"  => TokenType.RBrace,
                "->" => TokenType.Arrow,
                ","  => TokenType.Comma,
                ":"  => TokenType.Colon,
                ";"  => TokenType.Semicolon,
                _    => TokenType.Unknown
            };
        } 
    }
}