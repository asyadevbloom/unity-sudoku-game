using Sudoku.UI;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    [SerializeField] private int _value;
    [SerializeField] private BoardView _boardView;

    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _boardView.TrySetValue(_value);
    }
}
