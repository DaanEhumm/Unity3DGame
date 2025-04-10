using UnityEngine;
using System.Collections;
using System;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    //Weapon
    public AudioSource ShootingChannel;
    public AudioSource ReloadChannel;
    
    public AudioSource EmptyMagSound;
    //Weapon specific
    public AudioClip AR_M4_Shoot;
    public AudioClip AR_M4_Reload;
    public AudioClip Pistol_glock_Shoot;
    public AudioClip Pistol_glock_Reload;
    //Throwables
    public AudioSource ThrowablesChannel;
    public AudioClip GrenadeSound;

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

    public void PlayShootingSound(WeaponType weapon)
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
    public void PlayReloadSound(WeaponType weapon) 
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
