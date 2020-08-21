using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    bool mouseMovedLast = true;
    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = GameObject.Find("Bow").transform;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAimAngle();
    }

    void UpdateAimAngle()
    {
        float mouseMoved = Input.GetAxis("Mouse Y") + Input.GetAxis("Mouse X");
        float horizAim = Input.GetAxis("HorizontalAim");
        float vertAim = Input.GetAxis("VerticalAim");

        if (mouseMoved != 0) mouseMovedLast = true;
        if (horizAim + vertAim != 0) mouseMovedLast = false;
        Vector2 vect;
        if (mouseMovedLast)
        {
            Vector3 mousePosition = GetMousePosition();
            vect = mousePosition - transform.position;
        }
        else if (horizAim + vertAim != 0)
        {
            vect = new Vector2(horizAim, -vertAim);
        }
        else return;
        var ang = Vector2.SignedAngle(Vector2.right, vect);
        aimTransform.rotation = Quaternion.Euler(0, 0, ang);
    }

    Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPosition;
    }
}
