using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GrassTileMapGenerator : MonoBehaviour
{
    public TileBase[] grassTiles; // 用于生成地图的草地地图块
    public int mapSizeX = 8; // 地图横向大小
    public int mapSizeY = 10; // 地图纵向大小

    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        // 获取 Tilemap 组件
        tilemap = gameObject.AddComponent<Tilemap>();
        tilemapCollider = gameObject.AddComponent<TilemapCollider2D>();

        // 创建 CompositeCollider2D，并将其设置为 Tilemap 的碰撞体
        CompositeCollider2D compositeCollider = gameObject.AddComponent<CompositeCollider2D>();
        compositeCollider.isTrigger = false;

        // 设置 Tilemap 的大小
        tilemap.size = new Vector3Int(mapSizeX, mapSizeY, 0);

        // 生成地图
        GenerateMap();
    }

    void GenerateMap()
    {
        // 遍历地图的每个位置，随机选择一个草地地图块进行填充
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = grassTiles[Random.Range(0, grassTiles.Length)];
                tilemap.SetTile(position, tile);
            }
        }

        // 更新 TilemapCollider2D
        tilemapCollider.enabled = false;
        tilemapCollider.enabled = true;
    }
}
