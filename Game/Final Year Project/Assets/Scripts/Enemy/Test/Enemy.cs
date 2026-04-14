using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public LayerMask groundLayer, playerLayer;
    public float health;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange;
    public int damage;

    //public Animator animator;
    private bool alreadyAttacked;
    private bool takeDamage;
    public bool playerInSightRange;



    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
        bool playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        
    }
    /*
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        Vector2 direction = (walkPoint - transform.position).normalized;
        transform.position += (Vector3)direction * 2f * Time.deltaTime;

        if (Vector2.Distance(transform.position, walkPoint) < 0.5f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y + randomY,
            transform.position.z
        );

        walkPointSet = true;
    }

   private void ChasePlayer()
{
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * 3f * Time.deltaTime;
    }


  private void AttackPlayer()
{
    navAgent.SetDestination(transform.position);

    if (!alreadyAttacked)
    {
        transform.LookAt(player.position);
        alreadyAttacked = true;
        //animator.SetBool("Attack", true);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            /*
                YOU CAN USE THIS TO GET THE PLAYER HUD AND CALL THE TAKE DAMAGE FUNCTION

            PlayerHUD playerHUD = hit.transform.GetComponent<PlayerHUD>();
            if (playerHUD != null)
            {
               playerHUD.takeDamage(damage);
            }
            
        }
    }
}


    private void ResetAttack()
    {
        alreadyAttacked = false;
        //animator.SetBool("Attack", false);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        hitEffect.Play();
        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        //animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }
    */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
}
