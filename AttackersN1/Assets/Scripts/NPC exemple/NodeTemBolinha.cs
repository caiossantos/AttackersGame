using System.Collections;
using UnityEngine;

public class NodeTemBolinha : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        if (GameObject.FindGameObjectWithTag("Collectable"))
            status = Status.SUCCESS;

        Print();

        yield break;
    }
}
