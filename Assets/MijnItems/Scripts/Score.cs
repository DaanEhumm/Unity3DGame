using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Score : MonoBehaviour
{
    public static Score Instance { get; internal set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private bool scoreActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        scoreText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (scoreActive)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
    #region ================= Score =================
    internal void StartScore()
    {
        scoreActive = true;
        scoreText.gameObject.SetActive(true);
    }

    internal void StopScore()
    {
        scoreActive = false;
    }

    internal void AddScore(int points)
    {
        if (scoreActive)
        {
            score += points;
        }
    }

    internal int GetScore()
    {
        return score;
    }
    #endregion
}