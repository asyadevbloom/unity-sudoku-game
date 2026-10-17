using Sudoku.Core;
using UnityEngine;

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
                    int value = _board.GetCell(row, column);
                    cell.SetValue(value);
                }
            }
        }
    }
}