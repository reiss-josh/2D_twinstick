using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float xMove, yMove;
    public float moveSpeed = 5f;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        PerfMove();
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


}
