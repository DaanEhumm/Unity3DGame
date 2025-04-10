using System;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float èxlosionForce = 1200f;

    float countDown;
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        none,
        Grenade
    }

    public ThrowableType throwableType;

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
}
