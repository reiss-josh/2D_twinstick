using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private int playerHealth;
    private PlayerController playerObj;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = playerObj.health;
        healthText.text = "HP: " + playerHealth.ToString();
    }
}
