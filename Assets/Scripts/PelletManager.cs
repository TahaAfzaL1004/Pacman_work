using UnityEngine;

public class PelletManager : MonoBehaviour
{
    public GameObject pelletPrefab;
    public GameObject maze;

    public LayerMask wallLayer;

    public float pelletSpacing = 1f;
    public float pelletY = 0.5f;

    public Vector3 pelletCheckSize = new Vector3(1f, 2f, 1f);

    public NoPelletZone[] noPelletZones;

    void Start()
    {
        GeneratePellets();
    }

    void GeneratePellets()
    {
        Renderer[] renderers = maze.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in maze!");
            return;
        }

        Bounds mazeBounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            mazeBounds.Encapsulate(renderer.bounds);
        }

        for (float x = mazeBounds.min.x; x <= mazeBounds.max.x; x += pelletSpacing)
        {
            for (float z = mazeBounds.min.z; z <= mazeBounds.max.z; z += pelletSpacing)
            {
                Vector3 position = new Vector3(x, pelletY, z);

                // Check for walls
                if (IsNearWall(position))
                {
                    continue;
                }

                // Check restricted areas
                if (IsInsideNoPelletZone(position))
                {
                    continue;
                }

                Instantiate(
                    pelletPrefab,
                    position,
                    Quaternion.identity
                );
            }
        }
    }

    bool IsNearWall(Vector3 position)
{
    Collider[] colliders = Physics.OverlapBox(
        position,
        pelletCheckSize / 2f,
        Quaternion.identity,
        wallLayer
    );

    return colliders.Length > 0;
}

    bool IsInsideNoPelletZone(Vector3 position)
    {
        foreach (NoPelletZone zone in noPelletZones)
        {
            if (zone == null)
            {
                continue;
            }

            Collider zoneCollider = zone.GetComponent<Collider>();

            if (zoneCollider != null &&
                zoneCollider.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }
}