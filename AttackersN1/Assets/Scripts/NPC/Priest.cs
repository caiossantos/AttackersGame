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



        //behaviour.root = ;
        StartCoroutine(behaviour.Execute());
    }


    public override void Attack(string tag)
    {
        _animator.SetTrigger("IsAttacking");

        GameObject enemy = CheckUtilities.GetTheNearestGameObjectWithTag(transform.position, tag);
        if (enemy != null)
            transform.LookAt(enemy.transform.position, Vector3.up);

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
