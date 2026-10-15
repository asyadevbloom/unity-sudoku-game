using System;

namespace Sudoku.Core
{
    public class SudokuBoard
    {
        public const int Size = 9;
        public const int BoxSize = 3;

        private readonly int[,] _cells = new int[Size, Size];

        public int GetCell(int row, int column)
        {
            return _cells[row, column];
        }

        public void SetCell(int row, int column, int value)
        {
            _cells[row, column] = value;
        }

        public bool IsValidPlacement(int row, int column, int value)
        {
            // check if number is valid
            if (value < 1 || value > 9) return false;

            // check row and column
            for (int i = 0; i < Size; i++)
            {
                if (_cells[row, i] == value) return false;
                if (_cells[i, column] == value) return false;
            }

            // check 3x3 box
            int boxRow = row - row % BoxSize;
            int boxCol = column - column % BoxSize;
            for (int r = boxRow; r < boxRow + BoxSize; r++)
                for (int c = boxCol; c < boxCol + BoxSize; c++)
                    if (_cells[r, c] == value) return false;

            return true;
        }

        public bool IsEmpty(int row, int column)
        {
            return _cells[row, column] == 0;
        }

        public bool IsConsistent()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    int value = _cells[row, column];
                    if (value == 0) continue;

                    _cells[row, column] = 0;
                    bool ok = IsValidPlacement(row, column, value);
                    _cells[row, column] = value;

                    if (!ok) return false;
                }
            }
            return true;
        }

        public void Clear()
        {
            Array.Clear(_cells, 0, _cells.Length);
        }
            

        public int[,] Clone()
        {
            return (int[,])_cells.Clone();
        }

        public void LoadFrom(int[,] source)
        {
            if (source.GetLength(0) != Size || source.GetLength(1) != Size)
                throw new ArgumentException($"Source array must be {Size}x{Size}");

            for (int row = 0; row < Size; row++)
                for (int column = 0; column < Size; column++)
                    _cells[row, column] = source[row, column];
        }
    }
}


