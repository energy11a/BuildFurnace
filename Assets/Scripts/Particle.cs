using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector3 origin;
    public GameObject fire;
    public float moveForce = 10f;
    private Rigidbody rb;
    private bool isCo2 = false;

    void Start()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
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
        }
    }
}