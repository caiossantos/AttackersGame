using System.Collections;

public class NodeEnemyNear : BTNode
{
    private string _tag;
    private float _detectionArea;

    public NodeEnemyNear(string tag, float detectionArea)
    {
        _tag = tag;
        _detectionArea = detectionArea;
    }

    public override IEnumerator Run(BTRoot root)
    {
        status = Status.FAILURE;

        if (CheckUtilities.IsObjectWithTagInRange(root.transform.position, _detectionArea, _tag))
            status = Status.SUCCESS;

        yield break;
    }
}
