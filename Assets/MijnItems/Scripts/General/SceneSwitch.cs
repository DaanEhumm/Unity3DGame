using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MenuScene");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
