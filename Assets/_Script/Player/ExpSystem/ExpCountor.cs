using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCountor : MonoBehaviour
{
    public Simple simple;
    public PBO pBO;
    public Tank tank;
    public Simple_data simple_Data;
    public PBO_data pBO_Data;
    public Tank_data tank_Data;

    public int CorrentLevel;
    public int MaxLevel;
    public int CorrentExp;
    public int CorrentLevelExp;//当前等级经验值上限

    public event Action LevelUPed;

    // Start is called before the first frame update
    void Start()
    {
        CorrentLevelExp = 1000;
        CorrentLevel = 1;
        simple = FindFirstObjectByType<Simple>();
        pBO = FindFirstObjectByType<PBO>();
        tank = FindFirstObjectByType<Tank>();
        simple.Die += SimpleReward;
        pBO.Die += PBOReward;
        tank.Die += TankReward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelUP()
    {
        if (CorrentExp >= CorrentLevelExp)
        { 
            CorrentExp -= CorrentLevelExp;
            CorrentLevel++;
            CorrentLevelExp = 1000 + 350 * (CorrentLevel - 1);
            LevelUPed.Invoke();
        }
    }

    void SimpleReward() 
    { CorrentExp += simple_Data.Exp_reward;if (CorrentLevel<MaxLevel) LevelUP();}

    void PBOReward() 
    { CorrentExp += pBO_Data.Exp_reward; if (CorrentLevel < MaxLevel) LevelUP(); }

    void TankReward() 
    { CorrentExp += tank_Data.Exp_reward; if (CorrentLevel < MaxLevel) LevelUP(); }

}
