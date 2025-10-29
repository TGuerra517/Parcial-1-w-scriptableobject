using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Configuraci�n del arma")]
    [SerializeField] private int damage = 20;           // da�o por disparo
    [SerializeField] private float range = 50f;         // alcance m�ximo
    [SerializeField] private float fireRate = 0.5f;     // tiempo entre disparos (cadencia)

    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;       // c�mara desde donde disparamos
    [SerializeField] private LineRenderer lineRenderer; // referencia al Line Renderer
    [SerializeField] private LayerMask enemyLayer; // aparece en el inspector

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Debug.Log("PUM! Disparo realizado");

        RaycastHit hit;
        Vector3 shotOrigin = playerCamera.transform.position;
        Vector3 shotDirection = playerCamera.transform.forward;
        Vector3 hitPoint;

        Debug.DrawRay(shotOrigin, shotDirection * range, Color.red, 1f);

        if (Physics.Raycast(shotOrigin, shotDirection, out hit, range, enemyLayer))
        {
            Debug.Log("Impact� contra: " + hit.transform.name);

            hitPoint = hit.point; // el rayo termina donde peg�

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("El disparo no golpe� a un enemigo");
            hitPoint = shotOrigin + shotDirection * range; // si no pega nada, llega hasta el rango m�ximo
        }



        //// Mostrar el rayo visual
        if (lineRenderer != null)
        {
            StartCoroutine(ShowShotEffect(shotOrigin, hitPoint));

        }
    }

    IEnumerator ShowShotEffect(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        yield return new WaitForSeconds(0.05f); // el rayo dura 0.05 segundos

        lineRenderer.enabled = false;
    }
}
