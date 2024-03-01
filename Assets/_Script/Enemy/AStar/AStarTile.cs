using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TileType
{ 
    Ground,Obastacle
}

public class AStarTile
{
    public float toGoalCost;
    public float toStartCost;
    public float GeneralCost;

    public AStarTile father;

    public int x;
    public int y;

    public TileType type;

    public AStarTile(int x,int y,TileType type)
    { 
        this.x = x;
        this.y = y; 
        this.type = type;
    }
}
