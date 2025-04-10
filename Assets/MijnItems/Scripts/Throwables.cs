using System;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] internal float delay = 3f;
    [SerializeField] internal float damageRadius = 20f;
    [SerializeField] internal float èxlosionForce = 1200f;

    internal float countDown;
    internal bool hasExploded = false;
    internal bool hasBeenThrown = false;


    internal enum ThrowableType
    {
        none,
        Grenade
        // hier komt nog 1 lethal en 1/2 tacticals throwables
    }

    [SerializeField] internal ThrowableType throwableType;

    private void Start()
    {
        countDown = delay;
    }
    private void Update()
    {
        if (hasBeenThrown)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        getThrowableEffect();

        Destroy(gameObject);
    }

    private void getThrowableEffect()
    {
        Debug.Log(throwableType);
        
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }
    #region ================= Lethal Effects =================
    private void GrenadeEffect()
    {
        // visual
        Debug.Log("Grenade Exploded");
        GameObject explosionEffect = GlobalReff.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        SoundManager.Instance.ThrowablesChannel.PlayOneShot(SoundManager.Instance.GrenadeSound);

        // physical
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(èxlosionForce, transform.position, damageRadius);
            }
        }
    }
    #endregion

    #region ================= Tactical Effects =================
    // Hier komen de effecten van de tactische throwables
    #endregion
}
