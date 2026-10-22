using UnityEngine;

namespace Sudoku.UI
{
    public class ExitButton : MonoBehaviour
    {
        public void QuitGame()
        {
            // quit the application (only works in a built game)
            Application.Quit();

            // if we are running inside the Unity Editor, stop playing
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
