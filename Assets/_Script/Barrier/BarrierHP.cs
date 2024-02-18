using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHP : MonoBehaviour
{
    //����ű�����д���ϵ�Ѫ�����������¼����ܲ��������ƻ�ǽ����
    //ʵ��ű�

    //����
    public float maxHealth;         //���Ѫ��
    public float curHealth;         //��ǰѪ��
    public Enemy_data PBO;
    public Enemy_data SIM;
    public Enemy_data TAN;
    public string tag;

    public int DeadNum;         //������
    public static event Action UpgradeData;


    //�ӿں���
    //��Ѫ
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //�ڲ�����
    //��ʼ��
    private void Start()
    {
        UpgradeData += UpLevel;
        /*
        tag = gameObject.tag;
        switch (tag)
        {
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
        curHealth = maxHealth;
        */
    }

    //�������
    public void CheckDead()
    {
        if (curHealth < 0f)
        {
            Debug.Log("Dead!");
            Destroy(gameObject);
            //!!!ִ�������߼�
            UpgradeData?.Invoke();
        }
    }

    //��Ѫ//�����ƻ�������
    public void UpLevel()
    {
        DeadNum++;
        maxHealth += 50f;
    }
}
