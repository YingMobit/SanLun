using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCountor : MonoBehaviour
{
    public Enemy_data simple_Data;
    public Enemy_data pBO_Data;
    public Enemy_data tank_Data;

    public static int CorrentLevel;
    public static int MaxLevel;
    public int CorrentExp;
    public int CorrentLevelExp;//当前等级经验值上限

    public event Action LevelUPed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("MaxLevel",25);
        PlayerPrefs.SetInt("CorrentLevel", 1);
        CorrentLevelExp = 1200;
        CorrentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        LevelUP();
    }

    void LevelUP()
    {
        if (PlayerPrefs.GetInt("CorrentLevel",1) < PlayerPrefs.GetInt("MaxLevel",25) && CorrentExp >= CorrentLevelExp)
        {
            CorrentExp -= CorrentLevelExp;
            CorrentLevel++;
            PlayerPrefs.SetInt("CorrentLevel",CorrentLevel);
            CorrentLevelExp = 1200 + 400 * (CorrentLevel - 1);
            LevelUPed.Invoke();
        }
    }
}
