//See on chatgpt slop kood, kiire oli, võid vabalt üle kirjutada ja kustutada
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;

    [Header("Setup")]
    public GridLayout gridLayout;
    public GameObject prefab1;
    public GameObject prefab2;
    public string buildPlatformTag = "BuildPlatform";

    private Grid grid;
    private readonly Dictionary<Vector3Int, GameObject> placed = new Dictionary<Vector3Int, GameObject>();

    void Awake()
    {
        Instance = this;
        grid = gridLayout.GetComponent<Grid>();
        if (grid.cellSize.z <= 0.0001f)
            grid.cellSize = new Vector3(grid.cellSize.x, grid.cellSize.y, grid.cellSize.x);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) InitializeWithObject(prefab1);
        else if (Input.GetKeyDown(KeyCode.B)) InitializeWithObject(prefab2);
    }

    public Quaternion GridRotation => grid.transform.rotation;
    public void AlignToGrid(Transform t)
    {
        if (t) t.rotation = grid.transform.rotation;
    }

    Vector3Int OffsetFromNormalInGrid(Vector3 worldNormal)
    {
        var n = grid.transform.InverseTransformDirection(worldNormal).normalized;
        float ax = Mathf.Abs(n.x), ay = Mathf.Abs(n.y), az = Mathf.Abs(n.z);
        if (ay >= ax && ay >= az) return new Vector3Int(0, n.y > 0 ? 1 : -1, 0);
        if (ax >= az)             return new Vector3Int(n.x > 0 ? 1 : -1, 0, 0);
        return new Vector3Int(0, 0, n.z > 0 ? 1 : -1);
    }

    float PlatformTopAt(Vector3 world)
    {
        if (Physics.Raycast(world + Vector3.up * 50f, Vector3.down, out var h, 200f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            if (h.collider.CompareTag(buildPlatformTag))
                return h.collider.bounds.max.y;

        var c = GameObject.FindGameObjectWithTag(buildPlatformTag);
        return c ? c.GetComponent<Collider>().bounds.max.y : 0f;
    }

    Vector3Int CellXZAt(Vector3 world)
    {
        var local = grid.transform.InverseTransformPoint(world);
        var cs = grid.cellSize;
        int x = Mathf.FloorToInt(local.x / cs.x);
        int z = Mathf.FloorToInt(local.z / cs.z);
        return new Vector3Int(x, 0, z);
    }

    Vector3 FromKeyToWorld(Vector3Int key, float baseCenterY)
    {
        var cs = grid.cellSize;
        var local = new Vector3((key.x + 0.5f) * cs.x, 0f, (key.z + 0.5f) * cs.z);
        var world = grid.transform.TransformPoint(local);
        return new Vector3(world.x, baseCenterY + key.y * cs.y, world.z);
    }

    Vector3Int GetKeyFor(GameObject go)
    {
        foreach (var kv in placed)
            if (kv.Value == go) return kv.Key;

        var baseCenterY = PlatformTopAt(go.transform.position) + grid.cellSize.y * 0.5f;
        var key = CellXZAt(go.transform.position);
        key.y = Mathf.RoundToInt((go.transform.position.y - baseCenterY) / grid.cellSize.y);
        return key;
    }

    // Dictionary + physics-overlap guard
    bool IsCellFree(Vector3Int key, float baseCenterY, Transform ignoreRoot)
    {
        if (placed.TryGetValue(key, out var existing))
            if (!(ignoreRoot && existing && existing.transform.root == ignoreRoot))
                return false;

        var center = FromKeyToWorld(key, baseCenterY);
        var half = grid.cellSize * 0.5f * 0.98f;
        var hits = Physics.OverlapBox(center, half, grid.transform.rotation, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        foreach (var col in hits)
        {
            var root = col.transform.root;
            if (ignoreRoot && root == ignoreRoot) continue;
            if (root && root.CompareTag("Cube")) return false;
        }
        return true;
    }

    // Scan through ALL hits until we find a cube face or the build platform.
    public bool TryGetSnappedPos(out Vector3 snappedPos, Transform ignore = null)
    {
        snappedPos = Vector3.zero;

        var cam = Camera.main;
        if (!cam) return false;

        var hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition), 1000f,
                                      Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                          .OrderBy(h => h.distance);

        foreach (var hit in hits)
        {
            if (ignore && hit.collider && hit.collider.transform.root == ignore.root) continue;

            // 1) Cube face
            if (hit.collider.transform.root.CompareTag("Cube"))
            {
                var rootGO = hit.collider.transform.root.gameObject;
                var hitKey  = GetKeyFor(rootGO);
                var off     = OffsetFromNormalInGrid(hit.normal);

                float baseCenterY = PlatformTopAt(hit.collider.transform.position) + grid.cellSize.y * 0.5f;

                var target = hitKey + off;
                int safety = 0;
                while (!IsCellFree(target, baseCenterY, ignore) && safety++ < 256)
                    target += off;

                snappedPos = FromKeyToWorld(target, baseCenterY);
                return true;
            }

            // 2) Build platform
            if (hit.collider.CompareTag(buildPlatformTag))
            {
                float baseCenterY = hit.collider.bounds.max.y + grid.cellSize.y * 0.5f;

                var key = CellXZAt(hit.point);
                key.y = 0;

                int safety = 0;
                while (!IsCellFree(key, baseCenterY, ignore) && safety++ < 256)
                    key.y += 1;

                snappedPos = FromKeyToWorld(key, baseCenterY);
                return true;
            }

            // Otherwise: ignore this hit and keep scanning
        }

        return false;
    }

    public Vector3 GetPlaceablePos(Transform ignore = null)
    {
        if (TryGetSnappedPos(out var pos, ignore)) return pos;
        var cam = Camera.main;
        return cam ? cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f))
                   : Vector3.zero;
    }

    public void InitializeWithObject(GameObject prefab)
    {
        if (!prefab) return;
        var pos = GetPlaceablePos();
        var obj = Instantiate(prefab, pos, GridRotation);
        obj.tag = "Cube";
        if (!obj.GetComponent<ObjectDrag>()) obj.AddComponent<ObjectDrag>();
    }

    public void RegisterPlaced(GameObject go)
    {
        if (!go) return;

        var baseCenterY = PlatformTopAt(go.transform.position) + grid.cellSize.y * 0.5f;
        var key = CellXZAt(go.transform.position);
        key.y = Mathf.RoundToInt((go.transform.position.y - baseCenterY) / grid.cellSize.y);

        int safety = 0;
        while (!IsCellFree(key, baseCenterY, go.transform) && safety++ < 256)
            key.y += 1;

        go.transform.position = FromKeyToWorld(key, baseCenterY);
        AlignToGrid(go.transform);
        placed[key] = go;
    }

    public void Unregister(GameObject go)
    {
        if (!go) return;
        var key = GetKeyFor(go);
        if (placed.TryGetValue(key, out var g) && g == go)
            placed.Remove(key);
    }
}
