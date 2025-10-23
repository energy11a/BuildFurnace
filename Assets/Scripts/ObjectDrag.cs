using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 _offset;
    private void OnMouseDrag()
    {
        transform.position = BuildingSystem.Instance.GetPlaceablePos();
    }
}