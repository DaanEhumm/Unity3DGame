using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; set; }
    //ammo
    [SerializeField] private TextMeshProUGUI MagazineAmmoUI;
    [SerializeField] private TextMeshProUGUI TotalAmmoUI;
    [SerializeField] private Image AmmoTypeUI;
    //weapon
    [SerializeField] private Image ActiveWeaponUI;
    [SerializeField] private Image UnActiveWeaponUI;
    //Throwables
    [SerializeField] private Image LethalUI;
    [SerializeField] private TextMeshProUGUI LethallUI;
    [SerializeField] private Image TacticalUI;
    [SerializeField] private TextMeshProUGUI TacticallUI;

    [SerializeField] private Sprite emptyslot;
    [SerializeField] private Sprite greyslot;
    [SerializeField] internal GameObject MiddleDot;

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

    private void Update()
    {
        UpdateAmmoUI();
        UpdateWeaponUI();
    }

    private void UpdateAmmoUI()
    {
        Weapon activeWeapon = WeaponManager.Instance.ActiveWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            MagazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurts}";
            TotalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponType)}";

            Weapon.WeaponType type = activeWeapon.thisWeaponType;
            AmmoTypeUI.sprite = GetAmmoSprite(type);

            ActiveWeaponUI.sprite = GetWeaponSprite(type);

            if (unActiveWeapon)
            {
                UnActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponType);
            }
        }
        else
        {
            MagazineAmmoUI.text = "";
            TotalAmmoUI.text = "";

            AmmoTypeUI.sprite = emptyslot;
            ActiveWeaponUI.sprite = emptyslot;
            UnActiveWeaponUI.sprite = emptyslot;
        }
        if (WeaponManager.Instance.LethalsCount <=0)
        {
            LethalUI.sprite = greyslot;
            TacticalUI.sprite = greyslot;

        }
    }

    private void UpdateWeaponUI()
    {
        //zit in de updateAmmoUI
    }

    private Sprite GetWeaponSprite(Weapon.WeaponType type)
    {
       switch (type)
        {
            case Weapon.WeaponType.Pistol_Glock:
                return Resources.Load<GameObject>("Pistol_Glock_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponType.AR_M4:
                return Resources.Load<GameObject>("AR_M4_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponType type)
    {
        switch (type)
        {
            case Weapon.WeaponType.Pistol_Glock:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponType.AR_M4:
                return Resources.Load<GameObject>("AR_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.ActiveWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
    internal void UpdateThrowablesUI()
    {
        LethallUI.text = $"{WeaponManager.Instance.LethalsCount}";
        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwables.ThrowableType.Grenade:
                LethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
