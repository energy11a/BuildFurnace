using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu( menuName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public List<CubeEntry> buildingBlocks;
        public string SceneName;
        public string LevelName;
        public float winTemperature;
        public bool IsRaining;
        public Vector3 FurnacePos;
        public int reward;
    }
}
[System.Serializable]
public class CubeEntry{
    public GameObject cubePrefab;
    public int count;
}
