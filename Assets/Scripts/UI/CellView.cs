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
        [SerializeField] private TextMeshProUGUI[] _noteTexts;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        [SerializeField] private Color _playerColor = Color.blue;
        [SerializeField] private Color _givenColor = Color.black;
        private ColorTheme _theme;

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
            _valueText.text = value == 0 ? "" : value.ToString();
            _valueText.color = _theme.textPlayerColor;
        }

        /// <summary>
        /// highlights the cell when selected
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            _background.color = isSelected ? _theme.cellSelectedColor : (_button.interactable ? _theme.cellNormalColor : _theme.cellFixedColor);
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
            _valueText.color = _theme.textGivenColor;
            _background.color = _theme.cellNormalColor;
        }

        public void UnlockCell()
        {
            _button.interactable = true;
            _valueText.color = _theme.textPlayerColor;
            //_background.color = _theme.cellNormalColor;
        }

        public void SetError(bool hasError)
        {
            //_background.color = hasError ? _theme.cellErrorColor : (_button.interactable ? _theme.cellNormalColor : _theme.cellFixedColor);
            _valueText.color = hasError ? _theme.textErrorColor : _theme.textPlayerColor;
        }

        public void SetNote(int number, bool isActive)
        {
            int index = number - 1;

            if (index >= 0 && index < _noteTexts.Length)
            {
                _noteTexts[index].text = isActive ? number.ToString() : "";
            }
        }

        public void ClearAllNotes()
        {
            foreach (var noteText in _noteTexts)
            {
                noteText.text = "";
            }
        }

        public void SetTheme(ColorTheme theme)
        {
            _theme = theme;
        }

        public void SetGivenColor()
        {
            _valueText.color = _theme.textGivenColor;
        }

        public void SetPlayerColor()
        {
            _valueText.color = _theme.textPlayerColor;
        }
    }
}
