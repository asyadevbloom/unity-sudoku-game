using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    float _timeElapsed = 0f;
    bool _isRunning = true;

    private void Update()
    {
        if (_isRunning == true)
        {
            _timeElapsed += Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timeElapsed / 60);
            int seconds = Mathf.FloorToInt(_timeElapsed % 60);
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _timeElapsed = 0f;
        _isRunning = true;
    }
}
