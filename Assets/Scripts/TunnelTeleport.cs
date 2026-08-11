using UnityEngine;

public class TunnelTeleport : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.position = teleportTarget.position;
            }
            else
            {
                other.transform.position = teleportTarget.position;
            }
        }
    }
}