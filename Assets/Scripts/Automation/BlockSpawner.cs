using System;
using System.Collections.Generic;
using UnityEngine;
namespace Automation
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] float cellSize;
        [SerializeField] bool visualiseGrid;
        [SerializeField] int distanceAlongX;
        [SerializeField] int distanceAlongZ;
        [SerializeField] float gizmoSize;
        [SerializeField] float offset;
        private Vector3 freePos;
        private Vector3 gridStartPos;
        [SerializeField] GameObject blockPrefab;

        public static BlockSpawner Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Events events = Events.Instance;

            freePos = new Vector3(-distanceAlongX, -distanceAlongX, distanceAlongZ);
            gridStartPos = GetNearestGridPosition(transform.position);

            SpawnBlocks(events.levels[events.currentLevel].buildingBlocks);
        }

        private void OnDrawGizmos()
        {
            if (!visualiseGrid || !Application.isPlaying) return;
            Gizmos.color = Color.white;
            for (float x = -distanceAlongX; x <= distanceAlongX; x += cellSize)
            {
                for (float y = -distanceAlongX; y <= distanceAlongX; y += cellSize)
                {
                    for (float z = -distanceAlongZ; z <= distanceAlongZ; z += cellSize)
                    {
                        Vector3 pos = GetNearestGridPosition(transform.position) + new Vector3(x,y,z);
                        Gizmos.DrawCube(pos, Vector3.one * gizmoSize);
                    }
                }
            }
        }

        private void SpawnBlock(BlockData block){
            Vector3 pos = gridStartPos + freePos + new Vector3(UnityEngine.Random.Range(0,offset),0,UnityEngine.Random.Range(0,offset));
            GameObject spawned = Instantiate(blockPrefab, pos, Quaternion.identity);

            spawned.GetComponent<MeshFilter>().mesh = block.mesh;
            spawned.GetComponent<Renderer>().material = block.material;

            if (freePos.x + cellSize >= distanceAlongX){
                freePos.x = -distanceAlongX;
                if (freePos.z + cellSize >= distanceAlongZ){
                    freePos.z = -distanceAlongZ;
                    freePos.y += distanceAlongX;
                }
                else{
                    freePos.z += cellSize;
                }
            }
            else{
                freePos.x += cellSize;
            }
        }

        public Vector3 GetNearestGridPosition(Vector3 position)
        {
            float x = Mathf.Round(position.x / cellSize) * cellSize;
            float y = Mathf.Round(position.y / cellSize) * cellSize + 0.5f;
            float z = Mathf.Round(position.z / cellSize) * cellSize;
            return new Vector3(x,y,z);
        }

        public void SpawnBlocks(List<CubeEntry> buildingBlocks)
        {
            Debug.Log("Spawning Blocks " + buildingBlocks.Count);

            foreach (CubeEntry entry in buildingBlocks){
                if (!entry.BlockData.bought) continue;
                for (int i = 0; i < entry.count; i++){
                    SpawnBlock(entry.BlockData);
                }
            }
        }
    }
}
