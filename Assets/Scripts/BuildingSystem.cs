using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingSystem: MonoBehaviour
{
    public static BuildingSystem Instance;
    
    public GridLayout gridLayout;
    private Grid  grid;
    [SerializeField] private Tilemap tilemap;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject _objectToPlace;

    // Hoia koik grid positsioonid klotside jaoks
    private Dictionary<Vector3Int, GameObject> placedBlocks = new Dictionary<Vector3Int, GameObject>();

    #region Unity methods

    private void Awake()
    {
        Instance = this;
        grid = gridLayout.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InitializeWithObject(prefab2);
        }
    }
    #endregion
    
    #region Utils

    public Vector3 GetPlaceablePos()
    {
        
        Vector3 position = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("BuildPlatform") || hit.collider.CompareTag("Cube"))
            {
                Vector3Int cellPosition = gridLayout.WorldToCell(hit.point);

                // Arvuta hiire positsioon kohaliku ruumi suhtes
                Vector3 localHit = hit.collider.transform.InverseTransformPoint(hit.point);
                Vector3Int offset = Vector3Int.zero;

                // Snap X/Z kulgedele
                if (Mathf.Abs(localHit.x) > Mathf.Abs(localHit.z))
                    offset.x = (localHit.x > 0) ? 1 : -0;
                else
                    offset.z = (localHit.z > 0) ? 1 : -0;

                // Snap peale Y, kui hiir on klotsi kohal
                if (localHit.y > 0.1f)
                    offset.y = 1;

                Vector3Int newCell = cellPosition + offset;

                // Kui uus lahter juba tais, tosta Y +1
                if (placedBlocks.ContainsKey(newCell))
                    newCell.y += 1;

                // X ja Z suunal liigutame ainult 1 grid vorra, mitte pool cellSize
                Vector3 basePos = gridLayout.CellToWorld(cellPosition);
                position.x = basePos.x + offset.x * gridLayout.cellSize.x;
                position.z = basePos.z + offset.z * gridLayout.cellSize.z;

                // Y positsioon uhe grid sammuga ules (voi pool cellSize, kui alustame pinnast)
                position.y = basePos.y + offset.y * gridLayout.cellSize.y;
            }
            else
            {
                position = hit.point;
            }
        }
        else
        {
            position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        }

        return position;
    }



    #endregion

    #region Building placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = GetPlaceablePos();
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        _objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
    #endregion
}