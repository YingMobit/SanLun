using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    // ����
    public Tilemap FloorTilemap;            // ����
    public TileBase BarrierTile;     // ������Ƭ
    public TileBase[] GrassTiles;           // �ݵ���Ƭ��
    public TileBase FloorTile;          // ��ɫʯש��Ƭ
    public Grid grid;         // ��������
    public int[,] MapData;         // �洢��ͼ����xy���� ����0δ����1����
    public int BarrierNum;          // ����������

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3Int nowPos;          // grid��ǰ����Pos

    // ����

    // ��ʼ
    private void Start()
    {
        // ��յ�ͼ
        ClearChildren(grid.gameObject);
        // ��ʼС��ͼ
        BarrierNum = 1;
        InitialMap();
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

        // ���������岢����
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitialMap()
    {
        // ��ʼ��·����
        nowPos = new Vector3Int(0, 0, 0);

        // ����FloorTilemap,������
        GameObject tilemapObj = new GameObject("FloorTilemap");
        tilemapObj.transform.parent = grid.transform; // ��Tilemap��Ϊgrid��������
        FloorTilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 0;

        //������ʼ��ͼ
        GeneratePlot(nowPos);
        GenerateBarrier(nowPos);
    }

    public void GeneratePlot(Vector3Int centerPos)
    {
        // ����������Ƭ   13*9
        for (int x = -6+ centerPos.x; x < 7+ centerPos.x; x++)
        {
            for (int y = -4+ centerPos.y; y < 5+ centerPos.y; y++)
            {
                TileBase tile = GrassTiles[Random.Range(0, GrassTiles.Length)];
                FloorTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void GenerateBarrier(Vector3Int centerPos)
    {
        bool hasTile;
        //Direction.Up
        hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(0, +9, 0)) || FloorTilemap.HasTile(nowPos + new Vector3Int(0, +5, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier1 = new GameObject("Barrier" + BarrierNum++);// �� 0,+9,0
            Barrier1.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier1, Direction.Up);
        }

        //Direction.Down
        hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(0, -9, 0)) && FloorTilemap.HasTile(nowPos + new Vector3Int(0, -5, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier2 = new GameObject("Barrier" + BarrierNum++);// �� 0,-9,0
            Barrier2.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier2, Direction.Down);
        }

        //Direction.Left
        hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(-13, 0, 0)) && FloorTilemap.HasTile(nowPos + new Vector3Int(-7, 0, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier3 = new GameObject("Barrier" + BarrierNum++);// �� -13,0,0
            Barrier3.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier3, Direction.Left);
        }

        //Direction.Right
        hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(+13, 0, 0)) && FloorTilemap.HasTile(nowPos + new Vector3Int(+7, 0, 0));
        if (!hasTile)// û��С��ͼ��ô��������
        {
            GameObject Barrier4 = new GameObject("Barrier" + BarrierNum++);// �� +13,0,0
            Barrier4.transform.SetParent(grid.transform);
            AddBarrierComponent(Barrier4, Direction.Right);
        }
    }

    private void AddBarrierComponent(GameObject barrier,Direction dir)
    {
        Tilemap tilemap = barrier.AddComponent<Tilemap>();
        barrier.AddComponent<TilemapRenderer>();
        barrier.AddComponent<TilemapCollider2D>().usedByComposite = true;
        barrier.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        barrier.AddComponent<CompositeCollider2D>();
        barrier.tag = "Barrier";
        barrier.AddComponent<Defence>();
        barrier.AddComponent<Health>();
        switch (dir)
        {
            case Direction.Up://-6��+5��0 ������
                Vector3Int setUpPos = nowPos + new Vector3Int(-6, +5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setUpPos, BarrierTile);
                    setUpPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Down://-6,-5,0 ������
                Vector3Int setDownPos = nowPos + new Vector3Int(-6, -5, 0);
                for (int i = 0; i < 13; i++)
                {
                    tilemap.SetTile(setDownPos, BarrierTile);
                    setDownPos += new Vector3Int(+1, 0, 0);
                }
                break;
            case Direction.Left:// -7,-4,0 ������
                Vector3Int setLeftPos = nowPos + new Vector3Int(-7, -4, 0);
                for (int i = 0; i < 9; i++)
                {
                    tilemap.SetTile(setLeftPos, BarrierTile);
                    setLeftPos += new Vector3Int(0, +1, 0);
                }
                break;
            case Direction.Right:
                Vector3Int setRightPos = nowPos + new Vector3Int(+7, -4, 0);
                for (int i = 0; i < 9; i++)
                {
                    tilemap.SetTile(setRightPos, BarrierTile);
                    setRightPos += new Vector3Int(0, +1, 0);
                }
                break;
        }
    }



    // ��������
    // �������ȡ����
    public void GetPos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = grid.WorldToCell(worldPos);
        Debug.Log(gridPos);
    }
}