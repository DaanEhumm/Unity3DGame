using System.Collections;
using UnityEngine;
public class GlobalReff : MonoBehaviour
{
    public static GlobalReff Instance { get; set; }
    [SerializeField] internal GameObject BulletImpactEffectPrefab;
    [SerializeField] internal GameObject grenadeExplosionEffect;

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
}