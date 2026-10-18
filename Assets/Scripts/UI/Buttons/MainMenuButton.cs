using Sudoku.Core;
using UnityEngine;
using UnityEngine.SceneManagement; // <-- Обязательно для загрузки сцен!

namespace Sudoku.UI
{
    public class MainMenuButton : MonoBehaviour
    {
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private string _sceneName = "Sudoku";

        public void StartGame()
        {
            // save selected difficulty
            GameSettings.SelectedDifficulty = _difficulty;

           // load scene with game
            SceneManager.LoadScene(_sceneName);
        }
    }
}
