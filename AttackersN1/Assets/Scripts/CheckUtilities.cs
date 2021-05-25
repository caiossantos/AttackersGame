using UnityEngine;

public static class CheckUtilities
{
    public static bool IsObjectWithTagInRange(Vector3 myPosition, float range, string tag)
    {
        GameObject[] objectsFound = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject objectFound in objectsFound)
        {
            if (Vector3.Distance(myPosition, objectFound.transform.position) <= range)
                return true;
        }

        return false;
    }

    public static GameObject GetTheNearestGameObjectWithTag(Vector3 myPosition, string tag)
    {
        GameObject[] objectsFound = GameObject.FindGameObjectsWithTag(tag);

        GameObject nearestGameObject = null;
        float distance = 0f;
        float distance2 = float.MaxValue;

        foreach (GameObject objectFound in objectsFound)
        {
            distance = Vector3.Distance(myPosition, objectFound.transform.position);
            if (distance < distance2)
            {
                nearestGameObject = objectFound;
                distance2 = distance;
            }
        }

        return nearestGameObject;
    }
}