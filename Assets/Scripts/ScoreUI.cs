using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text bestScoreText;

    void Start()
    {
        string bestName = MainManager.Instance.HighScorePlayer;
        int bestScore = MainManager.Instance.HighScore;
        bestScoreText.text = $"Best Score : {bestName} : {bestScore}";
    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > MainManager.Instance.HighScore)
        {
            MainManager.Instance.HighScore = newScore;
            MainManager.Instance.HighScorePlayer = MainManager.Instance.PlayerName;
            MainManager.Instance.SaveHighScore();
        }
    }
}
