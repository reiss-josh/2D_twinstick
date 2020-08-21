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
    public float volume = 1f;
    
    public float spriteStretchX = 10f, spriteStretchY = 10f;
    public int damageAmt = 5, health = 100;

    public float iFrameTime = 0.4f;
    private float iFrameCounter = 0;
    public Color damageColor = new Color(255, 136, 136);
    
    public AudioClip dmgSound;
    public AudioClip deathSound;
    private bool dead = false;

    Path path;
    int currentWaypoint = 0;

    SpriteRenderer Sprite;
    Seeker seeker;
    Rigidbody2D rb;
    CircleCollider2D coll;
    Transform target;
    ParticleSystem particles;
    AudioSource audioSource;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {   
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }
        target = Player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        seeker = GetComponent<Seeker>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        InvokeRepeating("UpdatePath", 0f, recalculateTiming);
    }

    void UpdatePath()
    {
        if (dead) return;
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
        //play death sound and animation
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(deathSound, volume);
        anim.SetBool("isDead", true);

        //make sure we stop doing things
        dead = true;
        Destroy(rb);
        Destroy(coll);
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
        particles.Play();


        if (health < 0) Death();
        else {
            audioSource.pitch = Random.Range(0.5f, 1.5f);
            audioSource.PlayOneShot(dmgSound, volume);
        }
        StartCoroutine(FlashColor(damageColor, 3, 0.1f));
    }

    void FixedUpdate()
    {
        if (dead) return;

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
            if (iFrameCounter <= 0)
            {
                iFrameCounter = iFrameTime;
                TakeDamage(collision.gameObject.GetComponent<Projectile>().damageAmt);
            }
        }
    }
}
