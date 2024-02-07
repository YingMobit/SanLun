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
        // 清空旧地图
        tilemap.ClearAllTiles();
        barrierTilemap.ClearAllTiles();

        // 生成地图
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
                // 随机选择一个草地瓦片
                TileBase tile = grassTiles[Random.Range(0, grassTiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    void GenerateBarriers()
    {
        Vector3Int mapSize = new Vector3Int(13, 9, 0);

        // 生成四个角落的地面瓦片
        barrierTilemap.SetTile(new Vector3Int(-1, -1, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(mapSize.x, -1, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(-1, mapSize.y, 0), floorTile);
        barrierTilemap.SetTile(new Vector3Int(mapSize.x, mapSize.y, 0), floorTile);

        // 生成四条墙
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
