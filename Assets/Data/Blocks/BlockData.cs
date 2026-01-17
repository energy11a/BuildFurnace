using UnityEngine;

[CreateAssetMenu(menuName = "Block")]
public class BlockData : ScriptableObject
{
    public string alias;
    public Mesh mesh;
    public Material material;
    public Vector3 size = Vector3.one;
    public int cost;
    public bool bought;
}