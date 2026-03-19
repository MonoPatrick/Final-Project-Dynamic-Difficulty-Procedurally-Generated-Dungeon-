using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyAi : MonoBehaviour
{
    //following player
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Transform player;
    public Transform target;
    //Patrolling
    public Transform[] waypoint;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private float waitTime;
    public float startWaitTime;
    //Enemy attack
    private int postRankDamage;
    public int preRankDamage;

    public Player playerRank;
    // enenmy movement
    public float speed;
    private float enemySpeed;
    public float nextWaypointDistance = 3f;
    bool reachedEndOfPath = false;
    public bool patrolling;
    public bool seeking;
    int currentWaypoint = 0;
    public float detectionRange = 5f;

    void Start()
    {

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemySpeed = speed;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        waitTime = startWaitTime;
        //waypoint.position = new Vector2(Random.Range(minX, maxX),Random.Range(minY, maxY));
        
    }
    void UpdatePath ()
    {
        
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        
    }
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            
        }
    }
    // Update is called once per frame
    void Update()
    {

        speed = enemySpeed * playerRank.multiplier; 
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        if (distanceToPlayer < detectionRange)
        {
          // patrolling = false;
           // seeking = true;
        }
        else
        {
            //patrolling = true;
            //seeking = false;
        }
        if (patrolling)
        {
            target.position = waypoint[currentWaypoint].position;
            patrol();
        }

        if (seeking)
        {
            target.position = player.position;
            seekingPlayer();
        };
        Debug.Log(currentWaypoint);

    }
    
    void patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint[currentWaypoint].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoint[currentWaypoint].position) < 0.2f)
        {
            
            if (waitTime <= 0)
            {
               currentWaypoint = GetRandomPatrolPoint();
                waitTime = startWaitTime;
            }
            

            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
   

    int GetRandomPatrolPoint()
    {
        return (Random.Range(0, waypoint.Length));
    }
    void seekingPlayer()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
}
}



/*
 * void patrol()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            // Pick new patrol node
            waypoint.position = GetRandomPatrolPoint();
            seeker.StartPath(rb.position, waypoint.position, OnPathComplete);
            return;
        }

        // Follow the path to waypoint.position 
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }
 */


