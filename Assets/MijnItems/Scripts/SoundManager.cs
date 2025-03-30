using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; set; }
    public AudioSource shootingSoundGlock;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}