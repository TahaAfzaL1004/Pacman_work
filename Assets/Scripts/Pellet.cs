using UnityEngine;

public class Pellet : MonoBehaviour
{
    private bool eaten = false;


    private void Start()
    {
        // Register this pellet with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPellet(gameObject);
        }
        else
        {
            Debug.LogError(
                "GameManager.Instance is NULL when registering pellet!"
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

        // Add score
        GameManager.Instance.AddScore(10);

        // Tell GameManager that THIS EXACT pellet was eaten
        GameManager.Instance.PelletEaten(gameObject);

        // Destroy pellet
        Destroy(gameObject);
    }
}