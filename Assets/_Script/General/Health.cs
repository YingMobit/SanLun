using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //����ű�����дѪ���ܿ�
    //���е��˺�Ӧ�ö�����ͨ��Defence����������Health��,Defenceֱ��get������ű���ͨ��TakeDamage��Ѫ|||���������¼�ͨ��tag��Ѫ

    //����
    public float maxHealth;         //���Ѫ��
    public float curHealth;         //��ǰѪ��

    //�ӿں���
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //�ڲ�����
    //��ʼ��
    private void Start()
    {
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
