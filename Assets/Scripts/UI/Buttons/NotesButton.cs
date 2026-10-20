using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    public class NotesButton : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private Image _buttonImage;
        private bool _isActive = false;
        
        public void ToggleNotes()
        {
            _isActive = !_isActive;
            _boardView.ToggleNoteMode();

            _buttonImage.color = _isActive ? Color.yellow : Color.white;
        }
    }
}