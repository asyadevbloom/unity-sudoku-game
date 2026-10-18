using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    /// <summary>
    /// displays a single cell on the Sudoku board
    /// </summary>
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        [SerializeField] private Color _playerColor = Color.blue;
        [SerializeField] private Color _givenColor = Color.black;

        private int _row;
        private int _column;

        public int Row
        {
            get { return _row; }
        }

        public int Column
        {
            get { return _column; }
        }

        public event Action<CellView> OnCellClicked;

        private void Awake()
        {
            _button.onClick.AddListener(HandleClick);
        }
        
        /// <summary>
        /// sets the coordinates of this cell on the board.
        /// </summary>
        public void Init(int row, int column)
        {
            _row = row;
            _column = column;
        }

        /// <summary>
        /// sets the number to display
        /// </summary>
        public void SetValue(int value)
        {
            if (value == 0)
            {
                _valueText.text = "";
            }
            else
            {
                _valueText.text = value.ToString();
            }

            _valueText.color = _playerColor;
        }

        /// <summary>
        /// highlights the cell when selected
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            _background.color = isSelected ? Color.cyan : Color.white;
        }

        private void HandleClick()
        {
            OnCellClicked?.Invoke(this);
        }

        /// <summary>
        /// lock the cell so the player cannot click it or change it
        /// </summary> 
        public void LockCell()
        {
            _button.interactable = false;
            _valueText.color = _givenColor;
        }

        public void UnlockCell()
        {
            _button.interactable = true;
            _valueText.color = _playerColor; //return blue color for the player
        }

        public void SetError (bool hasError)
        {
            if (hasError == true)
            {
                _background.color = Color.red;
            }
            else
            {
                _background.color = Color.white;
            }
        }
    }
}
