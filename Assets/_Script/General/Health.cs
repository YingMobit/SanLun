using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Health : MonoBehaviour
{
    //����ű�����дѪ���ܿ�
    //���е��˺�Ӧ�ö�����ͨ��Defence����������Health��,Defenceֱ��get������ű���ͨ��TakeDamage��Ѫ|||���������¼�ͨ��tag��Ѫ

    //����
    public static float maxHealth;         //���Ѫ��
    public float curHealth;         //��ǰѪ��
    public PBO_data PBO;
    public Simple_data SIM;
    public Tank_data TAN;
    public string tag;

    public static int DeadNumOfBarrier;
    public static event Action<float> UpgradeData;
    public static event Action<Vector3Int,GameObject> UpdataMap;


    //�ӿں���
    //��Ѫ
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //��Ѫ
    public void UpHealth(float Health)
    {
        maxHealth += Health;
        curHealth += Health;
    }

    //�ڲ�����
    //��ʼ��
    private void Start()
    {
        UpgradeData += UpHealth;
        DeadNumOfBarrier = 0;
        tag = gameObject.tag;
        if(DeadNumOfBarrier == 0)
        {
            switch (tag)
            {
                //���Timer���timer��û�ĳɾ�̬
                case "Barrier":
                    maxHealth = 50f;
                    // ��ʼ��50,�����
                    break;
                case "PBO":
                    maxHealth = Mathf.RoundToInt(PBO.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                case "Simple":
                    maxHealth = Mathf.RoundToInt(SIM.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                case "Tank":
                    maxHealth = Mathf.RoundToInt(PBO.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                default:
                    break;
            }
        }
        
        curHealth = maxHealth;
    }

    //�������
    public void CheckDead()
    {
        if(curHealth<=0f)
        {
            Debug.Log("Dead!");
            //!!!ִ�������߼�
            if(tag=="Barrier")
            {
                DeadNumOfBarrier++;
                Destroy(gameObject);
                UpgradeData?.Invoke(50f);
                AddMap();
            }
        }
    }

    //������ͼ
    public void AddMap()
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int centerPos = new Vector3Int(bounds.x + bounds.size.x / 2, bounds.y + bounds.size.y / 2, 0);

        UpdataMap?.Invoke(centerPos,gameObject);
    }
}
