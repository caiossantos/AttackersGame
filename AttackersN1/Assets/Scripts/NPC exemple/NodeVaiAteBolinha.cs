using System.Collections;
using UnityEngine;

public class NodeVaiAteBolinha : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;

        GameObject target = null;
        float distance = float.MaxValue;
        
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        
        foreach (GameObject collectable  in collectables)
        {
            float distance2 = Vector3.Distance(collectable.transform.position, root.transform.position);

            if (distance2 < distance)
            {
                target = collectable;
                distance = distance2;
            }
        }

        if (target)
        {
            while (Vector3.Distance(target.transform.position, root.transform.position) > 1f)
            {
                root.transform.LookAt(target.transform);
                root.transform.Translate(Vector3.forward * 4f * Time.deltaTime);
                
                yield return null;

                if (!target) break; //caso o alvo desapareça antes
                if (Vector3.Distance(target.transform.position, root.transform.position) < 1f) break; //caso o alvo desapareça antes
            }
            status = Status.SUCCESS;
        }

        if (status == Status.RUNNING) status = Status.FAILURE;

        Print();
    }
}