using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequenceParallelWithPriority : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        //Realizar a sequencia em paralelo até que alguém entre em sucesso ou fique em running
        status = Status.FAILURE;

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
                else if (node.status == Status.SUCCESS)
                {
                    foreach (var otherNodeRoutine in routines)
                    {
                        if (otherNodeRoutine.Value != null)
                        {
                            //if (otherNodeRoutine.Key == node) continue;

                            root.StopCoroutine(otherNodeRoutine.Value);
                        }
                    }

                    status = Status.SUCCESS;
                    break;
                }
            }

            if (status == Status.RUNNING) break;

            foreach (BTNode node in children)
            {
                if (node.status == Status.FAILURE)
                    routines[node] = root.StartCoroutine(node.Run(root));
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
