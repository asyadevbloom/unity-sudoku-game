using UnityEngine;

namespace Sudoku
{
    [CreateAssetMenu(fileName = "NewTheme", menuName = "Sudoku/Color Theme")]
    public class ColorTheme : ScriptableObject
    {
        [Header("Board Colors")]
        public Color backgroundColor;
        public Color cellNormalColor;
        public Color cellSelectedColor;
        public Color cellErrorColor;
        public Color cellFixedColor;

        [Header("Text Colors")]
        public Color textGivenColor;
        public Color textPlayerColor;
        public Color textErrorColor;
    }
}
