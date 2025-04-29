using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    public string PlayerName;
    public string HighScorePlayer;
    public int HighScore;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "main")
        {
            SetupLevel();
        }
    }

    void SetupLevel()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Brick brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        UpdateScoreText();
        UpdateBestScoreText();
    }

    private void Update()
    {
        if (!m_Started && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
        else if (m_GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    void LaunchBall()
    {
        if (Ball == null)
        {
            Debug.LogError("Ball reference not set in the Inspector!");
            return;
        }

        m_Started = true;
        Vector3 forceDir = new Vector3(Random.Range(-1.0f, 1.0f), 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        UpdateScoreText();

        if (m_Points > HighScore)
        {
            HighScore = m_Points;
            HighScorePlayer = PlayerName;
            SaveHighScore();
            UpdateBestScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score : {m_Points}";
        }
    }

    void UpdateBestScoreText()
    {
        if (BestScoreText != null)
        {
            BestScoreText.text = $"Best Score : {HighScorePlayer} : {HighScore}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (GameOverText != null)
        {
            GameOverText.SetActive(true);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int score;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.playerName = HighScorePlayer;
        data.score = HighScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            HighScorePlayer = data.playerName;
            HighScore = data.score;
        }
    }
}


