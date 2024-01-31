using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    //����ű����ڻ��ܻ����˺��������������Ա�ͨ�����Էŵ���general�����ӵ�����ͨ������ű�������ű��ٵ���ȥִ�ж�HP�ű��Ŀ���/��Ѫ
    //���ڹ���ֻ�л���|||�����������Ż���������Ѫ��|||������������˺�����ͻ��ܣ�����������ڻ�����һ����������������ԣ���������һЩװ���ṩ�������ԣ������ǿ��Ա仯�ģ���Ҫһ���ӿں���ȥ������
    //����һ���¼��ӿڿ���ͨ��tag�ı���ֵ

    //����
    public float ShieldStrength;            //����ǿ�ȣ�����ֱ�ӵ���
    public float DamageReduction;           //�˺����⣺�ٷֱȼ����˺�
                                            //�߼����ȹ�����Ȼ��ۻ���
    
    //�ӿں���

    //���»���ֵ���˺�����ٷֱ�
    public void UpgradeData(float StrengthIncrease, float ReductionIncrease)
    {
        ShieldStrength += StrengthIncrease;
        DamageReduction += ReductionIncrease;
    }

    //������������ⲿԭʼ�˺�ֵ
    public void DealDamage(float Damage)
    {
        float TotalDamage = CalculateTotalDamage(Damage);
        if(ShieldStrength > 0f)
        {
            float RealDamage = Mathf.Max(0f, TotalDamage - ShieldStrength);//��������
            ShieldStrength = Mathf.Max(0f, ShieldStrength - TotalDamage);//���㻤��ʣ��
            ImplementDamage(RealDamage);
        }
        else
        {
            ImplementDamage(TotalDamage);
        }
    }

    //�ڲ�����

    //�˺���������ܴ����˺�
    public float CalculateTotalDamage(float Damage)
    {
        return Damage * (1f - DamageReduction);
    }

    //ʵʩ�����˺�
    public void ImplementDamage(float Damage)
    {
        //���������ʵ�˺���Health
        gameObject.GetComponent<Health>().TakeDamage(Damage);
    }
}
