using UnityEngine;

public class HoverTransparent : MonoBehaviour
{
    private Material mat;
    private Color originalColor;

    public float hoverAlpha = 0.3f; // kui labipaistev hiire all

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.color;
    }

    void OnMouseEnter()
    {
        Color c = mat.color;
        c.a = hoverAlpha;
        mat.color = c;
    }

    void OnMouseExit()
    {
        mat.color = originalColor;
    }
}
