using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Enemy
{
    private string _enemyTag = "Ally";
    private string _objectiveTag = "EnemyObjective";

    private BTRoot behaviour;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isAttacking;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyInit();
        BehaviourTree();
    }

    private void Update()
    {
        if (_navMeshAgent.velocity != Vector3.zero)
            _animator.SetBool("IsMoving", true);
        else
            _animator.SetBool("IsMoving", false);
    }

    public override void BehaviourTree() 
    {
        behaviour = GetComponent<BTRoot>();

        #region ATTACK_ENEMY
        BTSequence attackSequence1 = new BTSequence();
        attackSequence1.children.Add(new NodeEnemyNear(_enemyTag, Card.enemyDetectionRange));
        BTInverter inverterEnemyNearAttackRange = new BTInverter();
        inverterEnemyNearAttackRange.children.Add(new NodeEnemyNear(_enemyTag, Card.enemyAttackRange));
        attackSequence1.children.Add(inverterEnemyNearAttackRange);
        BTInverter inverterMove = new BTInverter();
        inverterMove.children.Add(new NodeIsMoving(_navMeshAgent));
        attackSequence1.children.Add(new NodeGoToTarget(_enemyTag, _navMeshAgent));

        BTSequence attackSequence2 = new BTSequence();
        attackSequence2.children.Add(new NodeEnemyNear(_enemyTag, Card.enemyAttackRange));
        attackSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));

        BTSequence attackSequence3 = new BTSequence();
        attackSequence3.children.Add(new NodeEnemyNear(_enemyTag, Card.enemyAttackRange));
        attackSequence3.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        attackSequence3.children.Add(new NodeAttack(_enemyTag, Card.enemyAttackRange));

        BTSelector attackSelector1 = new BTSelector();
        attackSelector1.children.Add(attackSequence3);
        attackSelector1.children.Add(attackSequence2);
        attackSelector1.children.Add(attackSequence1);
        #endregion

        #region OBJECTIVE_ATTACK
        BTSequence objectiveSequence1 = new BTSequence();
        BTInverter objectiveInverter1 = new BTInverter();
        objectiveInverter1.children.Add(new NodeEnemyNear(_objectiveTag, Card.enemyAttackRange));
        objectiveSequence1.children.Add(objectiveInverter1);
        objectiveSequence1.children.Add(new NodeGoToTarget(_objectiveTag, _navMeshAgent));

        BTSequence objectiveSequence2 = new BTSequence();
        objectiveSequence2.children.Add(new NodeEnemyNear(_objectiveTag, Card.enemyAttackRange));
        objectiveSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        objectiveSequence2.children.Add(new NodeAttack(_objectiveTag, Card.attackArea));

        BTSelector objectiveSelector1 = new BTSelector();
        objectiveSelector1.children.Add(objectiveSequence2);
        objectiveSelector1.children.Add(objectiveSequence1);
        #endregion

        BTSelector masterSelector = new BTSelector();
        masterSelector.children.Add(attackSelector1);
        masterSelector.children.Add(objectiveSelector1);

        behaviour.root = masterSelector;
        StartCoroutine(behaviour.Execute());
    }

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

    public override void Attack(string tag)
    {
        _animator.SetTrigger("IsAttacking");

        Transform enemy = CheckUtilities.GetTheNearestGameObjectWithTag(transform.position, tag).transform;
        transform.LookAt(enemy.position, Vector3.up);

        if (!_isAttacking)
            StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(Muzzle.transform.position, Card.attackArea);
        if (colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.CompareTag(_enemyTag))
                {
                    Enemy damage = collider.GetComponent<Enemy>();
                    if (damage != null)
                        damage.Damage(Card.attackDamage);
                }
            }
        }

        _isAttacking = true;

        yield return new WaitForSeconds(Card.attackSpeed);

        _isAttacking = false;
    }
}