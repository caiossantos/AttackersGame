using System.Collections;
using UnityEngine;

public class NodeEsquivaOponente : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;

        Print();

        GameObject target = null;
        GameObject[] oponents = GameObject.FindGameObjectsWithTag("NPC");
        float distance = float.MaxValue;

        foreach (GameObject oponent in oponents)
        {
            if (oponent != root.gameObject)
            {
                if (Vector3.Distance(root.transform.position, oponent.transform.position) < distance)
                {
                    target = oponent;
                    distance = Vector3.Distance(root.transform.position, oponent.transform.position);
                }
            }
        }

        if (target)
        {
            float time = Random.Range(.8f, 1.6f);   //tempo do strafe
            float sign = Mathf.Sign(Random.Range(-10, 10));  //direção do strafe (esq = -1; dir = 1)

            while (time > 0)
            {
                time -= Time.deltaTime;

                if (!target) break; //se o alvo sumir enquanto strafe

                root.transform.LookAt(target.transform.position);
                root.transform.Translate(Vector3.right * sign * Time.deltaTime * 4f);

                yield return null;
            }
        }

        if (!target) status = Status.FAILURE;
        else status = Status.SUCCESS;

        Print();
    }
}