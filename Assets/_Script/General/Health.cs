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
    //这个脚本用来写血量管控 后来成了屏障的
    //所有的伤害应该都是先通过Defence再由他进入Health的,Defence直接get到这个脚本并通过TakeDamage扣血|||后续还有事件通过tag加血

    //声明
    public struct Pos
    {
        public Vector3Int max;// 注意这个max仍然大了一个单位
        public Direction dir;
    }

    public enum Direction
    {
        Horizon,
        Vertical
    }

    public static float maxHealth;         //最大血量
    public float curHealth;         //当前血量
    public Enemy_data Enemy;
    public string tag;
    public Pos Posdata;

    //事件
    public static event Action<float> UpgradeData;          // 加血
    public static event Action<Pos,GameObject> UpdataMap;           // 加图
    // 屏障加血逻辑
    public static float BarrierHealthIncrease = 1f;         // 增血速率初始为1
    public static int BarrierDestroyCount = 0;          // 破坏的屏障计数
    public const int BarrierDestroyThreshold = 4;           // 触发增血逻辑的屏障破坏阈值
    public const int BarrierHealthIncreaseDecayThreshold = 20;           // 每破坏4*5个屏障减少增血速率
    public const float BarrierHealthIncreaseDecayRate = 0.9f;           // 增血速率的衰减率


    static Health()// 静态构造函数,当类第一次调用时激活
    {
        BarrierDestroyCount = 0;//但整型默认初始到0
        //UpgradeData += UpHealth;
    }


    //接口函数
    //扣血
    public void TakeDamage(float Damage)
    {
        curHealth = Mathf.Max(0f, curHealth -= Damage);
        CheckDead();
    }

    //加血
    public void UpHealth(float Health)
    {
        maxHealth += Health;
        curHealth += Health;
    }

    //内部函数
    //初始化
    private void Start()
    {
        if (UpgradeData == null)// || !UpgradeData.GetInvocationList().Contains(UpHealth))// 这是为啥
        {
            UpgradeData += UpHealth;
        }
        tag = gameObject.tag;
        if(BarrierDestroyCount == 0)
        {
            switch (tag)
            {
                //这边Timer里的timer还没改成静态
                case "Barrier":
                    maxHealth = 1f;
                    // 初始给50,后面调
                    break;
                case "PBO":
                    maxHealth = Mathf.RoundToInt(Enemy.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                case "Simple":
                    maxHealth = Mathf.RoundToInt(Enemy.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                case "Tank":
                    maxHealth = Mathf.RoundToInt(Enemy.BAS_MaxHealth + 0.3f * Timer.timer);
                    break;
                default:
                    break;
            }
        }
        
        curHealth = maxHealth;
    }

    //检查死亡
    private void CheckDead()
    {
        if(curHealth<=0f)
        {
            Debug.Log("Dead!");
            //!!!执行死亡逻辑
            if(tag=="Barrier")
            {
                BarrierDestroyCount++;
                Destroy(gameObject);
                AddHealth();
                AddMap();
            }
        }
    }

    // 死亡后
    // 死亡加血
    private void AddHealth()
    {
        BarrierDestroyCount++;
        if (BarrierDestroyCount % BarrierDestroyThreshold == 0)
        {
            BarrierHealthIncrease *= BarrierHealthIncreaseDecayRate;// 每破坏4个屏障，屏障增加的血量乘以0.9
        }

        if (BarrierDestroyCount % BarrierHealthIncreaseDecayThreshold == 0)
        {
            BarrierHealthIncrease *= BarrierHealthIncreaseDecayRate;// 每破坏4*5个屏障，减少增血速率
        }

        UpgradeData?.Invoke(BarrierHealthIncrease);
    }
    // 死亡加图
    private void AddMap()
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        UpdataMap?.Invoke(Posdata,gameObject);
        Debug.Log("Posdata:" + Posdata.max);
    }
}
