using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindingManager
{
    private static PathFindingManager instance;

    public PathFindingManager Instance
    {
        get
        { 
            if (instance == null)
                instance = new PathFindingManager();
            return instance;
        }
    }

    public AStarTile[,] InitialTile(Tilemap ground,Tilemap obstacle)
    {
        BoundsInt groundbounds = ground.cellBounds;
        BoundsInt obstaclebounds = obstacle.cellBounds;
        AStarTile[,] Tiles = new AStarTile[groundbounds.xMax - groundbounds.xMin, groundbounds.yMax - groundbounds.yMin];
        for (int x = groundbounds.xMin; x < groundbounds.xMax; x++)
        {
            for (int y = groundbounds.yMin;y < groundbounds.yMax ;y++ )
            {
                if (ground.GetTile(new Vector3Int(x, y, 0)))
                {
                    AStarTile newTie = new AStarTile(x - groundbounds.xMin, y - groundbounds.yMin, TileType.Ground);
                    Tiles[newTie.x, newTie.y] = newTie;
                }
            }
        }//初始化可行区域

        for (int x = obstaclebounds.xMin; x < obstaclebounds.xMax; x++)
        {
            for (int y = obstaclebounds.yMin; y < obstaclebounds.yMax; y++)
            {
                if (obstacle.GetTile(new Vector3Int(x, y, 0)))
                {
                    AStarTile newTie = new AStarTile(x - obstaclebounds.xMin, y - obstaclebounds.yMin, TileType.Obastacle);
                    Tiles[newTie.x, newTie.y] = newTie;
                }
            }
        }//初始化不可行区域
        return Tiles;
    }

    public List<AStarTile> FindPath(Vector3Int StartPos,Vector3Int GoalPos, AStarTile[,] Tiles, Tilemap ground, Tilemap obstacle) 
    {
        List<AStarTile> Openlist = null;
        List<AStarTile> Closelist = null;
        BoundsInt groundbounds = ground.cellBounds;
        BoundsInt obstaclebounds = obstacle.cellBounds;
        StartPos -= new Vector3Int(groundbounds.xMin, groundbounds.yMin, 0);
        GoalPos -= new Vector3Int(groundbounds.xMin,groundbounds.yMin,0);
        if (Tiles[StartPos.x, StartPos.y].type == TileType.Ground && Tiles[GoalPos.x, GoalPos.y].type == TileType.Ground) 
        {
            AStarTile[] around;
            around = GetAround(StartPos,Tiles);
            int count = 0;
            foreach (AStarTile tile in around)
            {
                if (tile.type != TileType.Obastacle && !Openlist.Contains(tile) && !Closelist.Contains(tile))
                {
                    tile.father = Tiles[StartPos.x, StartPos.y];
                    if (count <= 3)
                    {
                        tile.toStartCost = 1 + tile.father.toStartCost;
                    }
                    else tile.toStartCost = 1.4f + tile.father.toStartCost;
                    tile.toGoalCost = Mathf.Abs(tile.x - GoalPos.x) + Mathf.Abs(tile.y - GoalPos.y);
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
            Closelist.Add(closestTile);
            Openlist.Remove(closestTile);
            if (closestTile.x == GoalPos.x && closestTile.y == GoalPos.y) return Closelist;
            else return FindPath(new Vector3Int(closestTile.x,closestTile.y,0),GoalPos,Tiles,ground,obstacle);
        }
        else return null;
    }

    AStarTile[] GetAround( Vector3Int StartPos, AStarTile[,] Tiles)
    {
        AStarTile[] around = new AStarTile[8];
        around[0] = Tiles[StartPos.x + 1, StartPos.y];
        around[1] = Tiles[StartPos.x - 1, StartPos.y];
        around[2] = Tiles[StartPos.x , StartPos.y + 1];
        around[3] = Tiles[StartPos.x , StartPos.y - 1];
        around[4] = Tiles[StartPos.x + 1, StartPos.y+1];
        around[5] = Tiles[StartPos.x + 1, StartPos.y-1];
        around[6] = Tiles[StartPos.x - 1, StartPos.y - 1];
        around[7] = Tiles[StartPos.x - 1, StartPos.y + 1];
        return around;
    }
}
