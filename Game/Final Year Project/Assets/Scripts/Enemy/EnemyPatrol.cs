using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform waypoint;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float speed;
    private float waitTime;
    public float startWaitTime;



    void Start()
    {
        waitTime = startWaitTime;
        waypoint.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoint.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                waypoint.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}

