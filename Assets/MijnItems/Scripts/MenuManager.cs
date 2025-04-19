using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    internal void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene"); 
    }

    public void OpenRules()
    {
        SceneManager.LoadScene("RulesScene"); 
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
