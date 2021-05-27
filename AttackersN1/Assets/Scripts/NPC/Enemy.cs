using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] private GameObject _lifeUI;
    [SerializeField] private Card _card;
    [SerializeField] private Transform _muzzle = null;

    public Card Card { get { return _card; } }
    public LifeSystem LifeStatus { get; private set; }
    protected UiLifeEnemy UILife { get; private set; }
    public Transform Muzzle { get { return _muzzle; } }

    protected void EnemyInit()
    {
        LifeStatus = new LifeSystem(_card.life);

        //UI Life
        var newUI = Instantiate(_lifeUI, Vector3.zero, Quaternion.Euler(-45f, 90f, 0f));
        UILife = newUI.GetComponent<UiLifeEnemy>();
        UILife.SetValues(transform, _card.life);
    }

    public abstract void BehaviourTree();

    public abstract void Damage(float damage);

    protected abstract void DeathCheck();

    public abstract void Attack(string tag);
}