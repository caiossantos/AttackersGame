using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const float BULLET_SPEED = 10f;
    private float _damage;

    private void Awake() => Destroy(gameObject, 7f);

    private void Update() => transform.Translate(Vector3.forward * BULLET_SPEED * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("AllyObjective"))
        {
            other.gameObject.GetComponent<IDamage>()?.Damage(_damage);
            Destroy(gameObject);
        }
    }

    public void SetArrowDamage(float damage) => _damage = damage;

}