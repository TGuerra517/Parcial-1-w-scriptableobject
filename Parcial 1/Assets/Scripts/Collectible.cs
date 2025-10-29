using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleData data; // referencia al Scriptable Object

    private void Start()
    {
        // Cambia el color del objeto según el ScriptableObject
        GetComponent<Renderer>().material.color = data.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        Debug.Log($"[COIN] {data.collectibleName} recolectada (+{data.value})");

        // (Opcional) reproducir sonido si hay
        if (data.collectSound != null)
        {
            AudioSource.PlayClipAtPoint(data.collectSound, transform.position);
        }

        Destroy(gameObject);
    }
}
