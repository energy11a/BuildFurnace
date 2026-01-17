using System;
using UnityEngine;

namespace Building
{
    public class ObjectDrag : MonoBehaviour
    {
        public float speed = 1;

        public Collider col;
        private Outline outline;
        private Rigidbody rb;
        private Camera cam;

        private bool placed;

        //Sounds
        public AudioSource placeSound;
        public AudioSource grassSound;


        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cam = Camera.main;
        }

        void Start()
        {
            outline = GetComponent<Outline>();
            if (outline) outline.enabled = false;
        }

        void OnMouseDown()
        {
            if (GameManager.Instance.simulating) return;
            if (outline) outline.enabled = true;
            rb.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        bool IsPointInside(Collider col, Vector3 point)
        {
            Vector3 closest = col.ClosestPoint(point);
            return Vector3.Distance(closest, point) < 0.0001f;
        }


        public Vector3 targetPos;

        void OnMouseDrag()
        {
            if (GameManager.Instance.simulating) return;
            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null) return; 
            }

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            int mask = ~LayerMask.GetMask( "Walls", "Ignore Raycast");
            RaycastHit[] hits = Physics.RaycastAll(ray,Mathf.Infinity, mask);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                if (hit.transform == transform) continue;

                targetPos = hit.point + hit.normal * .5f;
                targetPos = BuildingSystem.Instance.GetNearestGridPosition(targetPos);
                targetPos= new Vector3(targetPos.x, Math.Max(0.5f, targetPos.y), targetPos.z); // so it doesn't clip through the ground
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
                return;
            }
        }


        void OnMouseUp()
        {
            if (GameManager.Instance.simulating) return;
            transform.position = targetPos;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if (outline) outline.enabled = false;
            placeSound.Play();
        }

        private void OnDrawGizmos()
        {
            if (targetPos == Vector3.zero) return;
            Debug.DrawRay(transform.position, transform.position - targetPos, Color.red);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Grass"))
            {
                grassSound.Play();
            }
        }
        void Update()
        {
            if (GameManager.Instance.simulating) return;

            // R key rotates object 90 degrees clockwise around Y
            if (Input.GetKeyDown(KeyCode.R) && outline.enabled)
            {
                float newY = transform.eulerAngles.y + 90f;
                transform.rotation = Quaternion.Euler(0f, newY, 0f);
            }
        }

    }
}