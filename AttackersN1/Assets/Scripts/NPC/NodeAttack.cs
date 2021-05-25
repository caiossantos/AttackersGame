using System.Collections;

public class NodeAttack : BTNode
{
    public override IEnumerator Run(BTRoot root)
    {
        status = Status.RUNNING;

        Enemy enemy = root.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.Attack();
            status = Status.SUCCESS;
        }
        else
            status = Status.FAILURE;

        yield break;
    }
}
