using UnityEngine;
[CreateAssetMenu(fileName = "SkinSO", menuName = "ScriptableObjects/SkinData")]
public class SkinSO : ScriptableObject
{
    [field: SerializeField]
    public int rarity { get; set; }
    [field: SerializeField]
    public float wear { get; set; }

}
