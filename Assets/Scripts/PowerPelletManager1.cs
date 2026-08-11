using UnityEngine;

public class PowerPelletManager : MonoBehaviour
{
    public GameObject powerPelletPrefab;
    public Transform[] powerPelletPositions;

    void Start()
    {
        foreach (Transform position in powerPelletPositions)
        {
            Instantiate(
                powerPelletPrefab,
                position.position,
                Quaternion.identity
            );
        }
    }
}