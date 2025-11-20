using System;
using UnityEngine;

namespace Simulation
{
    public class HeatArea : MonoBehaviour
    {
        private void Start()
        {
            
        Vector3 size = GetComponent<BoxCollider>().size;
        Vector3 scale = transform.lossyScale;

        Fire.Instance.heatAreaVolume =
            (size.x * scale.x) *
            (size.y * scale.y) *
            (size.z * scale.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("HeatParticle")) return;
            Fire.Instance.heatParticles.Add(other.GetComponent<HeatParticle>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("HeatParticle")) return;
            Fire.Instance.heatParticles.Remove(other.GetComponent<HeatParticle>());
        }
    }
}