using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private bool isShooting, readyToShoot;
    private bool allowReset = true;
    [SerializeField] private float shootingDelay = 2f;

    [SerializeField] private int bulletsPerBurts = 3;
    [SerializeField] private int BurstBulletsLeft;

    [SerializeField] private float spreadIntensity;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletVelocity = 30f;
    [SerializeField] private float bulletPrefabLifeTime = 3f;

    [SerializeField] private GameObject MuzzleEffect;
    private Animator animator;

    [SerializeField] private float reloadTime;
    [SerializeField] private int magazineSize, bulletsLeft;
    public bool isReloading;

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
    }

    void Update()
    {
        if (currentShootingmode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingmode == ShootingMode.Single)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        else if (currentShootingmode == ShootingMode.Bursts)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            BurstBulletsLeft = bulletsPerBurts;
            FireWeapon();
        }
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.EmptyMagSoundGlock.Play();
        }
        if (AmmoManager.Instance.AmmoCount != null)
        {
           AmmoManager.Instance.AmmoCount.text = $"{bulletsLeft / bulletsPerBurts}/{magazineSize / bulletsPerBurts}";
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        MuzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.shootingSoundGlock.Play();

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
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
        animator.SetTrigger("RELOAD");
        SoundManager.Instance.ReloadSoundGlock.Play();

    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
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
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}