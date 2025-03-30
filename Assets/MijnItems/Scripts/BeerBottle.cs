using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BeerBottle : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }
    }

}