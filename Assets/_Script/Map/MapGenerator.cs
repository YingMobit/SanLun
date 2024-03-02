using System;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    // ����
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public Tilemap FloorTilemap;            // ����
    public Tilemap ObstacleTilemap;          // �ϰ�
    public TileBase BarrierTile;     // ������Ƭ
    public TileBase[] GrassTiles;           // �ݵ���Ƭ��
    public TileBase FloorTile;          // ��ɫʯש��Ƭ
    public TileBase ObstacleTile;         // �ϰ�����Ƭ
    public Grid grid;         // ��������
    public int[,] MapData;         // �洢��ͼ����xy���� ����0δ����1����// ��ʱ�ò���
    public int BarrierNum;          // ����������
    public float ObstacleFrequency = 0.4f;  // �ϰ��������Ƶ�ʣ���ֵԽ���ϰ���Խ��
    public int ObstacleSeed;              // ������ӣ��������ɲ�ͬ���ϰ���ģʽ
    public GameObject Exit;         // ����
    public GridStorage storage;         // �洢�������ĵ���Ϣ


    // �����߼�
    private bool exitCanGenerate;           // �Ƿ�������ɳ���


    public static MapGenerator Instance { get; private set; }           // ��̬�� Instance ���ԣ����ڻ�ȡ����ʵ��

    // ����

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);  // ���پ�ʵ��
        }
        Instance = this;
    }

    //�ⲿ����
    public bool EnemyPos(Vector3 worldPos)
    {
        Vector3Int gridPos = grid.LocalToCell(worldPos);
        if (FloorTilemap.HasTile(gridPos))
        {
            if(!ObstacleTilemap.HasTile(gridPos))
            {
                return true;
            }
        }
        return false;
    }

    public void GenerateExit()
    {
        exitCanGenerate = true;
    }

    // �ڲ�����
    private void Start()
    {
        // ��յ�ͼ
        ClearChildren(grid.gameObject);
        // ��ʼ����
        storage = new GridStorage();
        storage.ClearPos();
        // ��ʼС��ͼ
        BarrierNum = 1;
        exitCanGenerate = false;
        InitialMap();
        // ע�ắ��
        Health.UpdataMap += AddMap;
        Health.AddExit += GenerateExit;
        // ��������
        Health.ExitGenerateCount = 0;
        Health.BarrierDestroyCount = 0;
        PlayerPrefs.SetInt("Point", 0);
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

        // ���������岢����
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitialMap()
    {
        // ��ʼ��·����
        Vector3Int centerPos = Vector3Int.zero;

        // ����FloorTilemap,�������
        GameObject tilemapObj = new GameObject("FloorTilemap");
        tilemapObj.transform.parent = grid.transform; // ��Tilemap��Ϊgrid��������
        FloorTilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 0;
        tilemapObj.AddComponent<TilemapCollider2D>().usedByComposite = true;
        tilemapObj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        tilemapObj.AddComponent<CompositeCollider2D>();
        tilemapObj.tag = "Ground";

        // ��������ϰ���
        GameObject tilemapObj1 = new GameObject("ObstacleTilemap");
        tilemapObj1.transform.parent = grid.transform;
        ObstacleTilemap = tilemapObj1.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer1 = tilemapObj1.AddComponent<TilemapRenderer>();
        tilemapRenderer1.sortingOrder = 0;
        tilemapObj1.AddComponent<TilemapCollider2D>().usedByComposite = true;
        tilemapObj1.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        tilemapObj1.AddComponent<CompositeCollider2D>();
        tilemapObj1.tag = "Obstacle";


        //������ʼ��ͼ
        GeneratePlot(centerPos);
        GenerateObstacle(centerPos);
        ObstacleTilemap.SetTile(centerPos, null);
        GenerateBarrier(centerPos);
        GenerateCorner(centerPos);
        storage.AddPos(centerPos);
    }

    private void GeneratePlot(Vector3Int centerPos)
    {
        // ����������Ƭ   13*9
        for (int x = -6+ centerPos.x; x < 7+ centerPos.x; x++)
        {
            for (int y = -4+ centerPos.y; y < 5+ centerPos.y; y++)
            {
                TileBase tile = GrassTiles[UnityEngine.Random.Range(0, GrassTiles.Length)];
                FloorTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }//centorPos��Ҫ���ɵ����ĵ�

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
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier1 = new GameObject("Barrier" + BarrierNum++);// �� 0,+9,0
            Barrier1.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier1,centerPos, Direction.Up);
        }

        //Direction.Down
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(0, -9, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(0, -5, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier2 = new GameObject("Barrier" + BarrierNum++);// �� 0,-9,0
            Barrier2.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier2, centerPos, Direction.Down);
        }

        //Direction.Left
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(-13, 0, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(-7, 0, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier3 = new GameObject("Barrier" + BarrierNum++);// �� -13,0,0
            Barrier3.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier3, centerPos, Direction.Left);
        }

        //Direction.Right
        hasTile = FloorTilemap.HasTile(centerPos + new Vector3Int(+13, 0, 0));//  && FloorTilemap.HasTile(centerPos + new Vector3Int(+7, 0, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier4 = new GameObject("Barrier" + BarrierNum++);// �� +13,0,0
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
            case Direction.Up://-6��+5��0 ������
                setPos = centerPos + new Vector3Int(-6, +5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Down://-6,-5,0 ������
                setPos = centerPos + new Vector3Int(-6, -5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setPos, BarrierTile);
                    setPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Left:// -7,-4,0 ������
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

    private void GenerateObstacle(Vector3Int centerPos)
    {
        // ʹ�õ�ǰʱ����Ϊ����������һ�����ƫ����
        System.Random rand = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        float randomOffsetX = rand.Next(-100000, 100000);
        float randomOffsetY = rand.Next(-100000, 100000);// ƫ��������ѡ�ֲ�//TODO�ĳ�ȫ��

        for (int x = -6 + centerPos.x; x < 7 + centerPos.x; x++)
        {
            for (int y = -4 + centerPos.y; y < 5 + centerPos.y; y++)
            {
                // ��Perlin�����Ļ������������ƫ����
                float perlinValue = Mathf.PerlinNoise((x + randomOffsetX) * ObstacleFrequency, (y + randomOffsetY) * ObstacleFrequency);
                if (perlinValue > 0.7f) // �������ֵ�������ϰ����ϡ��̶�
                {
                    ObstacleTilemap.SetTile(new Vector3Int(x, y, 0), ObstacleTile);
                }
            }
        }
    }

    public void AddMap(Health.Pos Posdata,GameObject barrier)
    {
        Vector3Int centerPos = GetCenterPos(Posdata);
        if (centerPos == new Vector3Int(1, 0, 0))// fix��Ϊ���޸��Ѿ�������С��ͼ��Ϊ��һ�����ϵ��ƻ�������
        {
            ChangeFloor(Posdata);
            return;
        }
        GeneratePlot(centerPos);
        if(exitCanGenerate)
        {
            Vector3Int exitPos = storage.GetRandomPos();
            Debug.Log("��ȡ����pos");
            Instantiate(Exit, grid.CellToLocalInterpolated(exitPos), Quaternion.Euler(Vector3.zero));// ��centerPos���ӳ���
        }
        GenerateObstacle(centerPos);//bug���˳���������ľ׮ ���ڸ��˳�������0.5��0.5��0��*5.12
        if (exitCanGenerate)
        {
            ObstacleTilemap.SetTile(centerPos, null);
            ObstacleTilemap.SetTile(centerPos + new Vector3Int(-1, 0, 0), null);
            ObstacleTilemap.SetTile(centerPos + new Vector3Int(0,-1,0), null);
            ObstacleTilemap.SetTile(centerPos + new Vector3Int(-1, -1, 0), null);
            exitCanGenerate = false;// ������
        }
        GenerateBarrier(centerPos);
        GenerateCorner(centerPos);
        ChangeFloor(Posdata);
        storage.AddPos(centerPos);
    }

    private Vector3Int GetCenterPos(Health.Pos Posdata)
    {
        Vector3Int centerPos = Vector3Int.zero;// ����ұ�
        if (Posdata.dir == Health.Direction.Horizon) { centerPos = Posdata.max + new Vector3Int(-7, 0, 0); }
        if (Posdata.dir == Health.Direction.Vertical) { centerPos = Posdata.max + new Vector3Int(0, -5, 0); }
        if (Posdata.dir == Health.Direction.Horizon)//������һ��
        {
            centerPos = centerPos + new Vector3Int(0, -5, 0);
            if (!FloorTilemap.HasTile(centerPos))
            {
                return centerPos;
            }
            centerPos += new Vector3Int(0, +10, 0);
            if (!FloorTilemap.HasTile(centerPos))
            {
                return centerPos;
            }
        }
        else// ���ŵ� ����ѡһ��
        {
            centerPos = centerPos + new Vector3Int(-7, 0, 0);
            if (!FloorTilemap.HasTile(centerPos))// �еĻ�����һ��
            {
                return centerPos;
            }
            centerPos += new Vector3Int(+14, 0, 0);
            if (!FloorTilemap.HasTile(centerPos))
            {
                return centerPos;
            }
        }
        return new Vector3Int(1,0,0);
    }

    // ��������
    // �������ȡ��������
    public void GetPos(Vector3 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3Int gridPos = grid.WorldToCell(worldPos);
        //Debug.Log(gridPos);
    }

    private void OnDisable()
    {
        Health.UpdataMap -= AddMap;
        Health.AddExit -= GenerateExit;
        if (Instance == this)
        {
            Instance = null;
        }
    }

}