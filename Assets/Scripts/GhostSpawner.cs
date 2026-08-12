using UnityEngine;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost Prefabs")]
    public GameObject ghostBlinkyPrefab;
    public GameObject ghostPinkyPrefab;

    [Header("Spawn Points")]
    public Transform blinkySpawnPoint;
    public Transform pinkySpawnPoint;
    
    [Header("Routing")]
    public Transform ghostHouseTarget;

    private List<GameObject> activeGhosts = new List<GameObject>();

    void Start()
    {
        SpawnAllGhosts();
    }

    public void SpawnAllGhosts()
    {
        // Wipe existing ghosts before respawning
        foreach (GameObject ghost in activeGhosts)
        {
            if (ghost != null) Destroy(ghost);
        }
        activeGhosts.Clear();

        if (ghostBlinkyPrefab != null && blinkySpawnPoint != null)
        {
            SpawnSingleGhost(ghostBlinkyPrefab, blinkySpawnPoint);
        }

        if (ghostPinkyPrefab != null && pinkySpawnPoint != null)
        {
            SpawnSingleGhost(ghostPinkyPrefab, pinkySpawnPoint);
        }
    }

    private void SpawnSingleGhost(GameObject prefab, Transform spawnTransform)
    {
        GameObject newGhost = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
        activeGhosts.Add(newGhost);

        GhostAI ai = newGhost.GetComponent<GhostAI>();
        if (ai != null)
        {
            ai.ghostHouse = ghostHouseTarget;
        }
        else
        {
            Debug.LogError("GhostAI script missing on spawned prefab: " + prefab.name);
        }
    }
}