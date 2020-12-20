using System;
using System.Linq;
using System.Text;
using compiler_c0.char_parser;
using compiler_c0.tokenizer.token;
using compiler_c0.tokenizer.token.extensions;
using Microsoft.VisualBasic.CompilerServices;

namespace compiler_c0.tokenizer
{
    public class Tokenizer
    {
        private Tokenizer()
        {
        }

        private static Tokenizer _instance;

        public static Tokenizer Instance
        {
            get { return _instance ??= new Tokenizer(); }
        }

        private Token _peeked;

        public bool HasNext()
        {
            _peeked ??= _fetchNextToken();

            return _peeked!.TokenType != TokenType.Eof;
        }

        public Token NextToken()
        {
            _peeked ??= _fetchNextToken();

            var res = _peeked;
            _peeked = null;
            return res;
        }

        public Token PeekToken()
        {
            _peeked ??= _fetchNextToken();

            return _peeked;
        }

        public Token ExpectToken(params TokenType[] type)
        {
            var token = NextToken();
            if (type.Contains(token.TokenType))
            {
                return token;
            }

            throw new Exception($"unexpected token: {token}");
        }

        private CharParser _charParser = CharParser.Instance;

        private Token _fetchNextToken()
        {
            _skipSpaceCharacters();

            if (!_charParser.HasNext())
            {
                return new Token(TokenType.Eof);
            }

            var peek = _charParser.PeekChar();
            Token token = null;
            if (char.IsDigit(peek))
            {
                token = _parseNumber();
            }
            else if (peek == '"')
            {
                token = _parseConstantString();
            }
            else if (char.IsLetter(peek))
            {
                token = _parseIdentOrKeyword();
            }
            else
            {
                token = _parseOperatorOrUnknown();
            }


            return token;
        }

        private void _skipSpaceCharacters()
        {
            while (_charParser.HasNext() && char.IsWhiteSpace(_charParser.PeekChar()))
            {
                _charParser.NextChar();
            }
        }

        private Token _parseNumber()
        {
            var pos = _charParser.CurrentPos();
            var sb = new StringBuilder();
            var hasPoint = false;
            while (char.IsDigit(_charParser.PeekChar()) || _charParser.PeekChar() == '.')
            {
                if (_charParser.PeekChar() == '.')
                {
                    if (hasPoint)
                        throw new Exception("duplicate point found");
                    hasPoint = true;
                }
                sb.Append(_charParser.NextChar());
            }

            if (hasPoint)
            {
                var number = double.Parse(sb.ToString());
                return new Token(TokenType.LiteralDouble, pos) {Value = number};
            }
            else
            {
                var number = ulong.Parse(sb.ToString());
                return new Token(TokenType.LiteralNumber, pos) {Value = number};
            }
        }

        private Token _parseConstantString()
        {
            var pos = _charParser.CurrentPos();
            var sb = new StringBuilder();
            var isBackslash = false;
            _charParser.NextChar();
            while (isBackslash || _charParser.PeekChar() != '"')
            {
                if (!_charParser.HasNext())
                    throw new Exception("invalid string");

                var next = _charParser.NextChar();
                if (isBackslash)
                {
                    switch (next)
                    {
                        case '\\':
                            break;
                        case '\"':
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append('\"');
                            break;
                        case '\'':
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append('\'');
                            break;
                        case 'n':
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append('\n');
                            break;
                        case 't':
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append('\t');
                            break;
                        case 'r':
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append('\r');
                            break;
                        default:
                            sb.Append(next);
                            break;
                    }

                    isBackslash = false;
                }
                else
                {
                    sb.Append(next);
                    if (next == '\\')
                        isBackslash = true;
                }
            }

            _charParser.NextChar();
            return new Token(TokenType.LiteralString, pos) {Value = sb.ToString()};
        }

        private Token _parseIdentOrKeyword()
        {
            var pos = _charParser.CurrentPos();
            var sb = new StringBuilder();
            while (char.IsLetterOrDigit(_charParser.PeekChar()) || _charParser.PeekChar() == '_')
            {
                sb.Append(_charParser.NextChar());
            }

            var value = sb.ToString();
            var tokenType = value.ToKeyWord();
            return tokenType != TokenType.Unknown
                ? new Token(tokenType, pos)
                : new Token(TokenType.Identifier, pos) {Value = value};
        }

        private Token _parseOperatorOrUnknown()
        {
            var pos = _charParser.CurrentPos();
            var sb = new StringBuilder();
            sb.Append(_charParser.PeekChar());
            var firstChar = _charParser.NextChar();
            if (firstChar.IsFirstOfOperator())
            {
                sb.Append(_charParser.PeekChar());
                var tokenType = sb.ToString().ToOperator();
                if (tokenType != TokenType.Unknown)
                {
                    _charParser.NextChar();
                    return new Token(tokenType, pos);
                }

                sb.Remove(1, 1);
            }

            return new Token(sb.ToString().ToOperator(), pos);
        }
    }
}