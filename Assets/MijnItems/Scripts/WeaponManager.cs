using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;
    public GameObject ActiveWeaponSlot;
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
    }

    public void PickUpWeapon(GameObject PickedUpWeapon)
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
}   
