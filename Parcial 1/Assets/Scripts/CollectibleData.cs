using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Collectibles/Collectible Data")]
public class CollectibleData : ScriptableObject
{
    [Header("Configuración del Collectible")]
    public string collectibleName = "Moneda";
    public int value = 1;
    public Color color = Color.yellow;
    public AudioClip collectSound;
}

