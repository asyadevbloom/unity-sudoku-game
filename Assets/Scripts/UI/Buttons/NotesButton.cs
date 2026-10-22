using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    public class NotesButton : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private Image _buttonImage;
        private Button button;
        private Color _originalColor;
        private bool _isActive = false;

        private void Awake()
        {
            button = GetComponent<Button>();
            _originalColor = _buttonImage.color;
        }

        public void ToggleNotes()
        {
            _isActive = !_isActive;
            _boardView.ToggleNoteMode();

            _buttonImage.color = _isActive ? button.colors.pressedColor : _originalColor;
        }

        public void ResetNotesMode()
        {
            _isActive = false;
            _buttonImage.color = _originalColor;
        }
    }
}