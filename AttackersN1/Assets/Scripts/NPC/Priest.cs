using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Priest : Enemy
{
    [SerializeField] private GameObject txtLifeEarnedFeedback;

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

        #region HEAL

        BTSequence healSequence1 = new BTSequence();
        //healSequence1.children.Add(new NodeHeal("Ally", Card.attackArea));
        healSequence1.children.Add(new NodeEnemyNear("Ally", Card.enemyDetectionRange));
        BTInverter isAllyOutsideMyHealRange = new BTInverter();
        isAllyOutsideMyHealRange.children.Add(new NodeEnemyNear("Ally", Card.enemyAttackRange));
        healSequence1.children.Add(isAllyOutsideMyHealRange);
        healSequence1.children.Add(new NodeGoToTarget("Ally", _navMeshAgent));

        BTSequence healSequence2 = new BTSequence();
        healSequence2.children.Add(new NodeHeal("Ally", Card.attackArea));  //Verifica se alguém precisa de cura (o nome da classe tá errado)
        healSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        healSequence2.children.Add(new NodeAttack("Ally", Card.attackArea));

        BTSelector healSelector1 = new BTSelector();
        healSelector1.children.Add(healSequence2);
        healSelector1.children.Add(healSequence1);

        #endregion

        #region ATTACK_ENEMY
        BTSequence attackSequence1 = new BTSequence();
        attackSequence1.children.Add(new NodeEnemyNear("Enemy", Card.enemyDetectionRange));
        BTInverter isEnemyOutsideMyAttackRange = new BTInverter();
        isEnemyOutsideMyAttackRange.children.Add(new NodeEnemyNear("Enemy", Card.enemyAttackRange));
        attackSequence1.children.Add(isEnemyOutsideMyAttackRange);
        attackSequence1.children.Add(new NodeGoToTarget("Enemy", _navMeshAgent));

        BTSequence attackSequence2 = new BTSequence();
        attackSequence2.children.Add(new NodeEnemyNear("Enemy", Card.enemyAttackRange));
        attackSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        attackSequence2.children.Add(new NodeAttack("Enemy", Card.attackArea));

        BTSelector attackSelector1 = new BTSelector();
        attackSelector1.children.Add(attackSequence2);
        attackSelector1.children.Add(attackSequence1);
        attackSelector1.children.Add(attackSequence2);
        #endregion

        #region OBJECTIVE_ATTACK
        BTSequence objectiveSequence1 = new BTSequence();
        BTInverter isObjectiveOutsideMyAttackRange = new BTInverter();
        isObjectiveOutsideMyAttackRange.children.Add(new NodeEnemyNear("AllyObjective", Card.enemyAttackRange));
        objectiveSequence1.children.Add(isObjectiveOutsideMyAttackRange);
        attackSequence1.children.Add(new NodeEnemyNear("AllyObjective", Card.enemyDetectionRange));
        //objectiveSequence1.children.Add(new NodeEnemyNear("AllyObjective", Card.enemyDetectionRange));
        BTInverter imStoppedObjective = new BTInverter();
        imStoppedObjective.children.Add(new NodeIsMoving(_navMeshAgent));
        objectiveSequence1.children.Add(imStoppedObjective);
        objectiveSequence1.children.Add(new NodeGoToTarget("AllyObjective", _navMeshAgent));

        BTSequence objectiveSequence2 = new BTSequence();
        objectiveSequence2.children.Add(new NodeEnemyNear("AllyObjective", Card.enemyAttackRange));
        objectiveSequence2.children.Add(new NodeStopNavMeshDestination(_navMeshAgent));
        objectiveSequence2.children.Add(new NodeAttack("AllyObjective", Card.attackArea));

        BTSelector objectiveSelector1 = new BTSelector();
        objectiveSelector1.children.Add(objectiveSequence2);
        objectiveSelector1.children.Add(objectiveSequence1);
        #endregion

        BTSelector masterSelector = new BTSelector();
        masterSelector.children.Add(healSelector1);
        //masterSelector.children.Add(attackSelector1);
        //masterSelector.children.Add(objectiveSelector1);

        behaviour.root = healSelector1;
        StartCoroutine(behaviour.Execute());
    }


    public override void Attack(string tag)
    {
        _animator.SetTrigger("IsAttacking");

        GameObject enemy = CheckUtilities.GetTheNearestGameObjectWithTag(transform.position, tag);
        if (enemy != null)
        {
            transform.LookAt(enemy.transform.position, Vector3.up);
            Vector3 txtPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, enemy.transform.position.z);
            //var txtLifeEarned = Instantiate(txtLifeEarnedFeedback, txtPos, Quaternion.identity);
            Instantiate(txtLifeEarnedFeedback, enemy.transform.position, Quaternion.identity);
            //txtLifeEarned.GetComponent<UiTxtLifeEarned>().ChangeText(Card.attackDamage);
        }
        if (!_isAttacking)
        {
            StartCoroutine(DoAttack(tag, -Card.attackDamage, Card.attackArea));
        }
    }

    private IEnumerator DoAttack(string tag, float damageToMake, float area)
    {
        Collider[] colliders = Physics.OverlapSphere(Muzzle.transform.position, area);
        if (colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.CompareTag(tag))
                {
                    IDamage damage = collider.GetComponent<IDamage>();
                    if (damage != null)
                        damage.Damage(damageToMake);
                }
            }
        }

        _isAttacking = true;

        yield return new WaitForSeconds(Card.attackSpeed);

        _isAttacking = false;
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
