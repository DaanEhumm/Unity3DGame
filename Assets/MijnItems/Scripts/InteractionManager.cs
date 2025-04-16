using UnityEngine;
using System.Collections;
using System;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    [SerializeField] internal Weapon HoveredWeapon = null;
    [SerializeField] internal Ammobox HoveredAmmoBox = null;
    [SerializeField] internal Throwables HoveredThrowables = null;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            HandleWeaponHighlight(hitObject);
            HandleAmmoHighlight(hitObject);
            HandleThrowableHighlight(hitObject);

        }
        else
        {
            ClearAllHighLights();
        }
    }
    #region ==============[ Voor Update ]===============
    private void HandleWeaponHighlight(GameObject obj)
    {
        Weapon weapon = obj.GetComponent<Weapon>();
        if (weapon != null && !weapon.IsActiveWeapon)
        {
            HoveredWeapon = weapon;
            HoveredWeapon.GetComponent<Outline>().enabled = true;
            HudManager.Instance.ShowInteractionText("Press F to Pickup Weapon");

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickUpWeapon(obj);
                HudManager.Instance.HideInteractionText();
            }
        }
        else if (HoveredWeapon != null)
        {
            HoveredWeapon.GetComponent<Outline>().enabled = false;
            HoveredWeapon = null;
            HudManager.Instance.HideInteractionText();
        }
    }
    private void HandleAmmoHighlight(GameObject obj)
    {
        Ammobox ammo = obj.GetComponent<Ammobox>();
        if (ammo != null)
        {
            HoveredAmmoBox = ammo;
            HoveredAmmoBox.GetComponent<Outline>().enabled = true;
            HudManager.Instance.ShowInteractionText("Press F to Pickup Ammo");

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickUpAmmo(HoveredAmmoBox);
                Destroy(HoveredAmmoBox.gameObject);
                HudManager.Instance.HideInteractionText();
            }
        }
        else if (HoveredAmmoBox != null)
        {
            HoveredAmmoBox.GetComponent<Outline>().enabled = false;
            HoveredAmmoBox = null;
            HudManager.Instance.HideInteractionText();
        }
    }

    private void HandleThrowableHighlight(GameObject obj)
    {
        Throwables throwable = obj.GetComponent<Throwables>();
        if (throwable != null)
        {
            HoveredThrowables = throwable;
            HoveredThrowables.GetComponent<Outline>().enabled = true;
            HudManager.Instance.ShowInteractionText("Press F to Pickup Throwable");

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickUpThrowables(HoveredThrowables);
                HudManager.Instance.HideInteractionText();
            }
        }
        else if (HoveredThrowables != null)
        {
            HoveredThrowables.GetComponent<Outline>().enabled = false;
            HoveredThrowables = null;
            HudManager.Instance.HideInteractionText();
        }
    }

    private void ClearAllHighLights()
    {
        if (HoveredWeapon != null)
        {
            HoveredWeapon.GetComponent<Outline>().enabled = false;
            HoveredWeapon = null;
        }

        if (HoveredAmmoBox != null)
        {
            HoveredAmmoBox.GetComponent<Outline>().enabled = false;
            HoveredAmmoBox = null;
        }

        if (HoveredThrowables != null)
        {
            HoveredThrowables.GetComponent<Outline>().enabled = false;
            HoveredThrowables = null;
        }
        HudManager.Instance.HideInteractionText();
    }
    #endregion Update
}
