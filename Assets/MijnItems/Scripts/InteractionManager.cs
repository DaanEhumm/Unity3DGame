using UnityEngine;
using System.Collections;
using System;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon HoveredWeapon = null;

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
        }
    }
}
    