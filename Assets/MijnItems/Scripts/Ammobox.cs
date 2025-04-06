using UnityEngine;
using System.Collections.Generic;
using System;

public class Ammobox : MonoBehaviour
{
    [SerializeField] internal int ammoAmount = 120;
    public AmmoType ammoType;

    public enum AmmoType
    {
        Pistol,
        Rifle,
    }
}
