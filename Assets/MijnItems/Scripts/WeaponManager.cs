using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    // weopon slots 
    public List<GameObject> weaponSlots;
    public GameObject ActiveWeaponSlot;
    [Header ("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    [Header ("throwables general")]
    public int throwForce = 15;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float MaxForceMultiplied = 2f;
    [Header ( "Lethals")]
    public int LethalsCount = 0;
    public Throwables.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;
    public int maxLethalsCount = 4;
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
    private void Start()
    {
        ActiveWeaponSlot = weaponSlots[0];
        equippedLethalType = Throwables.ThrowableType.none;
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == ActiveWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
            {

            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwithActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwithActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > MaxForceMultiplied)
            {
                forceMultiplier = MaxForceMultiplied;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if(LethalsCount > 0)
            {
                ThrowLethal();
            }
            forceMultiplier = 0;
        }
    }
    // Throwables
    internal void PickUpThrowables(Throwables throwables)
    {
       switch (throwables.throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                PickUpThrowablesAsLethal(Throwables.ThrowableType.Grenade);
                break;
                //going to add more
        }
    }
    private void PickUpThrowablesAsLethal(Throwables.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwables.ThrowableType.none)
        {
            equippedLethalType = lethal;

            if (LethalsCount != maxLethalsCount)
            {
                LethalsCount += 1;
                Destroy(InteractionManager.Instance.HoveredThrowables.gameObject);
                HudManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                Debug.Log ($"Lethal Limit Reached {LethalsCount} {lethal}");
            }
        }
        
    }

    private void ThrowLethal()
    {
        GameObject LethalPrefab = GetThrowablePrefab();
        GameObject throwable = Instantiate(LethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwables>().hasBeenThrown = true;
        LethalsCount -= 1;
        if (LethalsCount <= 0)
        {
            equippedLethalType = Throwables.ThrowableType.none;
        }


        HudManager.Instance.UpdateThrowablesUI();
    }

    private GameObject GetThrowablePrefab()
    {
        switch(equippedLethalType)
        {
            case Throwables.ThrowableType.Grenade:
                return grenadePrefab;
        }
        return new();
    }


    // Weapon 
    internal void PickUpWeapon(GameObject PickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(PickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {

        DropCurrentWeapon(pickedUpWeapon);
        pickedUpWeapon.transform.SetParent(ActiveWeaponSlot.transform, false);

        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapon.SpawnPosition.x, weapon.SpawnPosition.y, weapon.SpawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.SpawnRotation.x, weapon.SpawnRotation.y, weapon.SpawnRotation.z);

        weapon.IsActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (ActiveWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = ActiveWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().IsActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwithActiveSlot(int slotNumber)
    {
        if (ActiveWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = ActiveWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.IsActiveWeapon = false;
        }
        ActiveWeaponSlot = weaponSlots[slotNumber];

        if (ActiveWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = ActiveWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.IsActiveWeapon = true;
        }
    }
    // Ammo 
    internal void PickUpAmmo(Ammobox ammo)
    {
        switch (ammo.ammoType)
        {
            case Ammobox.AmmoType.Pistol:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case Ammobox.AmmoType.Rifle:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }
    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponType thisWeaponType)
    {
        switch (thisWeaponType)
        {
            case Weapon.WeaponType.Pistol_Glock:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponType.AR_M4:
                totalRifleAmmo -= bulletsToDecrease;
                break;
        }
    }
    public int CheckAmmoLeftFor(Weapon.WeaponType thisWeaponType)
    {
        switch (thisWeaponType)
        {
            case WeaponType.Pistol_Glock:
                return totalPistolAmmo;
            case WeaponType.AR_M4:
                return Instance.totalRifleAmmo;
            default:
                return 0;
        }
    }
}   
