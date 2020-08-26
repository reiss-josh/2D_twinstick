using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private float xMove, yMove;
    private bool shootButton;
    public float moveSpeed = 150f, speedCap = 10f, reboundForce = 1500f, frictionAmt = 0.2f;
    public int health = 20;

    public float iFrameTime = 1f;
    private float iFrameCounter = 0;

    private Rigidbody2D rb2d;
    private Transform aimTf;

    public event Action<Vector3, Quaternion> shootEvent;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        aimTf = GameObject.Find("Bow").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) Application.Quit(); //death
        GetInput();
        iFrameUpdate();
        if (shootButton && shootEvent != null) shootEvent(aimTf.GetChild(0).position, aimTf.rotation);
    }

    // FixedUpdate is called when the Physics System updates
    void FixedUpdate()
    {
        PerfMove();
        PerfFriction();
    }

    //should make this a coroutine
    void iFrameUpdate()
    {
        if (iFrameCounter > 0) iFrameCounter -= Time.deltaTime;
        if (iFrameCounter < 0) iFrameCounter = 0;
    }

    //should probably make this its own script
    void GetInput()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");
        shootButton = Input.GetButtonDown("Fire1");
        if (Input.GetButtonDown("Quit")) Application.Quit();
    }

    //should push this out to some kind of utilities script
    bool SameSign(float num1, float num2) {
        if (num1 > 0 && num2 < 0)
            return false;
        if (num1 < 0 && num2 > 0)
            return false;
        return true;
    }

    void PerfMove()
    {
        Vector2 moveVector = new Vector2(xMove, yMove) * moveSpeed * Time.deltaTime;
        Vector2 hypo = rb2d.velocity + moveVector; //see what we're gonna do

        //speed limiting
        //if we're not at the speed limit yet...
        if (rb2d.velocity.magnitude < speedCap)
        {
            //if current keypress would push us over the speedlimit, then put us at the speedlimit.
            if ((hypo.magnitude > speedCap)) rb2d.velocity = Vector2.ClampMagnitude(hypo, speedCap);
            //otherwise, just add the movement
            else rb2d.velocity += (moveVector);
        }
        //Debug.Log(rb2d.velocity);
        //Debug.DrawRay(transform.position, rb2d.velocity);
    }

    void PerfFriction()
    {
        float xMag = rb2d.velocity.x;
        float yMag = rb2d.velocity.y;
        var threshold = 1f;
        //if current velocity is below the threshold, remember that
        bool xBelowThresh = (Mathf.Abs(xMag) < threshold);
        bool yBelowThresh = (Mathf.Abs(yMag) < threshold);
        //if velocity is below the threshold, just set it to 0
        rb2d.velocity = new Vector2(xBelowThresh ? 0 : rb2d.velocity.x, yBelowThresh ? 0 : rb2d.velocity.y);

        //actually apply the friction
        Vector2 opposite = -rb2d.velocity.normalized;
        rb2d.AddForce(opposite * Time.deltaTime * frictionAmt);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.tag == "Enemy") && (iFrameCounter <= 0))
        {
            health -= collision.gameObject.GetComponent<Enemy>().damageAmt;
            var dir = (collision.transform.position - transform.position).normalized;
            Vector2 force = dir * -reboundForce;
            rb2d.velocity += force;
            iFrameCounter = iFrameTime;
        }
    }
}
