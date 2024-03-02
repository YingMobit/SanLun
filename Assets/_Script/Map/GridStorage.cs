using System.Collections.Generic;
using UnityEngine;

public class GridStorage
{
    // 存储已解锁的网格位置
    private List<Vector3Int> Grids = new List<Vector3Int>();

    // 添加一个已解锁的网格位置到列表中
    public void AddPos(Vector3Int gridPosition)
    {
        if (!Grids.Contains(gridPosition))
        {
            Grids.Add(gridPosition);
        }
    }

    // 检查一个网格是否已经解锁
    public bool IsGridsPos(Vector3Int gridPosition)
    {
        return Grids.Contains(gridPosition);
    }

    // 从已解锁的网格中移除一个位置
    public bool RemovePos(Vector3Int gridPosition)
    {
        return Grids.Remove(gridPosition);
    }

    // 随机获取一个已解锁的网格位置
    public Vector3Int GetRandomPos()
    {
        if (Grids.Count == 0)
        {
            throw new System.InvalidOperationException("尝试从空列表中获取元素");
        }
        int maxIndex = Mathf.FloorToInt(Grids.Count * (2.0f / 3.0f)); // 计算2/3位置的索引，向下取整确保索引有效
        Debug.Log("获取randompos");
        int randomIndex = Random.Range(0, maxIndex); // 随机索引现在限制在列表的前2/
        return Grids[randomIndex];
    }

    // 获取已解锁的网格数量
    public int CountPos()
    {
        return Grids.Count;
    }

    // 清空已解锁的网格列表
    public void ClearPos()
    {
        Grids.Clear();
    }
}
