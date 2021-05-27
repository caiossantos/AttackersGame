using UnityEngine;
using TMPro;

public class UiTxtLifeEarned : MonoBehaviour
{
    private const float UP_SPEED = 3f;

    void Awake() => Destroy(gameObject, .7f);

    private void Start() => transform.SetParent(GameObject.FindGameObjectWithTag("WorldCanvas").transform);

    void Update() => transform.Translate(Vector3.up * UP_SPEED * Time.deltaTime);

    public void ChangeText(float lifeAdded) => GetComponent<TMP_Text>().SetText($"+{lifeAdded}");

}
