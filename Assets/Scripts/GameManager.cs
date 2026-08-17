using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPowerModeActive;
    private float powerModeTimer;
    public TextMeshProUGUI scoreText;
    private int Score = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
         Score = 0;
    scoreText.text = "Score: 0";
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

    public void AddScore(int amount)
{
    Score += amount;
    scoreText.text = "Score: " + Score;
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