using UnityEngine;

public class GlobalReff : MonoBehaviour
{
    public static GlobalReff instance { get; set; }
    public GameObject BulletImpactEffectPrefab;
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
