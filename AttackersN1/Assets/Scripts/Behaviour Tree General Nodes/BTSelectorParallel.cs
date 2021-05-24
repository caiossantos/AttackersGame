using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelectorParallel : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;

        Print();

        //Manter uma referência para as Coroutines que estarão rodando
        Dictionary<BTNode, Coroutine> routines = new Dictionary<BTNode, Coroutine>();

        foreach (BTNode node in children)
        {
            //Inicia a Coroutine e armazena uma referência no Dictionary
            routines.Add(node, root.StartCoroutine(node.Run(root)));    
        }

        while (true)
        {
            //Assumimos a falha caso não achemos o contrário
            status = Status.FAILURE;    

            //Verifica o status dos filhos
            foreach (BTNode node in children)
            {
                if (node.status == Status.RUNNING)
                {
                    //Se alguém estiver em Runnig o SelectorParallel continua com status Running
                    status = Status.RUNNING;  
                    break;
                }
                else if (node.status == Status.SUCCESS)
                {
                    //Com isso, se todos os filhos derem Success o SelectorParallel dará Success
                    status = Status.SUCCESS;

                    //Stop todas as Coroutines
                    foreach (var routineNode in routines)
                    {
                        if (routineNode.Value != null)
                            root.StopCoroutine(routineNode.Value);
                    }

                    break;
                }
            }

            //Se não estiver Running sai do while loop
            if (status != Status.RUNNING) break;

            //Para cada node que terminar em Failure, inicia novamente sua coroutine
            foreach (BTNode node in children)
            {
                if (node.status == Status.FAILURE)
                    routines[node] = root.StartCoroutine(node.Run(root));
            }

            //Para que o while não rode todo frame
            yield return new WaitForSeconds(.1f);
        }

        Print();
    }
}