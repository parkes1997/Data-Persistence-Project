using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuUI : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void StartGame()
    {
        if (nameInputField == null)
        {
            Debug.LogError("StartMenuUI: Name Input Field is not assigned in the Inspector!");
            return;
        }

        string playerName = nameInputField.text;
        Debug.Log("StartMenuUI: Player name entered: " + playerName);

        if (MainManager.Instance != null)
        {
            MainManager.Instance.PlayerName = playerName;
            Debug.Log("StartMenuUI: Player name set in MainManager.");
        }
        else
        {
            Debug.LogError("StartMenuUI: MainManager.Instance is null!");
        }

        Debug.Log("StartMenuUI: Attempting to load scene 'main'.");
        SceneManager.LoadScene("main");
    }
}


