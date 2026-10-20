using Sudoku.Core;
using UnityEngine;
using System.Collections.Generic;

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
        [SerializeField] private ColorTheme _theme;

        private SudokuGenerator _generator;
        private SudokuBoard _board;
        private CellView _selectedCell;
        private CellView[,] _cellViews = new CellView[SudokuBoard.Size, SudokuBoard.Size];
        private int[,] _initialPuzzle;
        private SudokuBoard _solution;
        private Stack<(int row, int column, int previousValue)> _history = new();
        private bool _isNoteMode = false;
        private bool[,,] _notes = new bool[SudokuBoard.Size, SudokuBoard.Size, 9];

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
                    cell.SetTheme(_theme);
                    _cellViews[row, column] = cell;

                    cell.Init(row, column);

                    int value = _board.GetCell(row, column);

                    cell.SetValue(value);

                    // if the cell already has a number, lock it
                    if (value != 0)
                    {
                        cell.LockCell();
                    }

                    cell.SetSelected(false);

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

        public void ToggleNoteMode()
        {
            _isNoteMode = !_isNoteMode;
        }

        public void TrySetValue(int value)
        {
            if (_selectedCell != null)
            {
                if (_isNoteMode)
                {
                    int row = _selectedCell.Row;
                    int column = _selectedCell.Column;

                    if (_board.GetCell(row, column) != 0) return;

                    _notes[row, column, value -1] = !_notes[row, column, value - 1];

                    _selectedCell.SetNote(value, _notes[row, column, value - 1]);

                    return;
                }

                _history.Push((_selectedCell.Row, _selectedCell.Column, _board.GetCell(_selectedCell.Row, _selectedCell.Column)));
                
                // assign a value to a cell on the board
                _board.SetCell(_selectedCell.Row, _selectedCell.Column, value);
                //set the number to display
                _selectedCell.SetValue(value);

                _selectedCell.ClearAllNotes();
                for (int i = 0; i < 9; i++)
                {
                    _notes[_selectedCell.Row, _selectedCell.Column, i] = false;
                }

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
            if (GetEmptyCells().Count != 0) return false;
            return _board.IsConsistent();
        }

        public void ClearSelectedCell()
        {
            if (_selectedCell != null)
            {
                _history.Push((_selectedCell.Row, _selectedCell.Column, _board.GetCell(_selectedCell.Row, _selectedCell.Column)));

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
                    _cellViews[row, column].SetSelected(false);
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

        public void UseHint()
        {
            if (CheckWin())
            {
                _timer.StopTimer();
                _winPanel.SetActive(true);
                return;
            }

            if (_selectedCell != null)
            {
                int correctValue = _solution.GetCell(_selectedCell.Row, _selectedCell.Column);
                _board.SetCell(_selectedCell.Row, _selectedCell.Column, correctValue);
                _selectedCell.SetValue(correctValue);
                _selectedCell.LockCell();
                _selectedCell.SetError(false);
                _selectedCell = null;
            }
            else
            {
                var emptyCells = GetEmptyCells();

                if (emptyCells.Count == 0) return;

                int randomIndex = Random.Range(0, emptyCells.Count);
                var randomCell = emptyCells[randomIndex];

                int correctValue = _solution.GetCell(randomCell.row, randomCell.column);

                _board.SetCell(randomCell.row, randomCell.column, correctValue);

                _cellViews[randomCell.row, randomCell.column].SetValue(correctValue);
                _cellViews[randomCell.row, randomCell.column].LockCell();
            }
        }

        private List<(int row, int column)> GetEmptyCells()
        {
            var emptyCells = new List<(int row, int column)>();

            for (int row = 0; row < SudokuBoard.Size; row++)
            {
                for (int column = 0; column < SudokuBoard.Size; column++)
                {
                    if (_board.GetCell(row, column) == 0)
                    {
                        emptyCells.Add((row, column));
                    }
                }
            }

            return emptyCells;
        }

        public void Undo()
        {
            if (_history.Count == 0) return;

            var lastMove = _history.Pop();
            _board.SetCell(lastMove.row, lastMove.column, lastMove.previousValue);
            _cellViews[lastMove.row, lastMove.column].SetValue(lastMove.previousValue);
            if (lastMove.previousValue == 0)
            {
                _cellViews[lastMove.row, lastMove.column].UnlockCell();
            }
            _cellViews[lastMove.row, lastMove.column].SetError(false);
        }
    }
}