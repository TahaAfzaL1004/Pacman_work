using UnityEngine;

public class PelletManager : MonoBehaviour
{
    public GameObject pelletPrefab;
    public Transform pelletPositions;

    void Start()
    {
        foreach (Transform position in pelletPositions)
        {
            Instantiate(pelletPrefab, position.position, Quaternion.identity);
        }
    }
}