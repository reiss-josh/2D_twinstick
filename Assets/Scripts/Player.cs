using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float xMove, yMove;
    public float moveSpeed = 150f;
    public float reboundForce = 1500f;
    public float iFrameTime = 1f;
    private float iFrameCounter = 0;
    private Rigidbody2D rb2d;
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        PerfMove();
        if (iFrameCounter > 0) iFrameCounter-=Time.deltaTime;
        if (iFrameCounter < 0) iFrameCounter = 0;
    }

    void GetInput()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");
    }

    void PerfMove()
    {
        var moveVector = new Vector2(xMove, yMove) * moveSpeed;
        rb2d.AddForce(moveVector);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.tag == "Enemy") && (iFrameCounter <= 0))
        {
            health -= collision.gameObject.GetComponent<Enemy>().damageAmt;
            var dir = (collision.transform.position - transform.position).normalized;
            var force = dir * -reboundForce;
            //Debug.DrawRay(transform.position, force, Color.red, 1f);
            rb2d.AddForce(force);
            iFrameCounter = iFrameTime;
        }
    }
}
