using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] tiles;
    public int intervalX = 1;
    public int intervalY = 1;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x += intervalX)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y += intervalY)
            {
                int randomIndex = Random.Range(0, tiles.Length);
                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[randomIndex]);
            }
        }

        // ¸üÐÂÅö×²Ìå
        tilemap.RefreshAllTiles();
    }
}
