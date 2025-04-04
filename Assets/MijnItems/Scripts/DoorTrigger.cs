using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isFrontDoor = true; 

    private Timer timer;
    private bool timerStarted = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer not found");
        }
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