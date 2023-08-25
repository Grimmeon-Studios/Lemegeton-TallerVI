using UnityEngine;
[CreateAssetMenu(fileName = "attack", menuName = "ScriptableObjects/Item/Attack", order = 2)]
public class Attack: ScriptableObject
{
    public string prefabName = "attack";
    public float porcentaje = 0.3f;

}
