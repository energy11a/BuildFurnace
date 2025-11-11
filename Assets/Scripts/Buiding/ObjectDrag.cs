//See on chatgpt slop kood, kiire oli, võid vabalt üle kirjutada ja kustutada
// ObjectDrag.cs

using System;
using System.Collections.Generic;
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
    
    //Sounds
    public AudioSource placeSound;
    public AudioSource grassSound;

    // Layer juggling to ignore self while dragging
    private readonly List<Transform> _layered = new List<Transform>();
    private readonly List<int> _origLayers = new List<int>();
    private int _ignoreRaycastLayer;
    private RigidbodyConstraints _prevConstraints;

    void Awake()
    {
        _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    void OnMouseDown()
    {
        if (outline) outline.enabled = true;

        BuildingSystem.Instance.Unregister(gameObject);

        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
        _prevConstraints = rb.constraints;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Align immediately on pickup (clear physics tilt)
        BuildingSystem.Instance.AlignToGrid(transform);

        // Hide from raycasts
        _layered.Clear(); _origLayers.Clear();
        foreach (var t in GetComponentsInChildren<Transform>(true))
        {
            _layered.Add(t);
            _origLayers.Add(t.gameObject.layer);
            if (_ignoreRaycastLayer >= 0) t.gameObject.layer = _ignoreRaycastLayer;
        }
    }

    void OnMouseDrag()
    {
        if (BuildingSystem.Instance.TryGetSnappedPos(out var snapped, transform))
        {
            lastSnapped = true;
            transform.position = Vector3.Lerp(transform.position, snapped, Time.deltaTime * speed);
            return;
        }

        lastSnapped = false;

        // Follow first non-self hit (for preview when not snappable)
        var cam = Camera.main;
        if (!cam) return;

        var hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition), 1000f,
                                      Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)
                          .OrderBy(h => h.distance);

        foreach (var h in hits)
        {
            if (h.collider && h.collider.transform.root == transform.root) continue;
            transform.position = Vector3.Lerp(transform.position, h.point, Time.deltaTime * speed);
            return;
        }

        // Fallback: fixed depth
        var p = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = Vector3.Lerp(transform.position, p, Time.deltaTime * speed);
    }

    void OnMouseUp()
    {
        if (outline) outline.enabled = false;

        // Restore layers
        for (int i = 0; i < _layered.Count; i++)
            if (_layered[i]) _layered[i].gameObject.layer = _origLayers[i];
        _layered.Clear(); _origLayers.Clear();

        if (lastSnapped)
        {
            BuildingSystem.Instance.RegisterPlaced(gameObject);
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.constraints = _prevConstraints;
            placeSound.Play();
        }
        else
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.constraints = keepUprightOnDrop ? RigidbodyConstraints.FreezeRotation : _prevConstraints;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            grassSound.Play();
        }
    }
}
