using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 shotDirection;
    public int damageAmt = 10;
    public float moveSpeed = 500f;
    public bool isStickyProjectile = true;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxColl;
    public float projectileLifespan = 30f; //how long to wait before deleting this after spawn
    public float offset = 1f; //if a projectile is further from this than it's target, we adjust how it looks
    
    void Awake()
    {
        shotDirection = transform.rotation * Vector2.right;
        rb2d = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        rb2d.AddForce(shotDirection * moveSpeed, ForceMode2D.Impulse);
        Invoke("ProjectileCleanup", projectileLifespan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStickyProjectile == true)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                var pointing = collision.transform.position - this.transform.position;
                //if we went through the target, move closer
                if (Vector3.Distance(this.transform.position, collision.transform.position) > offset) this.transform.position += pointing/2;
                //make sure it's facing at the target
                var ang = Vector2.SignedAngle(Vector2.right, pointing);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ang));
            }
            this.transform.parent = collision.transform;
            rb2d.velocity *= 0;
            Destroy(rb2d);
            boxColl.enabled = false;
        }
    }

    private void ProjectileCleanup()
    {
        Destroy(this.gameObject);
    }




}
