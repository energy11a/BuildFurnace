using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu( menuName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public List<GameObject> availableCubesPrefabs;
        
    }
}