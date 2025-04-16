using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] internal float delay = 3f;
    [Header("Grenade")]
    [SerializeField] internal float damageRadius = 20f;
    [SerializeField] internal float èxlosionForce = 1200f;
    [Header("Smoke")]
    [SerializeField] internal float smokeDuration = 6f;

    internal float countDown;
    internal bool hasExploded = false;
    internal bool smokeisActive = false;
    internal bool hasBeenThrown = false;


    internal enum ThrowableType
    {
        none,
        Grenade,
        Smoke
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
                Detonate();
                hasExploded = true;
            }
        }
    }

    private void Detonate()
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
            case ThrowableType.Smoke:
                SmokeEffect();
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
    private void SmokeEffect()
    {
        Debug.Log("Smoke Exploded");

        GameObject smokeFX = Instantiate(GlobalReff.Instance.smokeEffect, transform.position, Quaternion.identity);

        ParticleSystem ps = smokeFX.GetComponent<ParticleSystem>();
        if (ps == null)
        {
            ps = smokeFX.GetComponentInChildren<ParticleSystem>();
        }

        // Speel geluid af
        SoundManager.Instance.ThrowablesChannel.PlayOneShot(SoundManager.Instance.SmokeSound);

        if (ps != null)
        {
            ps.Play();
            StartCoroutine(StopSmokeAfterDuration(ps, smokeFX));
        }
        else
        {
            Debug.LogWarning(" Geen ParticleSystem gevonden effect prefab!");
        }
    }

    private IEnumerator StopSmokeAfterDuration(ParticleSystem ps, GameObject smokeObject)
    {
        yield return new WaitForSeconds(smokeDuration);

        ps.Stop();

        Destroy(smokeObject, 2f);
    }
}
    #endregion
