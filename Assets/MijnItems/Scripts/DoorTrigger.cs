using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] internal bool isFrontDoor = true; // deze uitzetten bij de "achterdeur"/exit zone

    private Timer timer;
    private Score score;
    private bool timerStarted = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        score = FindFirstObjectByType<Score>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target[] targets = FindObjectsByType<Target>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            if (isFrontDoor)
            {
                if (!timerStarted)
                {
                    score.StartScore();
                    timer.StartTimer();
                    timerStarted = true;
                }
                foreach (Target target in targets)
                {
                    target.RaiseTarget();
                }
            }
            else
            {
                score.StopScore();
                timer.StopTimer();

                foreach (Target target in targets)
                {
                    target.FallTarget();
                }

                timerStarted = false;
            }
        }
    }
}