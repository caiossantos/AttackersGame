using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int life;
    public int attackDamage;
    public float attackSpeed;
    public float attackArea;
    public float enemyDetectionRange;
    public float enemyAttackRange;
    public GameObject prefab;
}