using Sudoku.Core;

namespace Sudoku
{
    /// <summary>
    /// stores global game settings that need to persist between scenes
    /// </summary>
    public static class GameSettings
    {
        public static Difficulty SelectedDifficulty = Difficulty.Easy;
    }
}
