//See on chatgpt slop kood, kiire oli, võid vabalt üle kirjutada ja kustutada
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectDrag : MonoBehaviour
{
    public float speed = 24f;
    public bool keepUprightOnDrop = true;

    private Outline outline;
    private Rigidbody rb;
    private bool lastSnapped;

    private RigidbodyConstraints _prevConstraints;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline) outline.enabled = false;

        rb = GetComponent<Rigidbody>(); // optional at start; created on demand
    }

    void OnMouseDown()
    {
        if (outline) outline.enabled = true;

        // Remove from grid occupancy
        BuildingSystem.Instance.Unregister(gameObject);

        // Ensure a rigidbody exists
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();

        // Prep for drag
        _prevConstraints = rb.constraints;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Force correct grid alignment when picked up (clears physics tilt)
        BuildingSystem.Instance.AlignToGrid(transform);
    }

    void OnMouseDrag()
    {
        // Try snap first
        if (BuildingSystem.Instance.TryGetSnappedPos(out var snapped, transform))
        {
            lastSnapped = true;
            transform.position = Vector3.Lerp(transform.position, snapped, Time.deltaTime * speed);
            return;
        }

        // Not snappable: follow raw cursor ray (ignoring our own colliders)
        lastSnapped = false;
        var cam = Camera.main;
        if (!cam) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, 1000f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                          .OrderBy(h => h.distance);

        foreach (var h in hits)
        {
            if (h.collider && h.collider.transform.root == transform.root) continue; // skip self
            transform.position = Vector3.Lerp(transform.position, h.point, Time.deltaTime * speed);
            return;
        }

        // Fallback: fixed depth in front of camera
        var p = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = Vector3.Lerp(transform.position, p, Time.deltaTime * speed);
    }

    void OnMouseUp()
    {
        if (outline) outline.enabled = false;

        if (lastSnapped)
        {
            // Final snap + register into the grid; remain kinematic
            BuildingSystem.Instance.RegisterPlaced(gameObject);
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = _prevConstraints;
        }
        else
        {
            // Drop with physics
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.constraints = keepUprightOnDrop ? RigidbodyConstraints.FreezeRotation : _prevConstraints;
        }
    }
}
