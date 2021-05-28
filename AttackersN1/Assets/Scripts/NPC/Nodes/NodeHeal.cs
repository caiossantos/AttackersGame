using System.Collections;
using UnityEngine;

public class NodeHeal : BTNode
{
    private string _tag;
    private float _detectionArea;

    public NodeHeal(string tag, float detectionArea)
    {
        _tag = tag;
        _detectionArea = detectionArea;
    }

    public override IEnumerator Run(BTRoot root)
    {
        Collider[] colliders = Physics.OverlapSphere(root.transform.position, _detectionArea);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != root.gameObject)
            {
                var enemy = collider.GetComponent<Enemy>();
                if (collider.CompareTag(_tag))
                {
                    //if (enemy.LifeStatus.CurrentLife < (enemy.Card.life - (enemy.Card.life * 0.8)))
                        status = Status.SUCCESS;
                    yield break;
                }
            }
        }

        status = Status.FAILURE;

        yield break;
    }
}