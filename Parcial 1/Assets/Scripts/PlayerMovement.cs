using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;             // velocidad normal
    public float sprintMultiplier = 1.8f;    // cuánto más rápido corre al sprintar
    public float rotationSmoothTime = 0.12f;

    [Header("Stamina")]
    //public float staminaDrainRate = 1f; // cuánta stamina gasta por segundo

    CharacterController controller;
    Transform cam;
    float rotationVelocity;
    PlayerStats stats;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null) cam = Camera.main.transform;
        else Debug.LogWarning("PlayerMovement: No se encontró cámara con Tag MainCamera.");

        stats = GetComponent<PlayerStats>();
        if (stats == null)
            Debug.LogWarning("PlayerMovement: No se encontró PlayerStats en el Player.");
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        bool wantsSprint = Input.GetKey(KeyCode.LeftShift) && inputDir.magnitude > 0.1f;
        bool canSprint = stats != null && stats.CanSprint(); // ✅ usa lógica nueva
        bool isSprinting = wantsSprint && canSprint;


        if (inputDir.magnitude >= 0.1f)
        {
            // Rotación relativa a cámara
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            if (cam != null) targetAngle += cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Ajustar velocidad (normal o sprint)
            float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            //// Si sprinta, gastar stamina
            //if (isSprinting && stats != null)
            //{
            //    stats.UseStamina(Mathf.CeilToInt(staminaDrainRate * Time.deltaTime));
            //}
        }
    }
}
