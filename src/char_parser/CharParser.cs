using System;
using System.Collections.Generic;
using System.IO;
using compiler_c0.global_config;

namespace compiler_c0.char_parser
{
    public class CharParser
    {
        private CharParser()
        {
            var sr = new StreamReader(GlobalConfig.Instance.InputFilePath);
            while (!sr.EndOfStream)
            {
                _lineBuffer.Add(sr.ReadLine() + '\n');
            }
        }

        private static CharParser _instance;

        public static CharParser Instance
        {
            get { return _instance ??= new CharParser(); }
        }

        private readonly List<string> _lineBuffer = new();

        private Pos _ptrNext = new(0, 0);
        private Pos _ptr = new(0, 0);
        private char? _peeked;

        public bool HasNext()
        {
            return _ptr.Row < _lineBuffer.Count;
        }

        public char NextChar()
        {
            if (_peeked == null)
            {
                _peeked = _fetchNextChar();
            }

            var ch = _peeked.Value;
            _peeked = null;
            _ptr = _ptrNext;
            return ch;
        }

        public char PeekChar()
        {
            if (_peeked == null)
            {
                _peeked = _fetchNextChar();
            }

            return _peeked.Value;
        }
        
        private char _fetchNextChar()
        {
            if (!HasNext())
            {
                return (char) 0;
            }

            var result = _lineBuffer[_ptrNext.Row][_ptrNext.Col];
            _ptrNext = _nextPos();
            return result;
        }

        private Pos _previousPos()
        {
            return _ptr.Col == 0
                ? new Pos(_ptr.Row - 1, _lineBuffer[_ptr.Row - 1].Length - 1)
                : new Pos(_ptr.Row, _ptr.Col - 1);
        }

        private Pos _nextPos()
        {
            return _ptr.Col == _lineBuffer[_ptr.Row].Length - 1
                ? new Pos(_ptr.Row + 1, 0)
                : new Pos(_ptr.Row, _ptr.Col + 1);
        }

        public Pos CurrentPos()
        {
            return _ptr;
        }
    }
}