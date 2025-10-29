using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Vida del Jugador")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Stamina del Jugador")]
    [SerializeField] private int maxStamina = 10;
    private int currentStamina;

    [SerializeField] private float staminaRegenRate = 1f;
    [SerializeField] private float staminaDrainRate = 1f;

    private bool isSprinting = false;
    private float staminaRegenTimer = 0f;
    private float staminaDrainTimer = 0f;

    private bool blockRegen = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleStamina();
    }

    private void HandleStamina()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (currentStamina > 1)
            {
                isSprinting = true;
                staminaDrainTimer += Time.deltaTime;

                if (staminaDrainTimer >= 1f / staminaDrainRate)
                {
                    currentStamina -= 1;
                    staminaDrainTimer = 0f;
                    Debug.Log($"[STAMINA] ↓ Nueva: {currentStamina}/{maxStamina}");
                }
            }
            else
            {
                isSprinting = false;
            }
        }
        else if (!blockRegen)
        {
            isSprinting = false;
            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer >= 1f / staminaRegenRate)
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += 1;
                    Debug.Log($"[STAMINA] ↑ Nueva: {currentStamina}/{maxStamina}");
                }
                staminaRegenTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Debug.Log("[SPRINT] Empezaste a sprintar");
        if (Input.GetKeyUp(KeyCode.LeftShift))
            Debug.Log("[SPRINT] Dejaste de sprintar");
    }

    public void ForceDrainStamina(int amount)
    {
        if (currentStamina > 1)
        {
            currentStamina -= amount;
            if (currentStamina < 1) currentStamina = 1;
            Debug.Log($"[STAMINA] ↓ Forzado por enemigo: {currentStamina}/{maxStamina}");
        }
    }

    public void SetBlockRegen(bool state)
    {
        blockRegen = state;
    }

    public int GetCurrentStamina() => currentStamina;
    public bool CanSprint() => currentStamina > 1;
}
