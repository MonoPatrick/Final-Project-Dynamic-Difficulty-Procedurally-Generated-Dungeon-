using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPC_Controller : MonoBehaviour
{
    public float maxHealth = 100f;
    public float curHealth;
    public int panicMultiplier = 1;

    
    public Node currentNode;
    public List<Node> path = new List<Node>();

    public LayerMask groundLayer, playerLayer;
    public float attackRange;
    public float distance;
    bool isAttacking = false;

    private DynamicDifficultyAdjustment DDA;

    public float baseSpeed;
    public float baseMaxHealth = 100f;

    private float lastDifficulty = -1f;
    private DynamicDifficultyAdjustment.Rank lastRank;

    public NPC_States states;
    public enum StateMachine
    {
        Patrol,
        Engage,
        Evade,
        Attacking,
        Knockback
    }

    public StateMachine currentState;

    public Transform player;

    public float speed;

    private void Start()
    {
        baseSpeed = 2.5f;
        currentState = StateMachine.Patrol;
        curHealth = maxHealth;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (DDA == null)
        {
            DDA = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<DynamicDifficultyAdjustment>();
        }
        if (states == null)
        {
            states = GetComponent<NPC_States>();
            if (states == null)
                Debug.LogError("NPC_States not assigned to NPC_Controller!");
        }
    }

    private void Update()
    {
        if(currentState != StateMachine.Knockback)
        {
            switch (currentState)
            {
                case StateMachine.Patrol:
                    states.NPCPatrolling();
                    break;
                case StateMachine.Engage:
                    states.NPCEngage(10 * DDA.difficulty);
                    break;
                case StateMachine.Evade:
                    states.NPCEvade();
                    break;
                case StateMachine.Attacking:
                    states.NPCAttack( 10 * DDA.difficulty);
                    break;
            }
        }
        
        float dist = Vector2.Distance(transform.position, player.position);
        bool playerSeen = dist < distance;
        bool playerAttackRange = dist < attackRange;

        if (currentState != StateMachine.Knockback)
        {
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
        }
        // CreatePath();
        //attack();
        //Debug.Log(isAttacking);
        UpdateDifficultyIfNeeded();

    }



    /*
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/
    private void UpdateDifficultyIfNeeded()
    {
        if (DDA.difficulty != lastDifficulty || DDA.playerRank != lastRank)
        {
            if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank3 ||
                DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank4 ||
                DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
            {
                speed = DDA.difficulty * baseSpeed;
            }
            maxHealth = DDA.difficulty * baseMaxHealth;

            lastDifficulty = DDA.difficulty;
            lastRank = DDA.playerRank;
        }
    }

    private void enemyDifficultyChange()
    {
        if(DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank3 || DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank4 || DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
        {
            speed = DDA.difficulty * baseSpeed;
        }
        

        maxHealth = DDA.difficulty * baseMaxHealth;
        
    }


}