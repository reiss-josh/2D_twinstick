using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class Astar_Generate_GridGraph : MonoBehaviour
{
    public GameObject mapHolder;
    private Tilemap wallMap;
    private TilemapCollider2D coll;
    public float tileSize = 0.8f;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<AstarPath>();
        wallMap = mapHolder.GetComponent<Tilemap>();
        coll = mapHolder.GetComponent<TilemapCollider2D>();
    }

    void Start()
    {
        var size = wallMap.size;
        var cent = wallMap.cellBounds.center;
        cent *= tileSize;

        // This creates a Grid Graph
        AstarData data = AstarPath.active.data;
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
        var gc = new GraphCollision();
        gc.use2D = true;
        gc.mask = LayerMask.GetMask("Environment");
        gg.collision = gc;
        gg.SetDimensions(size.x * 2, size.y * 2, tileSize/2);
        gg.center = cent + new Vector3(tileSize/4, tileSize/4, 0);
        gg.rotation = new Vector3(gg.rotation.y - 90, 270, 90);
        StartCoroutine(ScanMap());
    }

    IEnumerator ScanMap()
    {
        while(coll.bounds.extents == Vector3.zero)
        {
            yield return null;
        }
        AstarPath.active.Scan();
    }
}
