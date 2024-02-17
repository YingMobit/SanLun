using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Health : MonoBehaviour
{
    //这个脚本用来写血量管控
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
    public PBO_data PBO;
    public Simple_data SIM;
    public Tank_data TAN;
    public string tag;
    public Pos Posdata;


    public static int DeadNumOfBarrier;
    public static event Action<float> UpgradeData;
    public static event Action<Pos,GameObject> UpdataMap;


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
        UpgradeData += UpHealth;
        DeadNumOfBarrier = 0;
        tag = gameObject.tag;
        if(DeadNumOfBarrier == 0)
        {
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
        }
        
        curHealth = maxHealth;
    }

    //检查死亡
    public void CheckDead()
    {
        if(curHealth<=0f)
        {
            Debug.Log("Dead!");
            //!!!执行死亡逻辑
            if(tag=="Barrier")
            {
                DeadNumOfBarrier++;
                Destroy(gameObject);
                UpgradeData?.Invoke(50f);
                AddMap();
            }
        }
    }

    //死亡加图
    public void AddMap()
    {
        Tilemap tilemap = gameObject.GetComponent<Tilemap>();
        UpdataMap?.Invoke(Posdata,gameObject);
        Debug.Log("Posdata:" + Posdata.max);
    }
}
