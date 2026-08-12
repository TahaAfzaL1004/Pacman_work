using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0f, 15f, -10f);

    [Header("Follow Settings")]
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}