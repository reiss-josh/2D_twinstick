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
    public float tileSize = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        AstarPath pth = gameObject.AddComponent<AstarPath>();
        pth.logPathResults = PathLog.OnlyErrors;
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
        
        /*var layer1 = LayerMask.NameToLayer("Environment");
        var layer2 = LayerMask.NameToLayer("Enemy");
        var mask1 = 1 << layer1;
        var mask2 = 1 << layer2;
        var combinedMask = mask1 | mask2;*/
        gc.mask = LayerMask.GetMask("Environment");
        gc.diameter = tileSize/2f;
        gg.collision = gc;
        gg.SetDimensions(size.x, size.y, tileSize);
        gg.center = cent;
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
