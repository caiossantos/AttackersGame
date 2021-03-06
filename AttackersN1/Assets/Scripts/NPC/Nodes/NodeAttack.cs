using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NodeAttack : BTNode
{
    private string _enemyTag;
    private float _attackRange;
    private float _expandAttackRange;
    private bool hasAttacked;

    public NodeAttack(string enemyTag, float attackRange, float expandAttackRange = 0f)
    {
        _enemyTag = enemyTag;
        _attackRange = attackRange;
        _expandAttackRange = expandAttackRange;
    }

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.SUCCESS;

        Enemy enemy = root.GetComponent<Enemy>();

        Collider[] colliders = Physics.OverlapSphere(enemy.Muzzle.position, _attackRange + _expandAttackRange);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != root.gameObject && collider.CompareTag(_enemyTag))
            {
                if (collider.GetComponent<IDamage>() != null)
                {
                    hasAttacked = true;
                    break;
                }
                else
                    hasAttacked = false;
            }
        }

        if (!hasAttacked)
        {
            status = Status.FAILURE;
            yield break;
        }

        enemy?.Attack(_enemyTag);

        yield return null;
    }
}