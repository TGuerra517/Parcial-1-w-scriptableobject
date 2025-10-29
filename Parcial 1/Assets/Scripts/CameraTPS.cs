using UnityEngine;

public class CameraTPS : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Asignar CameraTarget del Player en el Inspector

    [Header("Distancia y Altura")]
    public float distance = 4f;
    public float height = 1.6f;

    [Header("Rotación")]
    public float sensitivity = 3.5f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Suavizado")]
    public float followSpeed = 10f;

    private float yaw;
    private float pitch;

    void Start()
    {
        if (target == null)
            Debug.LogError("CameraTPS: No se asignó target. Arrastra el CameraTarget del Player aquí.");

        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializar ángulos
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Entrada del mouse
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotación de la cámara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Posición deseada detrás del jugador
        Vector3 desiredPos = target.position + rotation * new Vector3(0, 0, -distance) + Vector3.up * height;

        // Mover suavemente la cámara hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Mirar al target
        transform.LookAt(target.position + Vector3.up * (height * 0.5f));
    }
}
