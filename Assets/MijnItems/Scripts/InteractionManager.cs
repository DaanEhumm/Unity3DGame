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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            //weapon
            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().IsActiveWeapon == false)
            {
                HoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                HoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (HoveredWeapon)
                {
                   HoveredWeapon.GetComponent<Outline>().enabled = false;   
                }
            }

            //ammo
            if (objectHitByRaycast.GetComponent<Ammobox>())
            {
                HoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<Ammobox>();
                HoveredAmmoBox.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpAmmo(HoveredAmmoBox);
                    Destroy(HoveredAmmoBox.gameObject);
                }
            }
            else
            {
                if (HoveredAmmoBox)
                {
                    HoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            //Throwables
            if (objectHitByRaycast.GetComponent<Throwables>())
            {
                HoveredThrowables = objectHitByRaycast.gameObject.GetComponent<Throwables>();
                HoveredThrowables.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpThrowables(HoveredThrowables);
                }
            }
            else
            {
                if (HoveredThrowables)
                {
                    HoveredThrowables.GetComponent<Outline>().enabled = false;
                }
            }
        }
    } // Ook deze updat eis nog te lang en hier moet ik nog wat aan doen 
}
    