using System.Collections.Generic;
using UnityEngine;

public class GridStorage
{
    // �洢�ѽ���������λ��
    private List<Vector3Int> Grids = new List<Vector3Int>();

    // ���һ���ѽ���������λ�õ��б���
    public void AddPos(Vector3Int gridPosition)
    {
        if (!Grids.Contains(gridPosition))
        {
            Grids.Add(gridPosition);
        }
    }

    // ���һ�������Ƿ��Ѿ�����
    public bool IsGridsPos(Vector3Int gridPosition)
    {
        return Grids.Contains(gridPosition);
    }

    // ���ѽ������������Ƴ�һ��λ��
    public bool RemovePos(Vector3Int gridPosition)
    {
        return Grids.Remove(gridPosition);
    }

    // �����ȡһ���ѽ���������λ��
    public Vector3Int GetRandomPos()
    {
        if (Grids.Count == 0)
        {
            throw new System.InvalidOperationException("���Դӿ��б��л�ȡԪ��");
        }
        int maxIndex = Mathf.FloorToInt(Grids.Count * (2.0f / 3.0f)); // ����2/3λ�õ�����������ȡ��ȷ��������Ч
        Debug.Log("��ȡrandompos");
        int randomIndex = Random.Range(0, maxIndex); // ������������������б��ǰ2/
        return Grids[randomIndex];
    }

    // ��ȡ�ѽ�������������
    public int CountPos()
    {
        return Grids.Count;
    }

    // ����ѽ����������б�
    public void ClearPos()
    {
        Grids.Clear();
    }
}
