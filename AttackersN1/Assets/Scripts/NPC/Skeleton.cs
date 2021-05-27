using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Enemy
{
    private const string ENEMY_TAG = "Ally";
    private const string OBJECTIVE_TAG = "EnemyObjective";

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
        attackSequence1.children.Add(new NodeEnemyNear(ENEMY_TAG, Card.enemyDetectionRange));
        BTInverter isEnemyOutsideMyAttackRange = new BTInverter();
        isEnemyOutsideMyAttackRange.children.Add(new NodeEnemyNear(ENEMY_TAG, Card.enemyAttackRange));
        attackSequence1.children.Add(isEnemyOutsideMyAttackRange);
        attackSequence1.children.Add(new NodeGoToTarget(ENEMY_TAG, _navMeshAgent));

        BTSequence attackSequence2 = new BTSequence();
        attackSequence2.children.Add(new NodeEnemyNear(ENEMY_TAG, Card.enemyAttackRange));
        attackSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        attackSequence2.children.Add(new NodeAttack(ENEMY_TAG, Card.attackArea));

        BTSelector attackSelector1 = new BTSelector();
        attackSelector1.children.Add(attackSequence2);
        attackSelector1.children.Add(attackSequence1);
        attackSelector1.children.Add(attackSequence2);
        #endregion

        #region OBJECTIVE_ATTACK
        BTSequence objectiveSequence1 = new BTSequence();
        BTInverter isObjectiveOutsideMyAttackRange = new BTInverter();
        isObjectiveOutsideMyAttackRange.children.Add(new NodeEnemyNear(OBJECTIVE_TAG, Card.enemyAttackRange));
        objectiveSequence1.children.Add(isObjectiveOutsideMyAttackRange);
        attackSequence1.children.Add(new NodeEnemyNear(OBJECTIVE_TAG, Card.enemyDetectionRange));
        BTInverter imStoppedObjective = new BTInverter();
        imStoppedObjective.children.Add(new NodeIsMoving(_navMeshAgent));
        objectiveSequence1.children.Add(imStoppedObjective);
        objectiveSequence1.children.Add(new NodeGoToTarget(OBJECTIVE_TAG, _navMeshAgent));

        BTSequence objectiveSequence2 = new BTSequence();
        objectiveSequence2.children.Add(new NodeEnemyNear(OBJECTIVE_TAG, Card.enemyAttackRange));
        objectiveSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        objectiveSequence2.children.Add(new NodeAttack(OBJECTIVE_TAG, Card.attackArea));

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
            StartCoroutine(DoAttack(tag));
    }

    private IEnumerator DoAttack(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(Muzzle.transform.position, Card.attackArea);
        if (colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.CompareTag(tag))
                {
                    IDamage damage = collider.GetComponent<IDamage>();
                    if (damage != null)
                        damage.Damage(Card.attackDamage);
                }
            }
        }

        _isAttacking = true;

        yield return new WaitForSeconds(Card.attackSpeed);

        _isAttacking = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Card.enemyDetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Card.enemyAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Muzzle.transform.position, Card.attackArea);
    }

}