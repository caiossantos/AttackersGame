using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;
        
        //Print();

        foreach (BTNode node in children)
        {
            yield return root.StartCoroutine(node.Run(root));

            if (node.status == Status.SUCCESS)
            {
                status = Status.SUCCESS;
                break;
            }
        }

        if (status == Status.RUNNING) status = Status.FAILURE;

        //Print();
    }
}
