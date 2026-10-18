using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sudoku.UI
{
    public class MenuButton : MonoBehaviour
    {
        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
