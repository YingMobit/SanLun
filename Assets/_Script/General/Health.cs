using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    //����ű�����дѪ���ܿ�
    //���е��˺�Ӧ�ö�����ͨ��Defence����������Health��,Defenceֱ��get������ű���ͨ��TakeDamage��Ѫ|||���������¼�ͨ��tag��Ѫ

    //����
    public float maxHealth;         //���Ѫ��
    public float curHealth;         //��ǰѪ��
    public PBO_data PBO;
    public Simple_data SIM;
    public Tank_data TAN;
    public string tag;

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
        tag = gameObject.tag;
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
        curHealth = maxHealth;
    }

    //�������
    public void CheckDead()
    {
        if(curHealth<0f)
        {
            Debug.Log("Dead!");
            //!!!ִ�������߼�
        }
    }

}
