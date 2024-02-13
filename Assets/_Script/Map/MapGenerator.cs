using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    // ����
    public Tilemap FloorTilemap;            // ����
    public GameObject[] BarrierPrefabs;     // ����Ԥ������
    public TileBase[] GrassTiles;           // �ݵ���Ƭ��
    public TileBase FloorTile;          // ��ɫʯש��Ƭ
    public Grid grid;         // ��������
    public int[,] MapData;         // �洢��ͼ����xy���� ����0δ����1����
    public int BarrierNum;          // ����������

    private Vector3Int nowPos;          // grid��ǰ����Pos

    // ����

    // ��ʼ
    private void Start()
    {
        // ��յ�ͼ
        ClearChildren(grid.gameObject);
        // ��ʼС��ͼ
        InitialMap();
        // ��ʼ����
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

        // ���������岢����
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InitialMap()
    {
        // ����FloorTilemap,������
        GameObject tilemapObj = new GameObject("FloorTilemap");
        tilemapObj.transform.parent = grid.transform; // ��Tilemap��Ϊgrid��������
        FloorTilemap = tilemapObj.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 0;

        // ����������Ƭ   ��ʼ13*9
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
        GameObject Barrier1 = new GameObject("Barrier" + BarrierNum++);// ��
        GameObject Barrier2 = new GameObject("Barrier" + BarrierNum++);// ��
        GameObject Barrier3 = new GameObject("Barrier" + BarrierNum++);// ��
        GameObject Barrier4 = new GameObject("Barrier" + BarrierNum++);// ��
        AddBarrierComponent(Barrier1,1);
    }

    private void AddBarrierComponent(GameObject barrier,int direction)
    {
        bool hasTile = FloorTilemap.HasTile(nowPos + new Vector3Int(-13,0,0));
        if(!hasTile)//û��С��ͼ��ô��������s
        {
            switch(direction)
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