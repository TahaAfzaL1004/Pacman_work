using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    public int scoreValue = 50;
    public float powerDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(scoreValue);

            GameManager.Instance.ActivatePowerMode(powerDuration);

            Destroy(gameObject);
        }
    }
}