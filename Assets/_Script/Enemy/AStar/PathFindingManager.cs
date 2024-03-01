using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindingManager:MonoBehaviour
{

    List<AStarTile> Openlist = new List<AStarTile>();
    List<AStarTile> Closelist = new List<AStarTile>();

    public AStarTile[,] InitialTile(Tilemap ground,Tilemap obstacle)
    {
        Debug.Log("InitialTile");
        BoundsInt groundbounds = ground.cellBounds;
        BoundsInt obstaclebounds = obstacle.cellBounds;
        AStarTile[,] Tiles = new AStarTile[groundbounds.xMax - groundbounds.xMin, groundbounds.yMax - groundbounds.yMin];
        for (int x = groundbounds.xMin; x < groundbounds.xMax; x++)
        {
            for (int y = groundbounds.yMin;y < groundbounds.yMax ;y++ )
            {
                if (ground.GetTile(new Vector3Int(x, y, 0)))
                {
                    AStarTile newTie = new AStarTile(x, y, TileType.Ground);
                    Tiles[newTie.x - groundbounds.xMin, newTie.y - groundbounds.yMin] = newTie;
                }
            }
        }//初始化可行区域

        for (int x = obstaclebounds.xMin; x < obstaclebounds.xMax; x++)
        {
            for (int y = obstaclebounds.yMin; y < obstaclebounds.yMax; y++)
            {
                if (obstacle.GetTile(new Vector3Int(x, y, 0)))
                {
                    AStarTile newTie = new AStarTile(x, y, TileType.Obastacle);
                    Tiles[newTie.x - obstaclebounds.xMin, newTie.y - obstaclebounds.yMin] = newTie;
                }
            }
        }//初始化不可行区域
        return Tiles;
    }

    public List<AStarTile> FindPath(Vector3Int StartPos,Vector3Int GoalPos, AStarTile[,] Tiles, Tilemap ground, Tilemap obstacle) 
    {
        Debug.Log("进入寻路");
        BoundsInt groundbounds = ground.cellBounds;
        BoundsInt obstaclebounds = obstacle.cellBounds;
        StartPos -= new Vector3Int(groundbounds.xMin, groundbounds.yMin, 0);
        GoalPos -= new Vector3Int(groundbounds.xMin,groundbounds.yMin,0);
        if (Tiles[StartPos.x, StartPos.y].type == TileType.Ground && Tiles[GoalPos.x, GoalPos.y].type == TileType.Ground)
        {
            AStarTile[] around;
            around = GetAround(StartPos, Tiles,groundbounds.size.x,groundbounds.size.y);
            int count = 0;
            foreach (AStarTile tile in around)
            {
                if (tile != null && tile.type != TileType.Obastacle && !Openlist.Contains(tile) && !Closelist.Contains(tile))
                {
                    tile.father = Tiles[StartPos.x, StartPos.y];
                    if (count <= 3)
                    {
                        tile.toStartCost = 1 + tile.father.toStartCost;
                    }
                    else tile.toStartCost = 1.4f + tile.father.toStartCost;
                    tile.toGoalCost = Mathf.Abs(tile.x - groundbounds.xMin - GoalPos.x) + Mathf.Abs(tile.y - groundbounds.yMin - GoalPos.y);
                    tile.GeneralCost = tile.toStartCost + tile.toGoalCost;
                    Openlist.Add(tile);
                }
                count++;
            }
            AStarTile closestTile = Openlist[0];
            foreach (AStarTile tile in Openlist)
            {
                if (tile.GeneralCost < closestTile.GeneralCost) closestTile = tile;
            }
            if (!Closelist.Contains(closestTile)) Closelist.Add(closestTile);
            if (Openlist.Contains(closestTile)) Openlist.Remove(closestTile);
            if (closestTile.x - groundbounds.xMin == GoalPos.x && closestTile.y - groundbounds.yMin == GoalPos.y) { Debug.Log("寻路完成"); return Closelist;}
            else
            {
                Debug.Log("寻路递归"); return FindPath(new Vector3Int(closestTile.x, closestTile.y, 0), GoalPos + new Vector3Int(groundbounds.xMin, groundbounds.yMin, 0), Tiles, ground, obstacle);
            } 
        }
        else return null;
    }

    AStarTile[] GetAround( Vector3Int StartPos, AStarTile[,] Tiles,int Tile_x_max,int Tile_y_max)
    {
        AStarTile[] around = new AStarTile[8];
        if (StartPos.x + 1 <= Tile_x_max - 1)                                    around[0] = Tiles[StartPos.x + 1, StartPos.y];
        if (StartPos.x - 1 >= 0)                                                 around[1] = Tiles[StartPos.x - 1, StartPos.y];
        if (StartPos.y + 1 <= Tile_y_max - 1)                                    around[2] = Tiles[StartPos.x , StartPos.y + 1];
        if (StartPos.y - 1 >= 0)                                                 around[3] = Tiles[StartPos.x , StartPos.y - 1];
        if (StartPos.x + 1 <= Tile_x_max - 1&& StartPos.y + 1 <= Tile_y_max - 1) around[4] = Tiles[StartPos.x + 1, StartPos.y+1];
        if (StartPos.x + 1 <= Tile_x_max - 1&& StartPos.y - 1 >= 0)              around[5] = Tiles[StartPos.x + 1, StartPos.y-1];
        if (StartPos.x - 1 >= 0&& StartPos.y - 1 >= 0)                           around[6] = Tiles[StartPos.x - 1, StartPos.y - 1];
        if (StartPos.x - 1 >= 0&& StartPos.y + 1 <= Tile_y_max - 1)              around[7] = Tiles[StartPos.x - 1, StartPos.y + 1];
        return around;
    }
}
