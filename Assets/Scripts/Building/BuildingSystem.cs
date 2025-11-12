using System;
using UnityEngine;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] float cellWidth = 5;
        [SerializeField] float cellHeight = 5;

        [SerializeField] bool visualiseGrid;
        [SerializeField] int distanceFromPlayer = 5; 
        [SerializeField] private float gizmoSize = 0.2f;

        public static BuildingSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else throw new Exception("There can only be one instance of BuildingSystem2");
        }
        private void OnDrawGizmos()
        {
            if (!visualiseGrid || !Application.isPlaying) return;
            Gizmos.color = Color.white;
            for (float x = -distanceFromPlayer; x <= distanceFromPlayer; x += cellWidth)
            {
                for (float y = -distanceFromPlayer; y <= distanceFromPlayer; y += cellHeight)
                {
                    for (float z = -distanceFromPlayer; z <= distanceFromPlayer; z += cellWidth)
                    {
                        Vector3 pos = GetNearestGridPosition(transform.position) + new Vector3(x,y,z);
                        Gizmos.DrawCube(pos, Vector3.one * gizmoSize);
                    }
                }
            }
            
        }

        public Vector3 GetNearestGridPosition(Vector3 position)
        {
            float x = Mathf.Round(position.x / cellWidth) * cellWidth;
            float y = Mathf.Round(position.y / cellWidth) * cellHeight;
            float z = Mathf.Round(position.z / cellWidth) * cellWidth;
            return new Vector3(x,y,z);
        }
    }
}