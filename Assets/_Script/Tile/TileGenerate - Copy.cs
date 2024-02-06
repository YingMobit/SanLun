using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GrassTileMapGenerator : MonoBehaviour
{
    public TileBase[] grassTiles; // �������ɵ�ͼ�Ĳݵص�ͼ��
    public int mapSizeX = 8; // ��ͼ�����С
    public int mapSizeY = 10; // ��ͼ�����С

    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        // ��ȡ Tilemap ���
        tilemap = gameObject.AddComponent<Tilemap>();
        tilemapCollider = gameObject.AddComponent<TilemapCollider2D>();

        // ���� CompositeCollider2D������������Ϊ Tilemap ����ײ��
        CompositeCollider2D compositeCollider = gameObject.AddComponent<CompositeCollider2D>();
        compositeCollider.isTrigger = false;

        // ���� Tilemap �Ĵ�С
        tilemap.size = new Vector3Int(mapSizeX, mapSizeY, 0);

        // ���ɵ�ͼ
        GenerateMap();
    }

    void GenerateMap()
    {
        // ������ͼ��ÿ��λ�ã����ѡ��һ���ݵص�ͼ��������
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = grassTiles[Random.Range(0, grassTiles.Length)];
                tilemap.SetTile(position, tile);
            }
        }

        // ���� TilemapCollider2D
        tilemapCollider.enabled = false;
        tilemapCollider.enabled = true;
    }
}
