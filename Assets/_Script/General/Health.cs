using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //这个脚本用来写血量管控
    //所有的伤害应该都是先通过Defence再由他进入Health的,Defence直接get到这个脚本并通过TakeDamage扣血|||后续还有事件通过tag加血

    //声明
    public float maxHealth;         //最大血量
    public float curHealth;         //当前血量

    //接口函数
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //内部函数
    //初始化
    private void Start()
    {
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
