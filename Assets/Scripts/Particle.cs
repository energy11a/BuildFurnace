using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameObject fire;
    public float moveForce = 10f;
    private Rigidbody rb;
    private bool _isCo2 = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   void FixedUpdate()
    {
        if (!fire) return;

        Vector3 direction = (fire.transform.position - transform.position).normalized;
        rb.AddForce(direction * moveForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            _isCo2 = true;
            rb.AddForce(Vector3.up * 100, ForceMode.Impulse);
        }
    }
}
