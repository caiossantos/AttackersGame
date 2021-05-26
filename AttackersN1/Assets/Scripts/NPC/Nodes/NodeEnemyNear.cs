using System.Collections;
using UnityEngine;

public class NodeEnemyNear : BTNode
{
    private string _tag;
    private float _detectionArea;

    public NodeEnemyNear(string tag, float detectionArea)
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
                if (collider.CompareTag(_tag))
                {
                    status = Status.SUCCESS;
                    yield break;
                }
            }
        }

        status = Status.FAILURE;

        yield break;
    }
}
