using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject currentEnemy;

    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
        SpawnEnemy();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (currentEnemy == null)
            {
                SpawnEnemy();
            }
            else
            {
                Debug.Log("Ya existe un enemigo en pantalla");
            }
        }
    }

    private void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        Enemy enemyScript = currentEnemy.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.OnDeath += HandleEnemyDeath;
        }

        Debug.Log("Enemy apareció en spawn inicial");
    }

    private void HandleEnemyDeath()
    {
        currentEnemy = null;
    }
}

