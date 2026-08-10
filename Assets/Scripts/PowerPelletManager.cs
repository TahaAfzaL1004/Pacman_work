using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    public float powerDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ActivatePowerMode(powerDuration);

            Destroy(gameObject);
        }
    }
}