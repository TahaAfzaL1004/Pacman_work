using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Power Mode")]
    public bool isPowerModeActive;
    private float powerModeTimer;

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    private int Score = 0;

    [Header("Game End UI")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;

    // =========================================================
    // PELLET TRACKING
    // =========================================================

    private HashSet<GameObject> remainingPellets =
        new HashSet<GameObject>();

    private bool pelletsReady = false;
    private bool gameEnded = false;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        Time.timeScale = 1f;

        Score = 0;

        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (gameEnded)
        {
            return;
        }

        // Power mode timer
        if (isPowerModeActive)
        {
            powerModeTimer -= Time.deltaTime;

            if (powerModeTimer <= 0f)
            {
                isPowerModeActive = false;

                Debug.Log("Power Mode Ended");
            }
        }
    }


    // =========================================================
    // PELLET REGISTRATION
    // =========================================================

    public void RegisterPellet(GameObject pellet)
    {
        if (pellet == null)
        {
            return;
        }

        if (remainingPellets.Add(pellet))
        {
            Debug.Log(
                "Pellet registered. Total remaining: " +
                remainingPellets.Count
            );
        }
    }


    // =========================================================
    // PELLET EATEN
    // =========================================================

    public void PelletEaten(GameObject pellet)
    {
        if (gameEnded)
        {
            return;
        }

        if (pellet == null)
        {
            return;
        }

        // Remove this exact pellet from our tracking list
        bool wasTracked = remainingPellets.Remove(pellet);

        if (!wasTracked)
        {
            Debug.LogWarning(
                "Tried to remove a pellet that was not registered."
            );

            return;
        }

        Debug.Log(
            "Pellet eaten! Actual tracked pellets remaining: " +
            remainingPellets.Count
        );

        // THIS is the only place where winning is checked.
        if (remainingPellets.Count <= 1)
        {
            YouWin();
        }
    }


    // =========================================================
    // GENERATION COMPLETE
    // =========================================================

    public void PelletsGenerationComplete()
    {
        pelletsReady = true;

        Debug.Log(
            "Pellet generation complete. Total pellets: " +
            remainingPellets.Count
        );

        // Safety check
        if (remainingPellets.Count == 0)
        {
            Debug.LogWarning(
                "Pellet generation completed but there are 0 registered pellets."
            );
        }
    }


    // =========================================================
    // SCORE
    // =========================================================

    public void AddScore(int amount)
    {
        if (gameEnded)
        {
            return;
        }

        Score += amount;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score;
        }
    }


    // =========================================================
    // POWER MODE
    // =========================================================

    public void ActivatePowerMode(float duration)
    {
        if (gameEnded)
        {
            return;
        }

        isPowerModeActive = true;
        powerModeTimer = duration;

        Debug.Log("Power Mode Activated");

        GhostAI[] allGhosts =
            Object.FindObjectsByType<GhostAI>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        foreach (GhostAI ghost in allGhosts)
        {
            ghost.BecomeFrightened();
        }
    }


    // =========================================================
    // GHOST SCORE
    // =========================================================

    public void AddGhostScore()
    {
        if (gameEnded)
        {
            return;
        }

        AddScore(200);

        Debug.Log("Ghost Eaten! 200 Points.");
    }


    // =========================================================
    // PACMAN DIED
    // =========================================================

    public void PacmanDied()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        Debug.Log("GAME OVER: Pacman was eaten!");

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }

        Time.timeScale = 0f;
    }


    // =========================================================
    // YOU WIN
    // =========================================================

    private void YouWin()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        isPowerModeActive = false;

        Debug.Log("=================================");
        Debug.Log("YOU WIN!");
        Debug.Log("All pellets have been eaten.");
        Debug.Log("=================================");

        if (winText != null)
        {
            winText.gameObject.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        Time.timeScale = 0f;
    }
}