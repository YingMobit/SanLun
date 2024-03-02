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
    public Pos Posdata;         // 存了位置信息
    public static int BarrierDestroyCount;          // 破坏的屏障计数
    public static int ExitGenerateCount;          // 生成的出口计数

    // 事件
    public static event Action<float> UpgradeData;          // 加血
    public static event Action<Pos,GameObject> UpdataMap;           // 加图
    public static event Action AddExit;         // 加出口
    // 屏障加血逻辑
    public static float BarrierHealthIncrease = 1f;         // 增血速率初始为1
    public const int BarrierDestroyThreshold = 4;           // 触发增血逻辑的屏障破坏阈值
    public const int BarrierHealthIncreaseDecayThreshold = 20;           // 每破坏4*5个屏障减少增血速率
    public const float BarrierHealthIncreaseDecayRate = 0.9f;           // 增血速率的衰减率
    // 出口逻辑
    private float exitProbability = 0.0f; // 当前累积的出口生成概率
    private float exitProbabilityIncrement = 0.000375f; // 初始概率增幅
    private float exitProbabilityIncrementFactor = 1.0f; // 概率增幅因子
    private const float Pmax = 0.03f; // 最大生成概率
    private const int MaxBarriersBeforeGuaranteedExit = 1000; // 保底出口的屏障破坏上限



    static Health()// 静态构造函数,当类第一次调用时激活
    {
        BarrierDestroyCount = 0;//但整型默认初始到0
        ExitGenerateCount = 0;
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
                case "Barrier":
                    maxHealth = 1f;
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
            exitProbabilityIncrementFactor *= 1.025f; // 增加概率增幅因子
            exitProbabilityIncrement *= exitProbabilityIncrementFactor; // 更新概率增幅
        }
        // 更新概率
        exitProbability += exitProbabilityIncrement;
        exitProbability = Mathf.Min(exitProbability, Pmax); // 保证概率不超过最大值 但实际超不过从期望上
        // 检查是否生成出口
        if (UnityEngine.Random.value < exitProbability)
        {
            AddExit?.Invoke();// 生成出口
            ExitGenerateCount++;
            ResetExitProbability();
            return;
        }
        if (BarrierDestroyCount >= 2) // bug：暂时修改
        {
            AddExit?.Invoke();// 生成出口
            ExitGenerateCount++;
            BarrierDestroyCount = 0; // 重置屏障破坏计数
        }
    }*/
    private void OnBarrierDead()
    {
        BarrierDestroyCount++;
        // 检查是否生成出口
        if (PlayerPrefs.GetInt("Level", 1) >= 25 && (PlayerPrefs.GetInt("Level", 1) - 25) / 10 == ExitGenerateCount)//  每10级一个出口
        {
            AddExit?.Invoke();// 生成出口
            Debug.Log("生成出口");
            ExitGenerateCount++;
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
        UpdataMap?.Invoke(Posdata,gameObject);
    }

    // 清概率数据
    private void ResetExitProbability()
    {
        // 重置出口生成概率计算的变量
        exitProbability = 0.0f;
        exitProbabilityIncrement = 0.000375f; // 重置为初始增幅
        exitProbabilityIncrementFactor = 1.0f; // 重置增幅因子
    }
}
