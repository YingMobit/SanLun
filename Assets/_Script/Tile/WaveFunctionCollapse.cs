using UnityEngine;
using System.Collections.Generic;

public class WaveFunctionCollapse
{
    private string[] inputTiles; // 输入数据，表示地图块的不同类型
    private int mapSize; // 地图大小
    private int[,] map; // 生成的地图数据

    public WaveFunctionCollapse(string[] inputTiles, int mapSize)
    {
        this.inputTiles = inputTiles;
        this.mapSize = mapSize;
        this.map = new int[mapSize, mapSize];
    }

    public bool Collapse()
    {
        // 初始化地图
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                map[x, y] = -1; // 表示未确定的地图块
            }
        }

        // 随机选择一个起始地图块
        int startX = Random.Range(0, mapSize);
        int startY = Random.Range(0, mapSize);
        map[startX, startY] = Random.Range(0, inputTiles.Length);

        // 迭代扩展地图
        while (!IsMapComplete())
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (map[x, y] == -1)
                    {
                        List<int> possibleTiles = new List<int>();

                        // 检查周围地图块的约束
                        for (int i = 0; i < inputTiles.Length; i++)
                        {
                            if (IsValidTile(x, y, i))
                            {
                                possibleTiles.Add(i);
                            }
                        }

                        if (possibleTiles.Count > 0)
                        {
                            map[x, y] = possibleTiles[Random.Range(0, possibleTiles.Count)];
                        }
                        else
                        {
                            // 如果无法找到合适的地图块，则重新生成地图
                            return false;
                        }
                    }
                }
            }
        }

        return true; // 表示波函数塌缩成功
    }

    // 检查某个地图块是否符合约束
    private bool IsValidTile(int x, int y, int tileIndex)
    {
        if (x > 0 && map[x - 1, y] == tileIndex) return false; // 左侧地图块相同
        if (x < mapSize - 1 && map[x + 1, y] == tileIndex) return false; // 右侧地图块相同
        if (y > 0 && map[x, y - 1] == tileIndex) return false; // 上方地图块相同
        if (y < mapSize - 1 && map[x, y + 1] == tileIndex) return false; // 下方地图块相同
        return true;
    }

    // 检查地图是否已经完整生成
    private bool IsMapComplete()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (map[x, y] == -1)
                {
                    return false; // 存在未确定的地图块
                }
            }
        }
        return true; // 所有地图块已确定
    }

    public int[,] GetResult()
    {
        return map;
    }
}
