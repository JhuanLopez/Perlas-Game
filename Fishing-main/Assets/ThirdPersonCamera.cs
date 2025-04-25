using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;           // Player target
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float followSpeed = 10f;
    public float rotateSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    private float currentZoom = 5f;
    private float yaw = 0f;
    private float pitch = 15f;

    void Update()
    {
        // Mouse input for rotation
        yaw += Input.GetAxis("Mouse X") * rotateSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;
        pitch = Mathf.Clamp(pitch, -35f, 60f); // Clamp pitch for usability

        // Zoom input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Smooth follow
        Vector3 desiredPosition = target.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Calculate camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 cameraOffset = rotation * new Vector3(0, 0, -currentZoom);
        Camera.main.transform.position = transform.position + offset + cameraOffset;
        Camera.main.transform.LookAt(transform.position + offset);
    }
}