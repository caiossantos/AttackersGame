using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] private GameObject _lifeUI;
    [SerializeField] private Card _card;

    protected Card Card { get { return _card; } }
    protected LifeSystem LifeStatus { get; private set; }

    protected void EnemyInit()
    {
        LifeStatus = new LifeSystem(_card.life);

        //UI Life
        var newUI = Instantiate(_lifeUI, Vector3.zero, Quaternion.Euler(-45f, 90f, 0f));
        UiLifeEnemy uiLifeEnemy = newUI.GetComponent<UiLifeEnemy>();
        uiLifeEnemy.SetValues(transform, _card.life);
    }

    public abstract void BehaviourTree();

    public abstract void Damage(float damage);

    protected abstract void DeathCheck(bool batAttack);

    public abstract void Attack();
}