using System.Collections;

class BTInverter : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        foreach (BTNode node in children)
        {
            yield return root.StartCoroutine(node.Run(root));

            if (node.status == Status.FAILURE)
            {
                status = Status.SUCCESS;
                yield break;
            } 
            else if (node.status == Status.SUCCESS)
            {
                status = Status.FAILURE;
                yield break;
            }
        }

        status = Status.RUNNING;

        yield break;
    }
}