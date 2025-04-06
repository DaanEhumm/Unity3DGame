using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit.gameObject.CompareTag("Target"))
        {
            Score.Instance.AddScore(10);
            print("hit a target");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Wall"))
        {
            Score.Instance.AddScore(-5);
            print("hit a wall");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Beer"))
        {
            Score.Instance.AddScore(10);
            print("hit a bottle");
            objectHit.gameObject.GetComponent<BeerBottle>().Shatter();
            Destroy(gameObject);
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
