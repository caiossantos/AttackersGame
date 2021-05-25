using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NodeGoToTarget : BTNode
{
    private string _tag;
    private float _stopDistanceFromTarget;
    private NavMeshAgent _navMeshAgent;

    public NodeGoToTarget(string tag, float stopDistanceFromTarget, NavMeshAgent navMeshAgent)
    {
        _tag = tag;
        _stopDistanceFromTarget = stopDistanceFromTarget;
        _navMeshAgent = navMeshAgent;
    }

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;

        Print();

        GameObject nearestGameObject = CheckUtilities.GetTheNearestGameObjectWithTag(root.transform.position, _tag);

        if (Vector3.Distance(root.transform.position, nearestGameObject.transform.position) > _stopDistanceFromTarget)
        {
            while (Vector3.Distance(root.transform.position, nearestGameObject.transform.position) > _stopDistanceFromTarget)
            {
                if (nearestGameObject == null)
                {
                    status = Status.FAILURE;
                    yield break;
                }
                _navMeshAgent.SetDestination(nearestGameObject.transform.position);
                yield return null;
            }

            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;
            _navMeshAgent.isStopped = false;
            status = Status.SUCCESS;
        }

        if (status == Status.RUNNING) status = Status.FAILURE;

        Print();
    }
}