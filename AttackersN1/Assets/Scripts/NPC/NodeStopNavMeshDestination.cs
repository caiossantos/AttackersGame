using System.Collections;
using UnityEngine.AI;

public class NodeStopNavMeshDestination : BTNode
{
    private NavMeshAgent _navMeshAgent;

    public NodeStopNavMeshDestination(NavMeshAgent navMeshAgent) => _navMeshAgent = navMeshAgent;

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        _navMeshAgent.ResetPath();
        _navMeshAgent.isStopped = true;
        _navMeshAgent.isStopped = false;

        if (!_navMeshAgent.hasPath)
            status = Status.SUCCESS;

        yield break;
    }
}