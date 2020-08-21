using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGfxHelper : MonoBehaviour
{
    void DestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
