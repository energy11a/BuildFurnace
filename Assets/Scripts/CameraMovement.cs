using UnityEngine;

public class IsoCameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 14f;
    public float pitch = 30f;
    public float switchDuration = 0.35f;
    
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
        cameraSwish = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) BeginTurn(-1);
        if (Input.GetKeyDown(KeyCode.Q)) BeginTurn(1);
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
