using UnityEngine;
using UnityEngine.InputSystem;

public class PacmanController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody playerRb;
    private Vector3 startPosition;

    private Vector3 currentDirection = Vector3.zero;
    private Vector3 requestedDirection = Vector3.zero;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Save spawn position for respawning
    }

    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            requestedDirection = Vector3.forward;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            requestedDirection = Vector3.back;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            requestedDirection = Vector3.left;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            requestedDirection = Vector3.right;
        }
    }

    void FixedUpdate()
    {
        if (requestedDirection != Vector3.zero)
        {
            currentDirection = requestedDirection;
        }

        if (currentDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentDirection);

            playerRb.MovePosition(
                playerRb.position +
                currentDirection * moveSpeed * Time.fixedDeltaTime
            );
        }
    }

    // Called by GhostAI.cs when a deadly ghost touches Pac-Man
   public void Die()
    {
        Debug.Log("Pac-Man Died!");
        
        transform.position = startPosition;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
        }

        currentDirection = Vector3.zero;
        requestedDirection = Vector3.zero;

        // Cleaned up API call: Uses FindAnyObjectByType instead
        GhostSpawner spawner = Object.FindAnyObjectByType<GhostSpawner>();
        if (spawner != null)
        {
            spawner.SpawnAllGhosts();
        }
    }
 }
