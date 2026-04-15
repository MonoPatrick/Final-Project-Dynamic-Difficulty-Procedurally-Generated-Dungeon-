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
    public float distance;
    bool isAttacking = false;


   public NPC_States states;
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
                states.NPCPatrolling();
                break;
            case StateMachine.Engage:
                states.NPCEngage();
                break;
            case StateMachine.Evade:
                states.NPCEvade();
                break;
            case StateMachine.Attacking:
                states.NPCAttack();
                break;
        }
        float dist = Vector2.Distance(transform.position, player.position);
        bool playerSeen = dist < distance;
        bool playerAttackRange = dist < attackRange;


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
        else if (playerSeen && playerAttackRange && currentState != StateMachine.Attacking && curHealth > (maxHealth * 20) / 100)
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
        //Debug.Log(isAttacking);
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


}