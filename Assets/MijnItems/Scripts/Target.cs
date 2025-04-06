using UnityEngine;

public class Target : MonoBehaviour
{
    private Quaternion originalRotation;
    void Start()
    {
        originalRotation = transform.rotation;
    }

    public void RaiseTarget()
    {
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    public void FallTarget()
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