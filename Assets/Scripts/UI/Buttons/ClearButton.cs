using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    public class ClearButton : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            _boardView.ClearSelectedCell();
        }
    }
}
