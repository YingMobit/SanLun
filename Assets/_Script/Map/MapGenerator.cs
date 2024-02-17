using System;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    // 声明
    public Tilemap FloorTilemap;            // 地面
    public TileBase BarrierTile;     // 屏障瓦片
    public TileBase[] GrassTiles;           // 草地瓦片组
    public TileBase FloorTile;          // 黑色石砖瓦片
    public Grid grid;         // 网格父物体
    public int[,] MapData;         // 存储地图数据xy坐标 数据0未解锁1解锁
    public int BarrierNum;          // 屏障命名数

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3Int nowPos;          // grid当前所在Pos

    // 函数

    // 初始
    private void Start()
    {
        // 清空地图
        ClearChildren(grid.gameObject);
        // 初始小地图
        BarrierNum = 1;
        InitialMap();
        //注册函数
        Health.UpdataMap += AddMap;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetPos(Input.mousePosition);
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
        // 初始化路径点
        nowPos = new Vector3Int(0, 0, 0);

        // 创建FloorTilemap,添加组件
        GameObject tilemapObj = new GameObject("FloorTilemap");
        tilemapObj.transform.parent = grid.transform; // 将Tilemap设为grid的子物体
        FloorTilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 0;

        //创建初始地图
        GeneratePlot(nowPos);
        GenerateBarrier(nowPos);
    }

    private void GeneratePlot(Vector3Int centerPos)
    {
        // 创建地面瓦片   13*9
        for (int x = -6+ centerPos.x; x < 7+ centerPos.x; x++)
        {
            for (int y = -4+ centerPos.y; y < 5+ centerPos.y; y++)
            {
                TileBase tile = GrassTiles[UnityEngine.Random.Range(0, GrassTiles.Length)];
                FloorTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }//centorPos是要生成的中心点

    private void GenerateCorner(Vector3Int centerPos)
    {
        FloorTilemap.SetTile(centerPos + new Vector3Int(+7, +5, 0), FloorTile);
        FloorTilemap.SetTile(centerPos + new Vector3Int(+7, -5, 0), FloorTile);
        FloorTilemap.SetTile(centerPos + new Vector3Int(-7, +5, 0), FloorTile);
        FloorTilemap.SetTile(centerPos + new Vector3Int(-7, -5, 0), FloorTile);
    }

    private void GenerateBarrier(Vector3Int centerPos)
    {
        bool hasTile;
        //Direction.Up
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(0, +9, 0));//  || FloorTilemap.HasTile(centerPos + new Vector3Int(0, +5, 0));
        if (!hasTile)// 没有小地图那么生成屏障
        {
            GameObject Barrier1 = new GameObject("Barrier" + BarrierNum++);// 上 0,+9,0
            Barrier1.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier1,centerPos, Direction.Up);
        }

        //Direction.Down
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(0, -9, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(0, -5, 0));
        if (!hasTile)// 没有小地图那么生成屏障
        {
            GameObject Barrier2 = new GameObject("Barrier" + BarrierNum++);// 下 0,-9,0
            Barrier2.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier2, centerPos, Direction.Down);
        }

        //Direction.Left
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(-13, 0, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(-7, 0, 0));
        if (!hasTile)// 没有小地图那么生成屏障
        {
            GameObject Barrier3 = new GameObject("Barrier" + BarrierNum++);// 左 -13,0,0
            Barrier3.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier3, centerPos, Direction.Left);
        }

        //Direction.Right
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(+13, 0, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(+7, 0, 0));
        if (!hasTile)// 没有小地图那么生成屏障
        {
            GameObject Barrier4 = new GameObject("Barrier" + BarrierNum++);// 右 +13,0,0
            Barrier4.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier4, centerPos, Direction.Right);
        }
    }

    private void AddBarrierComponent(GameObject barrier,Vector3Int centerPos,Direction dir)
    {
        Tilemap tilemap = barrier.AddComponent<Tilemap>();
        barrier.AddComponent<TilemapRenderer>();
        barrier.AddComponent<TilemapCollider2D>().usedByComposite = true;
        barrier.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        barrier.AddComponent<CompositeCollider2D>();
        barrier.tag = "Barrier";
        barrier.AddComponent<Defence>();
        Health health = barrier.AddComponent<Health>();

        Vector3Int setPos = Vector3Int.zero;
        switch (dir)
        {
            case Direction.Up://-6，+5，0 左至右
                setPos = centerPos + new Vector3Int(-6, +5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Down://-6,-5,0 左至右
                setPos = centerPos + new Vector3Int(-6, -5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Left:// -7,-4,0 下至上
                setPos = centerPos + new Vector3Int(-7, -4, 0);
                for (int i = 0; i < 9; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(0, +1, 0);
                }
                break;
            case Direction.Right:
                setPos = centerPos + new Vector3Int(+7, -4, 0);
                for (int i = 0; i < 9; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(0, +1, 0);
                }
                break;
        }
        if(dir ==Direction.Up||dir==Direction.Down)
        {
            health.Posdata = new Health.Pos { max = setPos, dir = Health.Direction.Horizon };
        }
        if (dir == Direction.Left || dir == Direction.Right)
        {
            health.Posdata = new Health.Pos { max = setPos, dir = Health.Direction.Vertical };
        }


    }

    private void ChangeFloor(Health.Pos Posdata)
    {
        Vector3Int Pos = Posdata.max;
        if (Posdata.dir == Health.Direction.Horizon)
        {
            for(int i=0;i<13;i++)
            {
                Pos += new Vector3Int(-1, 0, 0);
                FloorTilemap.SetTile(Pos, FloorTile);
            }
        }
        if (Posdata.dir == Health.Direction.Vertical)
        {
            for (int i = 0; i < 9; i++)
            {
                Pos += new Vector3Int(0, -1, 0);
                FloorTilemap.SetTile(Pos, FloorTile);
            }
        }
    }

    public void AddMap(Health.Pos Posdata,GameObject barrier)
    {
        Vector3Int centerPos = Vector3Int.zero;// 打的右边
        if (Posdata.dir == Health.Direction.Horizon) { centerPos = Posdata.max + new Vector3Int(-7, 0, 0); }
        if (Posdata.dir == Health.Direction.Vertical) { centerPos = Posdata.max + new Vector3Int(0, -5, 0); }
        Debug.Log("centerPos1:" + centerPos);// 7 0 0，barrier中心
        if (Posdata.dir == Health.Direction.Horizon)//上下哪一个
        {
            centerPos = centerPos + new Vector3Int(0, -5, 0);
            if(FloorTilemap.HasTile(centerPos))
            {
                centerPos += new Vector3Int(0, +10, 0);
            }
        }
        else// 竖着的 左右选一个
        {
            centerPos = centerPos + new Vector3Int(-7, 0, 0);
            if (FloorTilemap.HasTile(centerPos))// 有的话换另一边
            {
                centerPos += new Vector3Int(+14, 0, 0);
            }
        }

        Debug.Log("centerPlotPos" + centerPos);
        GeneratePlot(centerPos);
        GenerateBarrier(centerPos);
        GenerateCorner(centerPos);
        ChangeFloor(Posdata);
    }

    // 其他函数
    // 鼠标点击获取网格坐标
    public void GetPos(Vector3 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3Int gridPos = grid.WorldToCell(worldPos);
        Debug.Log(gridPos);
    }
}