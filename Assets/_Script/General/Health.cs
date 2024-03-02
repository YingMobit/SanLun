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
    //����ű�����дѪ���ܿ� �����������ϵ�
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
    public Enemy_data Enemy;
    public string tag;
    public Pos Posdata;         // ����λ����Ϣ
    public static int BarrierDestroyCount;          // �ƻ������ϼ���
    public static int ExitGenerateCount;          // ���ɵĳ��ڼ���

    // �¼�
    public static event Action<float> UpgradeData;          // ��Ѫ
    public static event Action<Pos,GameObject> UpdataMap;           // ��ͼ
    public static event Action AddExit;         // �ӳ���
    // ���ϼ�Ѫ�߼�
    public static float BarrierHealthIncrease = 1f;         // ��Ѫ���ʳ�ʼΪ1
    public const int BarrierDestroyThreshold = 4;           // ������Ѫ�߼��������ƻ���ֵ
    public const int BarrierHealthIncreaseDecayThreshold = 20;           // ÿ�ƻ�4*5�����ϼ�����Ѫ����
    public const float BarrierHealthIncreaseDecayRate = 0.9f;           // ��Ѫ���ʵ�˥����
    // �����߼�
    private float exitProbability = 0.0f; // ��ǰ�ۻ��ĳ������ɸ���
    private float exitProbabilityIncrement = 0.000375f; // ��ʼ��������
    private float exitProbabilityIncrementFactor = 1.0f; // ������������
    private const float Pmax = 0.03f; // ������ɸ���
    private const int MaxBarriersBeforeGuaranteedExit = 1000; // ���׳��ڵ������ƻ�����



    static Health()// ��̬���캯��,�����һ�ε���ʱ����
    {
        BarrierDestroyCount = 0;//������Ĭ�ϳ�ʼ��0
        ExitGenerateCount = 0;
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
        if(BarrierDestroyCount == 0)
        {
            switch (tag)
            {
                case "Barrier":
                    maxHealth = 1f;
                    break;
                default:
                    break;
            }
        }
        curHealth = maxHealth;
    }

    //�������
    private void CheckDead()
    {
        if(curHealth<=0f)
        {
            Debug.Log("Dead!");
            //!!!ִ�������߼�
            if(tag=="Barrier")
            {
                OnBarrierDead();
                Destroy(gameObject);
                AddHealth();
                AddMap();
            }
        }
    }

    /*private void OnBarrierDead()
    {
        BarrierDestroyCount++;
        if (BarrierDestroyCount % 10 == 0)
        {
            exitProbabilityIncrementFactor *= 1.025f; // ���Ӹ�����������
            exitProbabilityIncrement *= exitProbabilityIncrementFactor; // ���¸�������
        }
        // ���¸���
        exitProbability += exitProbabilityIncrement;
        exitProbability = Mathf.Min(exitProbability, Pmax); // ��֤���ʲ��������ֵ ��ʵ�ʳ�������������
        // ����Ƿ����ɳ���
        if (UnityEngine.Random.value < exitProbability)
        {
            AddExit?.Invoke();// ���ɳ���
            ExitGenerateCount++;
            ResetExitProbability();
            return;
        }
        if (BarrierDestroyCount >= 2) // bug����ʱ�޸�
        {
            AddExit?.Invoke();// ���ɳ���
            ExitGenerateCount++;
            BarrierDestroyCount = 0; // ���������ƻ�����
        }
    }*/
    private void OnBarrierDead()
    {
        BarrierDestroyCount++;
        // ����Ƿ����ɳ���
        if (PlayerPrefs.GetInt("Level", 1) >= 25 && (PlayerPrefs.GetInt("Level", 1) - 25) / 10 == ExitGenerateCount)//  ÿ10��һ������
        {
            AddExit?.Invoke();// ���ɳ���
            Debug.Log("���ɳ���");
            ExitGenerateCount++;
        }
    }

    // ������
    // ������Ѫ
    private void AddHealth()
    {
        BarrierDestroyCount++;
        if (BarrierDestroyCount % BarrierDestroyThreshold == 0)
        {
            BarrierHealthIncrease *= BarrierHealthIncreaseDecayRate;// ÿ�ƻ�4�����ϣ��������ӵ�Ѫ������0.9
        }

        if (BarrierDestroyCount % BarrierHealthIncreaseDecayThreshold == 0)
        {
            BarrierHealthIncrease *= BarrierHealthIncreaseDecayRate;// ÿ�ƻ�4*5�����ϣ�������Ѫ����
        }

        UpgradeData?.Invoke(BarrierHealthIncrease);
    }
    // ������ͼ
    private void AddMap()
    {
        UpdataMap?.Invoke(Posdata,gameObject);
    }

    // ���������
    private void ResetExitProbability()
    {
        // ���ó������ɸ��ʼ���ı���
        exitProbability = 0.0f;
        exitProbabilityIncrement = 0.000375f; // ����Ϊ��ʼ����
        exitProbabilityIncrementFactor = 1.0f; // ������������
    }
}
