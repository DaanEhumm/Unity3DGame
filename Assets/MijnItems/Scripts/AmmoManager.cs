using TMPro;
using UnityEngine;
using System.Collections;
public class AmmoManager : MonoBehaviour{
    public static AmmoManager Instance { get; set; }
    public TextMeshProUGUI AmmoCount;

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
