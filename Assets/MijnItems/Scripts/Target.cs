using UnityEngine;

public class Target : MonoBehaviour
{
    private Quaternion originalRotation;
    void Start()
    {
        originalRotation = transform.rotation;
    }

    internal void RaiseTarget()
    {
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    internal void FallTarget()
    {
        transform.rotation = originalRotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            FallTarget();        
        }
    }
}