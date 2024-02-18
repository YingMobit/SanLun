using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Health : MonoBehaviour
{
    //����ű�����дѪ���ܿ�
    //���е��˺�Ӧ�ö�����ͨ��Defence����������Health��,Defenceֱ��get������ű���ͨ��TakeDamage��Ѫ|||���������¼�ͨ��tag��Ѫ

    //����
    public struct Pos
    {
        public Vector3Int max;// ע�����max��Ȼ����һ����λ
        public Direction dir;
    }

    public enum Direction
    {
        Horizon,
        Vertical
    }

    public static float maxHealth;         //���Ѫ��
    public float curHealth;         //��ǰѪ��
    public Enemy_data PBO;
    public Enemy_data SIM;
    public Enemy_data TAN;
    public string tag;
    public Pos Posdata;


    public static int DeadNumOfBarrier;
    public static event Action<float> UpgradeData;
    public static event Action<Pos,GameObject> UpdataMap;

    static Health()// ��̬���캯��,�����һ�ε���ʱ����
    {
        DeadNumOfBarrier = 0;//������Ĭ�ϳ�ʼ��0
        //UpgradeData += UpHealth;
    }


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
        if (UpgradeData == null)// || !UpgradeData.GetInvocationList().Contains(UpHealth))// ����Ϊɶ
        {
            UpgradeData += UpHealth;
        }
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
        UpdataMap?.Invoke(Posdata,gameObject);
        Debug.Log("Posdata:" + Posdata.max);
    }
}
