using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_States : MonoBehaviour
{
    [SerializeField] private Cooldown cooldown;
    public NPC_Controller enemyController;
    public bool isAttacking = false;

  
    public float weaponRange;
    public LayerMask playerlayer;

    private bool canAttack = true;
    float nextPatrolPathTime = 0f;
    float patrolPathCooldown = 0.2f;

    float nextChasePathTime = 0f;
    float chasePathCooldown = 0.1f;

    private void Start()
    {
        isAttacking = false;
    }
    public void NPCPatrolling()
    {
        isAttacking = false;
        if (enemyController.currentNode == null || AStarManager.instance == null)
            return;

        if ((enemyController.path == null || enemyController.path.Count == 0) && Time.time >= nextPatrolPathTime)
        {
            nextPatrolPathTime = Time.time + patrolPathCooldown;

            Node[] nodes = AStarManager.instance.AllNodes();
            if (nodes == null || nodes.Length == 0) return;

            Node target = nodes[Random.Range(0, nodes.Length)];
            List<Node> newPath = AStarManager.instance.GeneratePath(enemyController.currentNode, target);

            if (newPath != null)
                enemyController.path = newPath;
        }

        CreatePath();

    }

    public void NPCAttack(float damage)
    {
        if (!canAttack || cooldown.isOnCooldown)
        {
            
            return;
        }
        isAttacking = true;
        canAttack = false;
        

        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, weaponRange, playerlayer);
        Debug.Log("Hits found: " + player.Length);


        Debug.Log("Player Attacked Up");
        if (player.Length > 0)
        {
            DynamicDifficultyAdjustment.Instance.changeDifficulty(-0.06f);
            player[0].GetComponent<playerHealth>().ChangeHealth(-damage);
            Debug.Log("Player hurt!");
        }
        
        StartCoroutine(Timer(1.5f));
    }
    public void NPCEngage(float damage)
    {
        isAttacking = false;
        if (enemyController.currentNode == null || enemyController.player == null || AStarManager.instance == null) // if the node the player is on is not in the enemys reach return null, if the player and astar instance is also null
            return;

        if ((enemyController.path == null || enemyController.path.Count == 0) && Time.time >= nextChasePathTime)
        {
            nextChasePathTime = Time.time + chasePathCooldown;

            Node target = AStarManager.instance.FindNearestNode(enemyController.player.position);

            if (target == null)
            {
                Debug.LogError("No node for players position");
                return;
            }

            List<Node> newPath = AStarManager.instance.GeneratePath(enemyController.currentNode, target);

            if (newPath != null && newPath.Count > 0)
            {
                enemyController.path = newPath;
                
            }
        }

        
        float distSqr = (transform.position - enemyController.player.position).sqrMagnitude;
        bool playerInAttackRange = distSqr < enemyController.attackRange * enemyController.attackRange;

        if (playerInAttackRange)
        {
            NPCAttack(damage);
        }
        CreatePath();
    }

    public void NPCEvade()
    {
        isAttacking = false;
        if (enemyController.path.Count == 0)
        {
            enemyController.path = AStarManager.instance.GeneratePath(enemyController.currentNode, AStarManager.instance.FindFurthestNode(enemyController.player.transform.position));
        }
        CreatePath();
    }

    public void CreatePath()
    {
        if (enemyController.path == null || enemyController.path.Count == 0)
            return;

        int x = 0;


        if (x >= enemyController.path.Count || enemyController.path[x] == null)
            return;

        Vector3 targetPos = new Vector3(enemyController.path[x].transform.position.x, enemyController.path[x].transform.position.y, -0.5f);


        transform.position = Vector3.MoveTowards(transform.position, targetPos, (enemyController.speed * enemyController.panicMultiplier) * Time.deltaTime);



        if (Vector2.Distance(transform.position, enemyController.path[x].transform.position) < 0.1f)
        {
            enemyController.currentNode = enemyController.path[x];
            enemyController.path.RemoveAt(x);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponRange);
    }

    IEnumerator Timer(float time)
    {

        yield return new WaitForSeconds(time);
        canAttack = true;
        isAttacking = false;
    }

}

