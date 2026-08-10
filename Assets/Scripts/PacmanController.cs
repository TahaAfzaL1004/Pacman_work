using UnityEngine;
using UnityEngine.InputSystem;

public class PacmanController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody playerRb;

    private Vector3 currentDirection = Vector3.zero;
    private Vector3 requestedDirection = Vector3.zero;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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
}