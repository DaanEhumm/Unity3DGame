using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    internal bool IsActiveWeapon;
    [Header ("Shooting")]
    [SerializeField] private bool isShooting, readyToShoot;
    [SerializeField] private float shootingDelay = 2f;
    private bool allowReset = true;

    [Header(" Only For Burst")] // ik heb nog geen burst wapen, ik zou evt de bestaande AR kunnen aanpassen naar een burst wapen
    [SerializeField] internal int bulletsPerBurts = 3;
    [SerializeField] private int BurstBulletsLeft;

    [Header ("spread")]
    [SerializeField] private float HipSpreadIntensity;
    [SerializeField] private float AdsSpreadIntensity;
    private float spreadIntensity;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletVelocity = 30f;
    [SerializeField] private float bulletPrefabLifeTime = 3f;

    [SerializeField] private GameObject MuzzleEffect;
    internal Animator animator;

    [Header("Reload")]
    [SerializeField] private float reloadTime;
    [SerializeField] internal int magazineSize, bulletsLeft;
    private bool isReloading = false;

    [Header("WeaponSpawn")]
    [SerializeField] internal Vector3 SpawnPosition;
    [SerializeField] internal Vector3 SpawnRotation;
    [SerializeField] internal Vector3 SpawnScale;

    internal bool IsADS;
    public enum WeaponType
    {
        Pistol_Glock,
        AR_M4,
    }
    public WeaponType thisWeaponType;

    public enum ShootingMode
    {
        Single,
        Bursts,
        Auto
    }

    [SerializeField] private ShootingMode currentShootingmode;

    private void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletsPerBurts;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
        spreadIntensity = HipSpreadIntensity;
    }
    void Update()
    {
        if (!IsActiveWeapon) return;

        HandleADSInput();
        if (isReloading) return;

        HandleShootingInput();
        HandleReloadInput();
        HandleFiring();
        HandleEmptyMagSound();
    }
    #region ==============[ Voor Update ]===============
    private void HandleADSInput()
    {
        if (Input.GetMouseButtonDown(1)) EnterADS();
        if (Input.GetMouseButtonUp(1)) ExitADS();
    }

    private void HandleShootingInput()
    {
        switch (currentShootingmode)
        {
            case ShootingMode.Auto:
                isShooting = Input.GetKey(KeyCode.Mouse0);
                break;
            case ShootingMode.Single:
            case ShootingMode.Bursts:
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
                break;
        }
    }

    private void HandleReloadInput()
    {
        bool needsReload = bulletsLeft < magazineSize;
        bool hasAmmo = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponType) > 0;

        if (Input.GetKeyDown(KeyCode.R) && needsReload && !isReloading && hasAmmo)
        {
            Reload();
        }
    }

    private void HandleFiring()
    {
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            BurstBulletsLeft = bulletsPerBurts;
            FireWeapon();
        }
    }

    private void HandleEmptyMagSound()
    {
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.EmptyMagSound.Play();
        }
    }
    #endregion Update

    #region ===================[ADS]==================
    private void EnterADS()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("EnterADS");
            IsADS = true;
            HudManager.Instance.MiddleDot.SetActive(false);
            spreadIntensity = AdsSpreadIntensity;
        }
    }
    private void ExitADS()
    {
        if (Input.GetMouseButtonUp(1))

        {
            animator.SetTrigger("ExitADS");
            IsADS = false;
            HudManager.Instance.MiddleDot.SetActive(true);
            spreadIntensity = HipSpreadIntensity;
        }
    }
    #endregion

    #region ==================[Shieten & Reloaden]==================
    private void FireWeapon()
    {
        bulletsLeft--;

        MuzzleEffect.GetComponent<ParticleSystem>().Play();
        if (IsADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }
        SoundManager.Instance.PlayShootingSound(thisWeaponType);

        readyToShoot = false;

        Vector3 shootDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        if (currentShootingmode == ShootingMode.Bursts && BurstBulletsLeft > 1)
        {
            BurstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        if (isReloading) return;

        isReloading = true;
        readyToShoot = false;
        Invoke("ReloadFinished", reloadTime);
        animator.SetTrigger("RELOAD");
        SoundManager.Instance.PlayReloadSound(thisWeaponType);

    }

    private void ReloadFinished()
    {
        int ammoNeeded = magazineSize - bulletsLeft;
        int ammoAvailable = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponType);

        if (ammoAvailable >= ammoNeeded)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(ammoNeeded, thisWeaponType);
        }
        else
        {
            bulletsLeft += ammoAvailable;
            WeaponManager.Instance.DecreaseTotalAmmo(ammoAvailable, thisWeaponType);
        }

        readyToShoot = true;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        //spread
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
#endregion
}