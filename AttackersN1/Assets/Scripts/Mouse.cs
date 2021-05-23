using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Camera _camera;

    private void Start() => _camera = Camera.main;

    public bool Aiming(out Vector3 rayHitPosition, LayerMask layerMask)
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
}
