using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 origin;
    public GameObject fire;
    public float moveForce = 10f;
    private Rigidbody rb;
    private bool isCo2 = false;
    private Renderer rend;
    private Color baseColor;

    void Start()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        if (!fire || isCo2) return;
        Vector3 direction = (fire.transform.position - transform.position).normalized;
        rb.AddForce(direction * moveForce);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, origin) > 10) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            isCo2 = true;
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            Color co2Color = new Color(1f, 0.5f, 0f, 0.5f); // orange with 50% alpha
            rend.material.color = co2Color;
        }
    }
}