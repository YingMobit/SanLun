using UnityEngine;
using System.Collections.Generic;

public class WaveFunctionCollapse
{
    private string[] inputTiles; // �������ݣ���ʾ��ͼ��Ĳ�ͬ����
    private int mapSize; // ��ͼ��С
    private int[,] map; // ���ɵĵ�ͼ����

    public WaveFunctionCollapse(string[] inputTiles, int mapSize)
    {
        this.inputTiles = inputTiles;
        this.mapSize = mapSize;
        this.map = new int[mapSize, mapSize];
    }

    public bool Collapse()
    {
        // ��ʼ����ͼ
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                map[x, y] = -1; // ��ʾδȷ���ĵ�ͼ��
            }
        }

        // ���ѡ��һ����ʼ��ͼ��
        int startX = Random.Range(0, mapSize);
        int startY = Random.Range(0, mapSize);
        map[startX, startY] = Random.Range(0, inputTiles.Length);

        // ������չ��ͼ
        while (!IsMapComplete())
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (map[x, y] == -1)
                    {
                        List<int> possibleTiles = new List<int>();

                        // �����Χ��ͼ���Լ��
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
                            // ����޷��ҵ����ʵĵ�ͼ�飬���������ɵ�ͼ
                            return false;
                        }
                    }
                }
            }
        }

        return true; // ��ʾ�����������ɹ�
    }

    // ���ĳ����ͼ���Ƿ����Լ��
    private bool IsValidTile(int x, int y, int tileIndex)
    {
        if (x > 0 && map[x - 1, y] == tileIndex) return false; // ����ͼ����ͬ
        if (x < mapSize - 1 && map[x + 1, y] == tileIndex) return false; // �Ҳ��ͼ����ͬ
        if (y > 0 && map[x, y - 1] == tileIndex) return false; // �Ϸ���ͼ����ͬ
        if (y < mapSize - 1 && map[x, y + 1] == tileIndex) return false; // �·���ͼ����ͬ
        return true;
    }

    // ����ͼ�Ƿ��Ѿ���������
    private bool IsMapComplete()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (map[x, y] == -1)
                {
                    return false; // ����δȷ���ĵ�ͼ��
                }
            }
        }
        return true; // ���е�ͼ����ȷ��
    }

    public int[,] GetResult()
    {
        return map;
    }
}
