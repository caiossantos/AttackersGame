using System.Collections;
using UnityEngine;

public class NodeCombateOponente : BTNode
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
            yield return new WaitForSeconds(.5f);   //aguarda um tempo antes de atirar

            root.transform.LookAt(target.transform);

            Vector3 projectilePosition = root.transform.position + root.transform.forward;
            GameObject projectile = root.GetComponent<NPC>().projectile;
            GameObject projectileRef = GameObject.Instantiate(projectile, projectilePosition, Quaternion.identity);

            projectileRef.GetComponent<Rigidbody>().AddForce(root.transform.forward * 200f);

            status = Status.SUCCESS;
        }
        else
            status = Status.FAILURE;

        Print();

    }
}
