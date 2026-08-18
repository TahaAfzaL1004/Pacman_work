using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PelletManager : MonoBehaviour
{
    public GameObject pelletPrefab;
    public GameObject maze;

    [Header("Pellet Settings")]
    public float pelletSpacing = 1f;
    public float pelletY = 0.5f;

    [Header("NavMesh")]
    public float navMeshCheckDistance = 1f;

    [Header("Wall Clearance")]
    public LayerMask wallLayer;
    public float wallCheckRadius = 0.25f;

    [Header("No Pellet Zones")]
    public List<GameObject> noPelletZones = new List<GameObject>();

    [Header("Duplicate Prevention")]
    public float minimumPelletDistance = 0.7f;

    [Header("Corridor Settings")]
    public float corridorCheckDistance = 2f;
    public float corridorCenterTolerance = 0.35f;

    private List<Vector3> pelletPositions = new List<Vector3>();


    private void Start()
    {
        GeneratePellets();

        // Wait one frame before telling GameManager
        // that pellet generation is finished.
        StartCoroutine(NotifyGameManager());
    }


    private IEnumerator NotifyGameManager()
    {
        // This is VERY important.
        //
        // It allows all other Start() functions, including
        // PowerPelletManager, to finish creating their pellets.
        yield return null;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PelletsGenerationComplete();
        }
    }


    private void GeneratePellets()
    {
        // Safety check
        if (pelletSpacing <= 0.1f)
        {
            Debug.LogError(
                "pelletSpacing is too small or 0! Forcing to 1f."
            );

            pelletSpacing = 1f;
        }


        Renderer[] renderers =
            maze.GetComponentsInChildren<Renderer>();


        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in Maze!");
            return;
        }


        Bounds mazeBounds = renderers[0].bounds;


        foreach (Renderer renderer in renderers)
        {
            mazeBounds.Encapsulate(renderer.bounds);
        }


        float startX = mazeBounds.min.x;
        float startZ = mazeBounds.min.z;


        int maxSafetyCounter = 0;


        for (
            float x = startX;
            x <= mazeBounds.max.x;
            x += pelletSpacing
        )
        {
            for (
                float z = startZ;
                z <= mazeBounds.max.z;
                z += pelletSpacing
            )
            {
                maxSafetyCounter++;


                if (maxSafetyCounter > 3000)
                {
                    Debug.LogError(
                        "Pellet generation exceeded safety limit!"
                    );

                    return;
                }


                Vector3 position =
                    new Vector3(x, pelletY, z);


                TrySpawnPellet(position);
            }
        }


        Debug.Log(
            "Pellets generated: " +
            pelletPositions.Count
        );
    }


    private void TrySpawnPellet(Vector3 position)
    {
        NavMeshHit hit;


        // Must be on NavMesh
        if (!NavMesh.SamplePosition(
            position,
            out hit,
            navMeshCheckDistance,
            NavMesh.AllAreas))
        {
            return;
        }


        // Must not be inside a wall
        if (Physics.CheckSphere(
            position,
            wallCheckRadius,
            wallLayer))
        {
            return;
        }


        // Must not be inside a no-pellet zone
        if (InsideNoPelletZone(position))
        {
            return;
        }


        bool horizontal;
        bool vertical;


        GetCorridorDirection(
            position,
            out horizontal,
            out vertical
        );


        // Must be inside a corridor
        if (!horizontal && !vertical)
        {
            return;
        }


        // Prevent duplicates
        foreach (Vector3 existingPosition in pelletPositions)
        {
            if (
                Vector3.Distance(
                    position,
                    existingPosition
                ) < minimumPelletDistance
            )
            {
                return;
            }
        }


        // Create pellet
        Instantiate(
            pelletPrefab,
            position,
            Quaternion.identity
        );


        pelletPositions.Add(position);
    }


    private void GetCorridorDirection(
        Vector3 position,
        out bool horizontal,
        out bool vertical)
    {
        horizontal = false;
        vertical = false;


        bool leftOpen =
            IsWalkable(
                position +
                Vector3.left *
                pelletSpacing
            );


        bool rightOpen =
            IsWalkable(
                position +
                Vector3.right *
                pelletSpacing
            );


        bool forwardOpen =
            IsWalkable(
                position +
                Vector3.forward *
                pelletSpacing
            );


        bool backwardOpen =
            IsWalkable(
                position +
                Vector3.back *
                pelletSpacing
            );


        if (leftOpen || rightOpen)
        {
            horizontal = true;
        }


        if (forwardOpen || backwardOpen)
        {
            vertical = true;
        }
    }


    private bool IsWalkable(Vector3 position)
    {
        NavMeshHit hit;


        if (!NavMesh.SamplePosition(
            position,
            out hit,
            navMeshCheckDistance,
            NavMesh.AllAreas))
        {
            return false;
        }


        if (Physics.CheckSphere(
            position,
            wallCheckRadius,
            wallLayer))
        {
            return false;
        }


        return true;
    }


    private bool InsideNoPelletZone(Vector3 position)
    {
        foreach (GameObject zone in noPelletZones)
        {
            if (zone == null)
            {
                continue;
            }


            Collider zoneCollider =
                zone.GetComponent<Collider>();


            if (
                zoneCollider != null &&
                zoneCollider.bounds.Contains(position)
            )
            {
                return true;
            }
        }


        return false;
    }
}