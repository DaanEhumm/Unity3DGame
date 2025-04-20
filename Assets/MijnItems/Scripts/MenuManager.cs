using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] internal TMP_InputField usernameInput;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    internal void PlayGame()
    {
        PlayerPrefs.SetString("username", usernameInput.text);
        SceneManager.LoadScene("GameScene");
    }
    internal void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene"); 
    }

    internal void OpenRules()
    {
        SceneManager.LoadScene("RulesScene"); 
    }
    internal void OpenMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
