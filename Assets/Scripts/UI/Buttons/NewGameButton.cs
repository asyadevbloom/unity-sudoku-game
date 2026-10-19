using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sudoku.UI
{
    public class NewGameButton : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
