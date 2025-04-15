using System;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] internal float delay = 3f;
    [Header("Grenade")]
    [SerializeField] internal float damageRadius = 20f;
    [SerializeField] internal float èxlosionForce = 1200f;
    [Header("Smoke")]
    [SerializeField] internal float smokeDuration = 8f;
    [SerializeField] internal float smokeRadius = 15f;

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

        GameObject smoke = Instantiate(GlobalReff.Instance.smokeEffect, transform.position, Quaternion.identity);
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        SoundManager.Instance.ThrowablesChannel.PlayOneShot(SoundManager.Instance.SmokeSound);

        if (ps != null)
        {
            ps.Play();
            StartCoroutine(StopSmokeAfterDuration(ps, smokeDuration));
        }
    }

    private System.Collections.IEnumerator StopSmokeAfterDuration(ParticleSystem ps, float duration = 8f)
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
        Destroy(ps.gameObject, 2f);
    }
}
    #endregion
