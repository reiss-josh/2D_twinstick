
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform Arrow;
    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<PlayerController>().shootEvent += Shoot;
    }

    public void Shoot(Vector3 arrowPos, Quaternion arrowAngle)
    {
        //Debug.Log(arrowPos);
        Instantiate(Arrow, arrowPos, arrowAngle);
    }
}