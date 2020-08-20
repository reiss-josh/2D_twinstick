using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 shotDirection;
    public float moveSpeed = 500f;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Awake()
    {
        shotDirection = transform.rotation * Vector2.right;
        rb2d = GetComponent<Rigidbody2D>();
        var frc = shotDirection * moveSpeed;
        Debug.Log(frc);
        rb2d.AddForce(shotDirection * moveSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
