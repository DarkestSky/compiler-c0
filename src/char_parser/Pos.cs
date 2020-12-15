using System;

namespace compiler_c0.char_parser
{
    public class Pos
    {
        public int Row { get; }
        public int Col { get; }

        public Pos(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString()
        {
            return $"(row: {Row}, col: {Col})";
        }
    }
}