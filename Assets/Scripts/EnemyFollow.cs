using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFollow : MonoBehaviour
{
    public GameObject Player;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float recalculateTiming = 0.5f;
    public Transform EnemyGfx;
    public float spriteStretchX = 10f;
    public float spriteStretchY = 10f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {   
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }
        target = Player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, recalculateTiming);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            {seeker.StartPath(rb.position, target.position, OnPathComplete);}
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        //check if at end of path
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else reachedEndOfPath = false;

        //calculate vector heading towards target, then move towards it
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        //add rotation for emphasis

        EnemyGfx.rotation = Quaternion.Euler(rb.velocity.x * spriteStretchX, rb.velocity.y * spriteStretchY, -rb.velocity.x);
        

        //determine distance to nextwaypoint. if it has been reached, note that.
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) currentWaypoint++;
    }
}
