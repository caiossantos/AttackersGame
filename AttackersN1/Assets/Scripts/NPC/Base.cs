using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Enemy
{
    private void Start() => EnemyInit();

    public override void Damage(float damage) 
    {
        LifeStatus.RemoveLife(damage);
        UILife.ChangeValue(LifeStatus.CurrentLife);
        DeathCheck();
    }

    protected override void DeathCheck() 
    {
        if (LifeStatus.IsDead())
        {
            UILife.Destroy();
            Destroy(gameObject);
        }
    }

    public override void BehaviourTree() { }
    public override void Attack(string tag) { }
}
