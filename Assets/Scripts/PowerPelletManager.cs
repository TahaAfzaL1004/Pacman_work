using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    public int scoreValue = 50;
    public float powerDuration = 10f;

    private bool eaten = false;


    private void Start()
    {
        // Register this power pellet with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPellet(gameObject);
        }
        else
        {
            Debug.LogError(
                "GameManager.Instance is NULL when registering power pellet!"
            );
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (eaten)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        eaten = true;

        // Score
        GameManager.Instance.AddScore(scoreValue);

        // Activate power mode
        GameManager.Instance.ActivatePowerMode(
            powerDuration
        );

        // Tell GameManager this exact power pellet was eaten
        GameManager.Instance.PelletEaten(gameObject);

        // Destroy
        Destroy(gameObject);
    }
}