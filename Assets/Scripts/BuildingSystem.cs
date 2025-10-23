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
                position = gridLayout.CellToWorld(cellPosition) + gridLayout.cellSize / 2f;
                Debug.Log(position.ToString());
            }
            else
            {
                position = hit.point;
            }
        }
        position.y = transform.position.y+ gridLayout.cellSize.y / 2f;
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