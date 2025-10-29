using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats del enemigo")]
    [SerializeField] private int health = 10;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float staminaDrainRate = 1f;

    [Header("Movimiento del enemigo")]
    [SerializeField] private float moveSpeed = 2f; // velocidad regulable en el editor

    public enum EnemyState { Normal, Chase, Damage, Dead }
    private EnemyState currentState = EnemyState.Normal;


    private Transform player;
    private PlayerStats playerStats;
    private float staminaDrainTimer = 0f;

    private bool isChasing = false;

    // Evento para avisar al EnemyManager
    public System.Action OnDeath;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            SetState(EnemyState.Chase);
            if (!isChasing)
            {
                Debug.Log("Chase");
                isChasing = true;
            }

            ChasePlayer();
            DrainPlayerStamina();
            playerStats.SetBlockRegen(true); // bloquea regeneración
        }
        else
        {
            SetState(EnemyState.Normal);
            if (isChasing)
            {
                Debug.Log("Normal");
                isChasing = false;
            }

            playerStats.SetBlockRegen(false); // vuelve a regenerar
            staminaDrainTimer = 0f;           // resetea contador
        }
    }


    private void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            Debug.Log("[ENEMY STATE] → " + currentState.ToString());
        }
    }


    private void ChasePlayer()
    {
        if (player == null) return;

        
        // Movimiento hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        // Opcional: hacer que mire hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }


    private void DrainPlayerStamina()
    {
        if (playerStats == null) return;

        staminaDrainTimer += Time.deltaTime;

        if (staminaDrainTimer >= 1f) // cada 1 segundo
        {
            playerStats.ForceDrainStamina(1);
            Debug.Log("[ENEMY] Jugador dentro del rango → pierde 1 de stamina");
            staminaDrainTimer = 0f;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        SetState(EnemyState.Damage); // cambia a “Damage”
        Debug.Log("[ENEMY] Damage, vida restante: " + health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // opcional: volver a Chase si sigue vivo y el jugador está cerca
            if (Vector3.Distance(transform.position, player.position) <= detectionRange)
                SetState(EnemyState.Chase);
            else
                SetState(EnemyState.Normal);
        }
    }

    private void Die()
    {
        SetState(EnemyState.Dead);
        Debug.Log("[ENEMY] Dead");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
