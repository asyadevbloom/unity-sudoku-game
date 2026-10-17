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
        }
    }
}
