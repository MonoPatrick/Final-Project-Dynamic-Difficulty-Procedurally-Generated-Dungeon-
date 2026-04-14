using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPC_Controller : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
    public int panicMultiplier = 1;


    public Node currentNode;
    public List<Node> path = new List<Node>();

    public LayerMask groundLayer, playerLayer;
    public float attackRange;
    bool isAttacking = false;

    public enum StateMachine
    {
        Patrol,
        Engage,
        Evade,
        Attacking
    }

    public StateMachine currentState;

    public Transform player;

    public float speed = 1f;

    private void Start()
    {
        currentState = StateMachine.Patrol;
        curHealth = maxHealth;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Engage:
               Engage();
                break;
            case StateMachine.Evade:
               Evade();
                break;
            case StateMachine.Attacking:
                attack();
                break;
        }
        
        bool playerSeen = Vector2.Distance(transform.position, player.position) < 2.0f;
        bool playerAttackRange = Vector2.Distance(transform.position, player.position) < 2.5f;


        if (!playerSeen && !playerAttackRange && currentState != StateMachine.Patrol && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Patrol;
            path.Clear();
        }
        
        
        
        else if (playerSeen && !playerAttackRange && currentState != StateMachine.Engage && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Engage;
            path.Clear();
        }
        else if (playerSeen && playerAttackRange && currentState != StateMachine.Engage && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Attacking;
            path.Clear();
        }
        else if (currentState != StateMachine.Evade && curHealth <= (maxHealth * 20) / 100)
        {
            panicMultiplier = 2;
            currentState = StateMachine.Evade;
            path.Clear();
        }
       // CreatePath();
        //attack();
        Debug.Log(isAttacking);
    }

    void Patrol()
    {
        if (currentNode == null || AStarManager.instance == null)
            return;

        if (path == null || path.Count == 0)
        {
            Node[] nodes = AStarManager.instance.AllNodes();

            if (nodes == null || nodes.Length == 0)
                return;

            Node target = nodes[Random.Range(0, nodes.Length)];
            List<Node> newPath = AStarManager.instance.GeneratePath(currentNode, target);

            if (newPath != null)
                path = newPath;
        }

        CreatePath();

    }
    void attack()
    {
        
    }
    void Engage()
    {
        if (currentNode == null || player == null || AStarManager.instance == null) // if the node the player is on is not in the enemys reach return null, if the player and astar instance is also null
            return;

        if (path == null || path.Count == 0)
        {
            Node target = AStarManager.instance.FindNearestNode(player.position);// this finds the nearest node to where the player is

            if (target == null)
            {
                Debug.LogError("No node for players position");
                return;
            }

            List<Node> newPath = AStarManager.instance.GeneratePath(currentNode, target);

            if (newPath != null && newPath.Count > 0)
            {
                path = newPath;
            }
        }
        bool playerInAttackRange = Vector2.Distance(transform.position, player.position) < attackRange;
        if (playerInAttackRange)
        {
            attack();
        }
        else
        {
            isAttacking = false;
        }
        CreatePath();
    }

    void Evade()
    {
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
        }
        CreatePath();
    }
    
    private void OnDrawGizmos()
    {
        if (path == null || path.Count == 0)
            return;
        if (path.Count > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i].transform.position, path[i - 1].transform.position);
            }
        }
    }

    public void CreatePath()
    {
        if (path == null || path.Count == 0)
            return;

        int x = 0;

        
        if (x >= path.Count || path[x] == null)
            return;

        Vector3 targetPos = new Vector3(path[x].transform.position.x,path[x].transform.position.y,-0.5f);

       
        transform.position = Vector3.MoveTowards(transform.position, targetPos, (speed * panicMultiplier) * Time.deltaTime);

        

        if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
        {
            currentNode = path[x];
            path.RemoveAt(x);
        }
    }
}