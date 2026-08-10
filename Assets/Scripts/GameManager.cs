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
    }
}