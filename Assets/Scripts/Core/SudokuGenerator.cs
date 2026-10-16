using System;
using System.Collections.Generic;

namespace Sudoku.Core
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
    }

    /// <summary>
    /// generates Sudoku puzzles by filling a complete board and
    /// then digging holes while ensuring the solution stays unique
    /// </summary>
    public class SudokuGenerator
    {
        // difficulty tuning: number of empty cells to try to reach
        private const int EasyTargetEmpty = 40;
        private const int MediumTargetEmpty = 48;
        private const int HardTargetEmpty = 54;

        private readonly System.Random _random = new System.Random();
        
        /// <summary>
        /// generates a new puzzle with a unique solution
        /// </summary>
        public SudokuBoard Generate(Difficulty difficulty)
        {
            var board = new SudokuBoard();
            FillCompletely(board);
            int targetEmpty = GetTargetEmptyCount(difficulty);
            DigHoles(board, targetEmpty);

            //safety check
            if (!board.IsConsistent())
            {
                throw new InvalidOperationException("Generated board is inconsistent");
            } 

            return board;
        }

        /// <summary>
        /// fills an empty board with a valid, randomly shuffled solution,
        /// uses backtracking with randomized number order
        /// </summary>
        private void FillCompletely(SudokuBoard board)
        {
            FillCell(board, 0, 0);
        }

        private bool FillCell(SudokuBoard board, int row, int column)
        {
            if (row == SudokuBoard.Size) 
                return true;

            if (column == SudokuBoard.Size)
                return FillCell(board, row + 1, 0);

            var values = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            Shuffle(values);

            foreach(int value in values)
            {
                if (!board.IsValidPlacement(row, column, value))
                    continue;

                board.SetCell(row, column, value);

                if (FillCell(board, row, column + 1))
                    return true;

                board.SetCell(row, column, 0);
            }

            return false;
        }

        /// <summary>
        /// removes cells from the board to create a puzzle, ensuring the
        /// solution remains unique after each removal
        /// </summary>
        private void DigHoles(SudokuBoard board, int targetEmpty)
        {
            var cells = new List<(int row, int column)>();
            for (int row = 0; row < SudokuBoard.Size; row++)
                for (int column = 0; column < SudokuBoard.Size; column++)
                    cells.Add((row, column));

            Shuffle(cells);

            int emptyCount = 0;

            foreach (var (row, column) in cells)
            {
                if (emptyCount >= targetEmpty)
                    break;

                int savedValue = board.GetCell(row, column);

                board.SetCell(row, column, 0);

                var solver = new SudokuSolver(board);
                if (solver.CountSolutions(2) != 1)
                {
                    board.SetCell(row, column, savedValue);
                }
                else
                {
                    emptyCount++;
                }
            }
        }

        /// <summary>
        /// returns the target number of empty cells for the given difficulty
        /// </summary>
        private int GetTargetEmptyCount(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:   return EasyTargetEmpty;
                case Difficulty.Medium: return MediumTargetEmpty;
                case Difficulty.Hard:   return HardTargetEmpty;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(difficulty), $"Unknown difficulty: {difficulty}");
            }
        }

        /// <summary>
        /// fisher-Yates shuffle that shuffles a list in place
        /// </summary>
        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
