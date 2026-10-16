using System;

namespace Sudoku.Core
{
    public class SudokuSolver
    {
        private readonly SudokuBoard _board;
        private int _callCount = 0;
        private const int MaxCalls = 1_000_000;
        private int _solutionCount;
        private int _solutionLimit;

        public SudokuSolver(SudokuBoard board)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board));
        }

        public bool Solve()
        {
            if (!_board.IsConsistent())
            {
                return false;
            }

            _callCount = 0;
            return SolveCell(0, 0);
        }

        private bool SolveCell(int row, int column)
        {
            if (++_callCount > MaxCalls)
            {
                return false;
            }

            if (row == SudokuBoard.Size)
                return true;

            // reached end of current row, move to next row
            if (column == SudokuBoard.Size)
                return SolveCell(row + 1, 0);

            // cell already filled, skip it
            if (!_board.IsEmpty(row, column))
                return SolveCell(row, column + 1);

            // try each number 1-9
            for (int value = 1; value <= SudokuBoard.Size; value++)
            {
                if (!_board.IsValidPlacement(row, column, value))
                    continue;

                // place the number
                _board.SetCell(row, column, value);

                // try to solve the rest of the board
                if (SolveCell(row, column + 1))
                    return true;

                // undo and try next number
                _board.SetCell(row, column, 0);
            }

            return false;
        }

        public int CountSolutions(int limit = 2)
        {
            if (!_board.IsConsistent())
                return 0;

            _solutionCount = 0;
            _solutionLimit = limit;

            CountSolutionsRecursive(0, 0);

            return _solutionCount;
        }

        private void CountSolutionsRecursive(int row, int column)
        {
            // return if we've hit the limit
            if (_solutionCount >= _solutionLimit)
                return;

            // reached past the last row, found a solution
            if (row == SudokuBoard.Size)
            {
                _solutionCount++;
                return;
            }

            // end of current row — move to next
            if (column == SudokuBoard.Size)
            {
                CountSolutionsRecursive(row + 1, 0);
                return;
            }

            // cell already filled — skip
            if (!_board.IsEmpty(row, column))
            {
                CountSolutionsRecursive(row, column + 1);
                return;
            }

            // try each number 1-9
            for (int value = 1; value <= SudokuBoard.Size; value++)
            {
                if (!_board.IsValidPlacement(row, column, value))
                    continue;

                _board.SetCell(row, column, value);

                CountSolutionsRecursive(row, column + 1);

                // undo
                _board.SetCell(row, column, 0);

                // stop early if we've hit the limit
                if (_solutionCount >= _solutionLimit)
                    return;
            }
        }
    }
}
