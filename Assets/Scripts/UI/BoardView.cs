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
        [SerializeField] private Difficulty _difficulty = Difficulty.Easy;

        private SudokuGenerator _generator;
        private SudokuBoard _board;
        private CellView _selectedCell;
        private CellView[,] _cellViews = new CellView[SudokuBoard.Size, SudokuBoard.Size];

        private void Start()
        {
            _generator = new SudokuGenerator();
            _board = _generator.Generate(_difficulty);

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

                bool isError = HasConflict(_selectedCell.Row, _selectedCell.Column, value);
                _selectedCell.SetError(isError);

                if (CheckWin())
                {
                    Debug.Log("Victory!");
                }
            }

            
        }

        /// <summary>
        /// checks if the placed value violates any Sudoku rules
        /// </summary>
        private bool HasConflict(int row, int column, int value)
        {
            // if value is 0 (we erased it), it's never a conflict
            if (value == 0) return false;

            for (int i = 0; i < SudokuBoard.Size; i++)
            {
                // check row and column (skip the cell we just changed)
                if (i != column && _board.GetCell(row, i) == value)
                {
                    return true;
                }
                if (i != row && _board.GetCell(i, column) == value)
                {
                    return true;
                }
            }

            // check 3x3 box
            int boxRow = row - row % SudokuBoard.BoxSize;
            int boxCol = column - column % SudokuBoard.BoxSize;

            for (int r = boxRow; r < boxRow + SudokuBoard.BoxSize; r++)
            {
                for (int c = boxCol; c < boxCol + SudokuBoard.BoxSize; c++)
                {
                    // skip the cell we just changed
                    if (r == row && c == column) continue;

                    if (_board.GetCell(r, c) == value) return true;
                }
            }

            return false;
        }

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
    }
}