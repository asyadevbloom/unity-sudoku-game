using Sudoku.Core;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Sudoku.UI
{
    /// <summary>
    /// displays a Sudoku board on the UI by spawning cell views
    /// and filling them with numbers
    /// </summary>
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameTimer _timer;

        private SudokuGenerator _generator;
        private SudokuBoard _board;
        private CellView _selectedCell;
        private CellView[,] _cellViews = new CellView[SudokuBoard.Size, SudokuBoard.Size];
        private int[,] _initialPuzzle;
        private SudokuBoard _solution;

        private void Start()
        {
            _generator = new SudokuGenerator();
            
            // generate a new puzzle with empty sells
            _board = _generator.Generate(GameSettings.SelectedDifficulty);
            
            // save a pristine copy of the initial puzzle (used for restarting level)
            _initialPuzzle = _board.Clone();

            // create a separate board and solve it to get the correct answers
            // (used for strict error checking when the player enters a number
            _solution = new SudokuBoard();
            _solution.LoadFrom(_initialPuzzle);
            var solver = new SudokuSolver(_solution);
            solver.Solve();

            // spawn the visual cells on the screen
            SpawnCells();
        }

        private void SpawnCells()
        {
            for (int row = 0; row < SudokuBoard.Size; row++)
            {
                for (int column = 0; column < SudokuBoard.Size; column++)
                {
                    CellView cell = Instantiate(_cellPrefab, transform);
                    _cellViews[row, column] = cell;

                    cell.Init(row, column);

                    int value = _board.GetCell(row, column);

                    cell.SetValue(value);

                    // if the cell already has a number, lock it
                    if (value != 0)
                    {
                        cell.LockCell();
                    }

                    cell.OnCellClicked += HandleCellClicked;
                }
            }
        }

        private void HandleCellClicked(CellView clickedCell)
        {
            if (_selectedCell != null)
            {
                _selectedCell.SetSelected(false);
            }

            _selectedCell = clickedCell;
            _selectedCell.SetSelected(true);
        }

        public void TrySetValue(int value)
        {
            if (_selectedCell != null)
            {
                // assign a value to a cell on the board
                _board.SetCell(_selectedCell.Row, _selectedCell.Column, value);
                //set the number to display
                _selectedCell.SetValue(value);

                //bool isError = HasConflict(_selectedCell.Row, _selectedCell.Column, value);
                //_selectedCell.SetError(isError);

                int correctValue = _solution.GetCell(_selectedCell.Row, _selectedCell.Column);
                bool isError = (value != 0 && value != correctValue);
                _selectedCell.SetError(isError);

                if (CheckWin())
                {
                    _timer.StopTimer();
                    _winPanel.SetActive(true);
                }
            }

            
        }

        /// <summary>
        /// checks if the placed value violates any Sudoku rules
        /// </summary>
        //private bool HasConflict(int row, int column, int value)
        //{
        //    // if value is 0 (we erased it), it's never a conflict
        //    if (value == 0) return false;

        //    for (int i = 0; i < SudokuBoard.Size; i++)
        //    {
        //        // check row and column (skip the cell we just changed)
        //        if (i != column && _board.GetCell(row, i) == value)
        //        {
        //            return true;
        //        }
        //        if (i != row && _board.GetCell(i, column) == value)
        //        {
        //            return true;
        //        }
        //    }

        //    // check 3x3 box
        //    int boxRow = row - row % SudokuBoard.BoxSize;
        //    int boxCol = column - column % SudokuBoard.BoxSize;

        //    for (int r = boxRow; r < boxRow + SudokuBoard.BoxSize; r++)
        //    {
        //        for (int c = boxCol; c < boxCol + SudokuBoard.BoxSize; c++)
        //        {
        //            // skip the cell we just changed
        //            if (r == row && c == column) continue;

        //            if (_board.GetCell(r, c) == value) return true;
        //        }
        //    }

        //    return false;
        //}

        private bool CheckWin()
        {
            for (int row = 0; row < SudokuBoard.Size; row++)
            {
                for (int column = 0; column < SudokuBoard.Size; column++)
                {
                    if (_board.GetCell(row, column) == 0) return false;
                }
            }

            return _board.IsConsistent();
        }

        public void ClearSelectedCell()
        {
            if (_selectedCell != null)
            {
                _board.SetCell(_selectedCell.Row, _selectedCell.Column, 0);
                _selectedCell.SetValue(0);
                _selectedCell.SetError(false);
            }
        }

        public void RestartBoard()
        {
            _timer.ResetTimer();
            _board.LoadFrom(_initialPuzzle);

            for (int row = 0; row < SudokuBoard.Size; row++)
            {
                for (int column = 0; column < SudokuBoard.Size; column++)
                {
                    int value = _board.GetCell(row, column);
                    _cellViews[row, column].SetValue(value);

                    if (value != 0)
                    {
                        _cellViews[row, column].LockCell();
                    }
                    else
                    {
                        _cellViews[row, column].UnlockCell();
                    }

                    _cellViews[row, column].SetError(false);
                }
            }
        }

        /// <summary>
        /// fills the board with correct answers, leaving one cell empty
        /// (used for testing the win condition)
        /// </summary>
        public void DebugFillBoard()
        {
            bool emptyCellLeft = false;

            for (int row = 0; row < SudokuBoard.Size; row++)
            {
                for (int column = 0; column < SudokuBoard.Size; column++)
                {
                    // only fill cells that were originally empty (can be changed)
                    if (_initialPuzzle[row, column] == 0)
                    {
                        if (!emptyCellLeft)
                        {
                            // leave this one empty for the player
                            _board.SetCell(row, column, 0);
                            _cellViews[row, column].SetValue(0);
                            _cellViews[row, column].UnlockCell();
                            emptyCellLeft = true;
                        }
                        else
                        {
                            // fill with the correct answer
                            int correctValue = _solution.GetCell(row, column);
                            _board.SetCell(row, column, correctValue);
                            _cellViews[row, column].SetValue(correctValue);
                            _cellViews[row, column].LockCell();
                        }
                    }
                }
            }
        }
    }
}