using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_States : MonoBehaviour
{
    public NPC_Controller enemyController;
    public bool isAttacking = false;

    private void Start()
    {
        isAttacking = false;
    }
    public void NPCPatrolling()
    {
        isAttacking = false;
        if (enemyController.currentNode == null || AStarManager.instance == null)
            return;

        if (enemyController.path == null || enemyController.path.Count == 0)
        {
            Node[] nodes = AStarManager.instance.AllNodes();

            if (nodes == null || nodes.Length == 0)
                return;

            Node target = nodes[Random.Range(0, nodes.Length)];
            List<Node> newPath = AStarManager.instance.GeneratePath(enemyController.currentNode, target);

            if (newPath != null)
                enemyController.path = newPath;
        }

        CreatePath();

    }

    public void NPCAttack()
    {
        isAttacking = true;
    }
    public void NPCEngage()
    {
        isAttacking = false;
        if (enemyController.currentNode == null || enemyController.player == null || AStarManager.instance == null) // if the node the player is on is not in the enemys reach return null, if the player and astar instance is also null
            return;

        if (enemyController.path == null || enemyController.path.Count == 0)
        {
            Node target = AStarManager.instance.FindNearestNode(enemyController.player.position);// this finds the nearest node to where the player is

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
        bool playerInAttackRange = Vector2.Distance(transform.position, enemyController.player.position) < enemyController.attackRange;

        if (playerInAttackRange)
        {
            NPCAttack();
        }
        else
        {
            
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

}

