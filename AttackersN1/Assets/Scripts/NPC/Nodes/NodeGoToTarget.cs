using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NodeGoToTarget : BTNode
{
    private string _tag;
    private NavMeshAgent _navMeshAgent;

    public NodeGoToTarget(string tag, NavMeshAgent navMeshAgent)
    {
        _tag = tag;
        _navMeshAgent = navMeshAgent;
    }

    public override IEnumerator Run(BTRoot root)
    {
        Transform nearestGameObject = null;
        float distance = 0f;
        float distance2 = float.MaxValue;

        Collider[] colliders = Physics.OverlapSphere(root.transform.position, 100f);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != root.gameObject)
            {
                if (collider.CompareTag(_tag))
                {
                    distance = Vector3.Distance(root.transform.position, collider.gameObject.transform.position);

                    if (distance < distance2)
                    {
                        nearestGameObject = collider.gameObject.transform;
                        distance2 = distance;
                    }
                }
            }
        }

        if (nearestGameObject == null)
        {
            status = Status.FAILURE;
            yield break;
        }

        _navMeshAgent.SetDestination(nearestGameObject.position);
        status = Status.SUCCESS;
        
        yield break;
    }
}