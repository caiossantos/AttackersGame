using System.Collections;
using UnityEngine;

public class NodeVeOponente : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        GameObject[] oponents = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject oponent in oponents)
        {
            if (oponent != root.gameObject)
            {
                if (Vector3.Distance(root.transform.position, oponent.transform.position) < 5f)
                {
                    status = Status.SUCCESS;
                    break;
                }
            }
        }

        Print();

        yield break;
    }
}