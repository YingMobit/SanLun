using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHP : MonoBehaviour
{
    //这个脚本用来写屏障的血量，里面用事件接受并传出被破坏墙数量
    //实验脚本

    //声明
    public float maxHealth;         //最大血量
    public float curHealth;         //当前血量
    public Enemy_data PBO;
    public Enemy_data SIM;
    public Enemy_data TAN;
    public string tag;

    public int DeadNum;         //死亡数
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
        UpgradeData += UpLevel;
        /*
        tag = gameObject.tag;
        switch (tag)
        {
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
        */
    }

    //检查死亡
    public void CheckDead()
    {
        if (curHealth < 0f)
        {
            Debug.Log("Dead!");
            Destroy(gameObject);
            //!!!执行死亡逻辑
            UpgradeData?.Invoke();
        }
    }

    //加血//根据破坏数增加
    public void UpLevel()
    {
        DeadNum++;
        maxHealth += 50f;
    }
}
