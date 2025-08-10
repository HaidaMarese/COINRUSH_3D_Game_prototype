using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject[] treePrefabs;

    [Header("How many & where")]
    public int count = 20;
    public Vector2 area = new Vector2(18f, 18f); 
    public float y = 0f;

    [Header("Placement rules")]
    public float minSpacing = 2.0f;          
    public LayerMask blockedLayers;          
    public float overlapCheckRadius = 0.75f; 

    [Header("Randomization")]
    public Vector2 uniformScaleRange = new Vector2(0.9f, 1.3f);
    public bool randomYRotation = true;

    [Header("Make them solid")]
    public bool addCapsuleColliderIfMissing = true;
    public string obstacleLayerName = "Obstacles";
    public bool markStatic = true;

    void Start()
    {
        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogWarning("[TreeSpawner] No tree prefabs assigned.");
            return;
        }

        int placed = 0, tries = 0, maxTries = Mathf.Max(count * 50, 500);
        while (placed < count && tries < maxTries)
        {
            tries++;

            float x = Random.Range(-area.x * 0.5f, area.x * 0.5f);
            float z = Random.Range(-area.y * 0.5f, area.y * 0.5f);
            Vector3 pos = new Vector3(x, y, z) + transform.position;

            if (Physics.CheckSphere(pos, overlapCheckRadius, blockedLayers))
                continue;

            GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            Quaternion rot = randomYRotation ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) : Quaternion.identity;
            GameObject t = Instantiate(prefab, pos, rot, transform);

            float s = Random.Range(uniformScaleRange.x, uniformScaleRange.y);
            t.transform.localScale = new Vector3(s, s, s);

            if (addCapsuleColliderIfMissing && t.GetComponentInChildren<Collider>() == null)
            {
                var col = t.AddComponent<CapsuleCollider>();
                col.center = new Vector3(0f, 1f, 0f); // tweak to your tree
                col.height = 2.0f;
                col.radius = 0.35f;
            }

            int layer = LayerMask.NameToLayer(obstacleLayerName);
            if (layer >= 0) SetLayerRecursively(t, layer);

            if (markStatic) t.isStatic = true;

            placed++;
        }

        if (placed < count)
            Debug.LogWarning($"[TreeSpawner] Placed {placed}/{count} trees (ran out of space).");
    }

    void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform c in go.transform) SetLayerRecursively(c.gameObject, layer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.6f, 0.2f, 0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(area.x, 0.1f, area.y));
        Gizmos.color = new Color(0f, 0.6f, 0.2f, 0.9f);
        Gizmos.DrawWireCube(transform.position, new Vector3(area.x, 0.1f, area.y));
    }
}
