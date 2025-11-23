using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsoCameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 14f;
    public float pitch = 30f;
    public float switchDuration = 0.35f;

    // camera controls
    private InputAction camLeft;
    private InputAction camRight;

    //Audio
    private AudioSource cameraSwish;

    float baseYaw = -45f;
    int index = 0;
    float startYaw, targetYaw, yaw, t;

    void Awake()
    {
        yaw = startYaw = targetYaw = baseYaw;
    }

    void Start()
    {
        // Gotta find the actions first
        camLeft = InputSystem.actions.FindAction("Camera_left");
        camRight = InputSystem.actions.FindAction("Camera_right");

        camLeft.performed += OnCamLeft;
        camRight.performed += OnCamRight;


        cameraSwish = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        camLeft.performed -= OnCamLeft;
        camRight.performed -= OnCamRight;
    }

    void OnCamLeft(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            BeginTurn(1);
        }
    }

    void OnCamRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BeginTurn(-1);
        }
    }

    void LateUpdate()
    {
        if (!target) return;
        if (t < 1f)
        {
            t += Time.deltaTime / switchDuration;
            float u = t * t * (3f - 2f * t);
            yaw = Mathf.LerpAngle(startYaw, targetYaw, u);
            if (t >= 1f) yaw = targetYaw;
        }
        var r = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position - r * Vector3.forward * distance;
        transform.rotation = r;
    }

    void BeginTurn(int step)
    {
        cameraSwish.Play();
        index = (index + step % 4 + 4) % 4;
        startYaw = yaw;
        targetYaw = baseYaw + 90f * index;
        t = 0f;
    }
}
