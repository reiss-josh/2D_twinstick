using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGoblinSpawner : MonoBehaviour
{
    public GameObject Goblin;
    public GameObject Player;
    public int numGoblins;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 loc = Player.transform.position;
        for(int i = 0; i < numGoblins; i++)
        {
            var newGob = Instantiate(Goblin, loc, Quaternion.Euler(Vector3.up));
            newGob.transform.parent = gameObject.transform;
        }
    }
}
