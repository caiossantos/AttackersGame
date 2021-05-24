using UnityEngine;
using UnityEngine.UI;

public class UiLifeEnemy : MonoBehaviour
{
    private Transform _parent;
    private Slider _slider;

    private void Awake() => _slider = GetComponentInChildren<Slider>();

    private void Start() => transform.SetParent(GameObject.FindGameObjectWithTag("WorldCanvas").transform);

    void Update() => transform.position = new Vector3(_parent.position.x, _parent.position.y + 2f, _parent.position.z);

    public void SetValues(Transform parent, float sliderMaxValue)
    {
        _parent = parent;
        _slider.maxValue = sliderMaxValue;
        _slider.value = sliderMaxValue;
    }

    public void ChangeValue(float newValue)
    {
        if (newValue < 0) 
            _slider.value = 0;
        else
            _slider.value = newValue;
    }

    public void Destroy() => Destroy(gameObject);
}