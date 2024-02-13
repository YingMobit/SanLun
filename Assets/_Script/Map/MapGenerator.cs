using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    // 声明
    public Tilemap FloorTilemap;            // 地面
    public GameObject[] BarrierPrefabs;     // 屏障预制体组
    public TileBase[] GrassTiles;           // 草地瓦片组
    public TileBase FloorTile;          // 黑色石砖瓦片
    public Grid grid;         // 网格父物体
    public int[,] MapData;         // 存储地图数据xy坐标 数据0未解锁1解锁
    public int BarrierNum;          // 屏障命名数

    private Vector3Int nowPos;          // grid当前所在Pos

    // 函数

    // 初始
    private void Start()
    {
        // 清空地图
        ClearChildren(grid.gameObject);
        // 初始小地图
        InitialMap();
        // 初始屏障
        BarrierNum = 1;
        InitialBarrier();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GetPos();
        }

    }

    private void ClearChildren(GameObject parent)
    {
        if (parent == null)
        {
            Debug.LogError("Grid not assigned!");
            return;
        }

        // 遍历子物体并销毁
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitialMap()
    {
        // 创建FloorTilemap,添加组件
        GameObject tilemapObj = new GameObject("FloorTilemap");
        tilemapObj.transform.parent = grid.transform; // 将Tilemap设为grid的子物体
        FloorTilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 0;

        // 创建地面瓦片   初始13*9
        for (int x = -6; x < 7; x++)
        {
            for (int y = -4; y < 5; y++)
            {
                TileBase tile = GrassTiles[Random.Range(0, GrassTiles.Length)];
                FloorTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void InitialBarrier()
    {
        GameObject Barrier1 = new GameObject("Barrier" + BarrierNum++);// 左
        GameObject Barrier2 = new GameObject("Barrier" + BarrierNum++);// 上
        GameObject Barrier3 = new GameObject("Barrier" + BarrierNum++);// 右
        GameObject Barrier4 = new GameObject("Barrier" + BarrierNum++);// 下
        AddBarrierComponent(Barrier1,1);
    }

    private void AddBarrierComponent(GameObject barrier,int direction)
    {
        bool hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(-13,0,0));
        if(!hasTile)//没有小地图那么生成屏障s
        {
            switch(direction)
        }
    }



    // 其他函数
    // 鼠标点击获取坐标
    public void GetPos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = grid.WorldToCell(worldPos);
        Debug.Log(gridPos);
    }
}