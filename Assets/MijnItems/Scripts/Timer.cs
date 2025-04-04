using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;  
    private float timeElapsed = 0f;
    private bool timerRunning = false;
    private void Start()
    {
        timerText.gameObject.SetActive(false);
    }
    void Update()
    {
        if (timerRunning)
        {
            timeElapsed += Time.deltaTime;  
            timerText.text = "Time: " + timeElapsed.ToString("F2");
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
        timerText.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}