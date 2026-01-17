using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 origin;
    public GameObject fire;
    public float moveForce = 10f;
    Rigidbody rb;
    Renderer rend;
    bool isCo2;

    void OnEnable()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (GameManager.Instance) GameManager.Instance.OnSimulationEnd += OnSimEnd;
    }

    void OnDisable()
    {
        if (GameManager.Instance) GameManager.Instance.OnSimulationEnd -= OnSimEnd;
    }

    void OnDestroy()
    {
        if (GameManager.Instance) GameManager.Instance.OnSimulationEnd -= OnSimEnd;
    }

    void OnSimEnd()
    {
        if (this) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!fire || isCo2) return;
        var dir = (fire.transform.position - transform.position).normalized;
        rb.AddForce(dir * moveForce);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, origin) > 10f) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fire")) return;
        isCo2 = true;
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        rend.material.color = new Color(1f, 0.5f, 0f, 0.5f);
    }
}