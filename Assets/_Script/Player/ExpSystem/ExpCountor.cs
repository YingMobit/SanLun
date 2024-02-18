using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCountor : MonoBehaviour
{
    public Enemy_data simple_Data;
    public Enemy_data pBO_Data;
    public Enemy_data tank_Data;

    public int CorrentLevel;
    public int MaxLevel;
    public int CorrentExp;
    public int CorrentLevelExp;//当前等级经验值上限

    public event Action LevelUPed;

    // Start is called before the first frame update
    void Start()
    {
        CorrentLevelExp = 2000;
        CorrentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        LevelUP();
    }

    void LevelUP()
    {
        if (CorrentLevel < MaxLevel && CorrentExp >= CorrentLevelExp)
        {
            CorrentExp -= CorrentLevelExp;
            CorrentLevel++;
            CorrentLevelExp = 2000 + 2000 * (CorrentLevel - 1);
            LevelUPed.Invoke();
        }
    }
}
