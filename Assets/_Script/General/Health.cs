using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    //这个脚本用来写血量管控
    //所有的伤害应该都是先通过Defence再由他进入Health的,Defence直接get到这个脚本并通过TakeDamage扣血|||后续还有事件通过tag加血

    //声明
    public float maxHealth;         //最大血量
    public float curHealth;         //当前血量
    public PBO_data PBO;
    public Simple_data SIM;
    public Tank_data TAN;
    public string tag;

    public static event Action UpgradeData;

    //接口函数
    //扣血
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //内部函数
    //初始化
    private void Start()
    {
        tag = gameObject.tag;
        switch (tag)
        {
            //这边Timer里的timer还没改成静态
            case "Barrier":
                maxHealth = 50f;
                // 初始给50,后面调
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

    //检查死亡
    public void CheckDead()
    {
        if(curHealth<0f)
        {
            Debug.Log("Dead!");
            //!!!执行死亡逻辑
        }
    }

}
