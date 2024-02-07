using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap barrierTilemap;
    public TileBase[] grassTiles;
    public TileBase barrierTile;
    public TileBase floorTile;


    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        // ��վɵ�ͼ
        tilemap.ClearAllTiles();
        barrierTilemap.ClearAllTiles();

        // ���ɵ�ͼ
        GenerateTiles();
        GenerateBarriers();
    }

    void GenerateTiles()
    {
        Vector3Int mapSize = new Vector3Int(13, 9, 0);
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                // ���ѡ��һ���ݵ���Ƭ
                TileBase tile = grassTiles[Random.Range(0, grassTiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    void GenerateBarriers()
    {
        Vector3Int mapSize = new Vector3Int(13, 9, 0);

        // �����ĸ�����ĵ�����Ƭ
        barrierTilemap.SetTile(new Vector3Int(-1, -1, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(mapSize.x, -1, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(-1, mapSize.y, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(mapSize.x, mapSize.y, 0), floorTile);

        // ��������ǽ
        GenerateWall(new Vector3Int(-1, 0, 0), new Vector3Int(1, mapSize.y, 0));
        GenerateWall(new Vector3Int(mapSize.x, 0, 0), new Vector3Int(1, mapSize.y, 0));
        GenerateWall(new Vector3Int(0, mapSize.y, 0), new Vector3Int(mapSize.x, 1, 0));
        GenerateWall(new Vector3Int(0, -1, 0), new Vector3Int(mapSize.x, 1, 0));
    }

    void GenerateWall(Vector3Int start, Vector3Int size)
    {
        for (int x = start.x; x < start.x + size.x; x++)
        {
            for (int y = start.y; y < start.y + size.y; y++)
            {
                if (x == start.x || x == start.x + size.x - 1 || y == start.y || y == start.y + size.y - 1)
                {
                    barrierTilemap.SetTile(new Vector3Int(x, y, 0), barrierTile);
                }
            }
        }
    }
}
