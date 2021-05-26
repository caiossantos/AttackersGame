using System.Collections;
using UnityEngine.AI;

public class NodeIsMoving : BTNode
{
    private NavMeshAgent _navMeshAgent;
    public NodeIsMoving(NavMeshAgent navMeshAgent) => _navMeshAgent = navMeshAgent;

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        if (_navMeshAgent.velocity.z != 0 || _navMeshAgent.velocity.x != 0)
            status = Status.SUCCESS;

        yield break;
    }
}