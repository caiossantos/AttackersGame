using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Camera _camera;

    private void Start() => _camera = Camera.main;

    public bool Aiming(LayerMask layerMask, out Vector3 rayHitPosition)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask))
        {
            rayHitPosition = hitInfo.point;
            return true;
        }

        rayHitPosition = Vector3.zero;
        return false;
    }

    public bool Aiming(string colliderTag, out Vector3 rayHitPosition)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue))
        {
            if (hitInfo.collider.CompareTag(colliderTag))
            {
                rayHitPosition = hitInfo.point;
                return true;
            }
        }

        rayHitPosition = Vector3.zero;
        return false;
    }

}
