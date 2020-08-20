using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = GameObject.Find("Bow").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = GetMousePosition();
        var vect = mousePosition - transform.position;
        var ang = Vector2.SignedAngle(Vector2.right, vect);
        aimTransform.rotation = Quaternion.Euler(0,0,ang);
    }

    Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPosition;
    }
}
