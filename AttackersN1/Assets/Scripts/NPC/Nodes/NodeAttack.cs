using System.Collections;
using UnityEngine;

public class NodeAttack : BTNode
{
    private string _enemyTag;
    private float _attackRange;
    private bool hasAttacked;

    public NodeAttack(string enemyTag, float attackRange)
    {
        _enemyTag = enemyTag;
        _attackRange = attackRange;
    }

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.SUCCESS;

        Enemy enemy = root.GetComponent<Enemy>();

        Collider[] colliders = Physics.OverlapSphere(enemy.Muzzle.position, _attackRange);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != root.gameObject)
            {
                if (collider.GetComponent<Enemy>() != null)
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