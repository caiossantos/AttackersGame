using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int life;
    public int attack;
    public float attackSpeed;
    public float enemyDetectionRange;
    public float enemyAttackRange;
    public GameObject prefab;
}