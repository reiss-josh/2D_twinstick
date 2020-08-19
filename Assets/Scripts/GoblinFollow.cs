using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollow : MonoBehaviour
{
    public GameObject Player;
    private Transform playertf;
    private Rigidbody2D rb2d;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playertf = Player.GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var heading = playertf.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        Debug.Log(direction);
        rb2d.AddForce(direction * speed);
    }
}
