using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPowerModeActive;
    private float powerModeTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isPowerModeActive)
        {
            powerModeTimer -= Time.deltaTime;

            if (powerModeTimer <= 0)
            {
                isPowerModeActive = false;
                Debug.Log("Power Mode Ended");
            }
        }
    }

    public void ActivatePowerMode(float duration)
    {
        isPowerModeActive = true;
        powerModeTimer = duration;

        Debug.Log("Power Mode Activated");

        GhostAI[] allGhosts = Object.FindObjectsByType<GhostAI>(FindObjectsInactive.Exclude);
        foreach (GhostAI ghost in allGhosts)
        {
            ghost.BecomeFrightened();
        }
    }

    // Your friend can implement this later
    /*
    public void AddGhostScore()
    {
        // Add score logic here
    }
    */
}