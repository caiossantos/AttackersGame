using UnityEngine;
using UnityEngine.AI;

public class Warrior : Enemy
{
    private BTRoot behaviour;

    private void Start()
    {
        EnemyInit();
        BehaviourTree();
    }

    public override void BehaviourTree()
    {
        behaviour = GetComponent<BTRoot>();

        //Detect Enemy
        BTSequence enemyDetectionSequence = new BTSequence();
        enemyDetectionSequence.children.Add( new NodeEnemyNear("Enemy", float.MaxValue) );
        enemyDetectionSequence.children.Add( new NodeEnemyNear("Enemy", Card.enemyDetectionRange) );

        //Approach Enemy
        BTSequence enemyAttackRangeSequence = new BTSequence();
        BTInverter attackEnemyInverter = new BTInverter();
        attackEnemyInverter.children.Add(new NodeEnemyNear("Enemy", Card.enemyAttackRange));
        enemyAttackRangeSequence.children.Add(attackEnemyInverter);
        enemyAttackRangeSequence.children.Add( new NodeGoToTarget("Enemy", Card.enemyAttackRange, GetComponent<NavMeshAgent>()) );

        //Enemy Sequence
        BTSequence enemyAttackSequence = new BTSequence();
        enemyAttackSequence.children.Add( enemyDetectionSequence );
        enemyAttackSequence.children.Add( enemyAttackRangeSequence );
        //enemyAttackSequence.children.Add( new NodeStopNavMeshDestination(GetComponent<NavMeshAgent>()) );


        //Go to objective
        BTSequence goToObjectiveSequence = new BTSequence();
        goToObjectiveSequence.children.Add(new NodeEnemyNear("AllyObjective", float.MaxValue));

        BTInverter goToObjectiveInverter = new BTInverter();
        goToObjectiveInverter.children.Add(new NodeEnemyNear("AllyObjective", Card.enemyAttackRange));

        goToObjectiveSequence.children.Add(goToObjectiveInverter);
        goToObjectiveSequence.children.Add(new NodeGoToTarget("AllyObjective", Card.enemyAttackRange, GetComponent<NavMeshAgent>()));

        //Master Selector
        BTSelectorParallel selectorParallel = new BTSelectorParallel();
        selectorParallel.children.Add(enemyAttackSequence);
        selectorParallel.children.Add( goToObjectiveSequence );

        behaviour.root = selectorParallel;
        StartCoroutine(behaviour.Execute());
    }

    public override void Attack()
    {

    }

    public override void Damage(float damage)
    {
    }

    protected override void DeathCheck(bool batAttack)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Card.enemyDetectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Card.enemyAttackRange);
    }

}
