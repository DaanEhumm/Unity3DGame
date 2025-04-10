using UnityEngine;
using System.Collections;
using System;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("Weapon")]
    [SerializeField] internal AudioSource ShootingChannel;
    [SerializeField] internal AudioSource ReloadChannel;
    [SerializeField] internal AudioSource EmptyMagSound;

    [Header("Weapon specific")]
    [SerializeField] internal AudioClip AR_M4_Shoot;
    [SerializeField] internal AudioClip AR_M4_Reload;
    [SerializeField] internal AudioClip Pistol_glock_Shoot;
    [SerializeField] internal AudioClip Pistol_glock_Reload;

    [Header("Throwables")]
    [SerializeField] internal AudioSource ThrowablesChannel;
    [SerializeField] internal AudioClip GrenadeSound;

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

    internal void PlayShootingSound(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Pistol_Glock:
                ShootingChannel.PlayOneShot(Pistol_glock_Shoot);
                break;
            case WeaponType.AR_M4:
                ShootingChannel.PlayOneShot(AR_M4_Shoot);
                break;
        }
    }
    internal void PlayReloadSound(WeaponType weapon) 
    {
        switch (weapon)
        {
            case WeaponType.Pistol_Glock:
                ReloadChannel.PlayOneShot(Pistol_glock_Reload);
                break;
            case WeaponType.AR_M4:
                ReloadChannel.PlayOneShot(AR_M4_Reload);
                break;
        }
    }
}
