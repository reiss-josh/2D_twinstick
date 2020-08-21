using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public Transform EnemyGfx;

    public float speed = 200f, reboundForce = 100f;
    public float nextWaypointDistance = 3f;
    public float recalculateTiming = 0.5f;
    
    public float spriteStretchX = 10f, spriteStretchY = 10f;
    public int damageAmt = 5, health = 100;

    public float iFrameTime = 1f;
    private float iFrameCounter = 0;

    Path path;
    int currentWaypoint = 0;

    SpriteRenderer Sprite;
    Seeker seeker;
    Rigidbody2D rb;
    Transform target;

    // Start is called before the first frame update
    void Awake()
    {   
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }
        target = Player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
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

    void Death()
    {
        Destroy(gameObject);
    }

    public IEnumerator FlashColor(Color color, int numFlashes, float flashTime)
    {
        for (int i = 0; i < numFlashes; i++)
        {
            Sprite.color = (color);
            yield return new WaitForSeconds(flashTime);
            Sprite.color = (Color.white);
            yield return new WaitForSeconds(flashTime);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) Death();
        StartCoroutine(FlashColor(Color.red, 3, 0.1f));
    }

    void FixedUpdate()
    {
        if (iFrameCounter > 0) iFrameCounter -= Time.deltaTime;
        if (iFrameCounter < 0) iFrameCounter = 0;
        if (path == null)
            return;

        //check if at end of path
        if(currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var dir = (collision.transform.position - transform.position).normalized;
            var force = dir * -reboundForce;
            rb.AddForce(force);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().damageAmt);
        }
    }
}
