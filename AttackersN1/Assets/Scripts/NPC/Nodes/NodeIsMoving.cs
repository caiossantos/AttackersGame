using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NodeIsMoving : BTNode
{
    private NavMeshAgent _navMeshAgent;
    public NodeIsMoving(NavMeshAgent navMeshAgent) => _navMeshAgent = navMeshAgent;

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        if (_navMeshAgent.velocity != Vector3.zero)
            status = Status.SUCCESS;

        yield break;
    }
}