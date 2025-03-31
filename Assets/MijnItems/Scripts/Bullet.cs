using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit.gameObject.CompareTag("Target"))
        {
            print("hit" + objectHit.gameObject.name + "!");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Beer"))
        {
            print("hit a bottle");
            objectHit.gameObject.GetComponent<BeerBottle>().Shatter();
        }
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReff.Instance.BulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}
