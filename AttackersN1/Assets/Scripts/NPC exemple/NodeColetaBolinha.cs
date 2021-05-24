using System.Collections;
using UnityEngine;

public class NodeColetaBolinha : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (GameObject collectable in collectables)
        {
            if (Vector3.Distance(collectable.transform.position, root.transform.position) < 1.01)
            {
                GameObject.Destroy(collectable);
                status = Status.SUCCESS;
            }
        }

        Print();

        yield break;
    }
}