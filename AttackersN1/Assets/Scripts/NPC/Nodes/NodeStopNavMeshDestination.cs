using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NodeStopNavMeshDestination : BTNode
{
    private NavMeshAgent _navMeshAgent;

    public NodeStopNavMeshDestination(NavMeshAgent navMeshAgent) => _navMeshAgent = navMeshAgent;

    public override IEnumerator Run(BTRoot root)
    {
        yield return new WaitForSeconds(.4f);

        status = Status.FAILURE;

        if (_navMeshAgent.hasPath)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = Vector3.zero;
        }

        if (_navMeshAgent.velocity == Vector3.zero)
            status = Status.SUCCESS;

        yield break;
    }
}