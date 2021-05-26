using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequenceParallel : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.SUCCESS;

        //Print();

        Dictionary<BTNode, Coroutine> routines = new Dictionary<BTNode, Coroutine>();

        foreach (BTNode node in children)
        {
            routines.Add(node, root.StartCoroutine(node.Run(root)));
        }

        while (true)
        {
            foreach (BTNode node in children)
            {
                if (node.status == Status.RUNNING)
                {
                    status = Status.RUNNING;
                    break;
                }
                else if (node.status == Status.FAILURE)
                {
                    status = Status.FAILURE;
                        
                    foreach (var routineNode in routines)
                    {
                        if (routineNode.Value != null)
                            root.StopCoroutine(routineNode.Value);
                    }

                    break;
                }
            }

            if (status != Status.RUNNING) break;

            foreach (BTNode node in children)
            {
                if (node.status == Status.SUCCESS)
                    routines[node] = root.StartCoroutine(node.Run(root));
            }

            yield return new WaitForSeconds(.1f);
        }

        //Print();
    }
}