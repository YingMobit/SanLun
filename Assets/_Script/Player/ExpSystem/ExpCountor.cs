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
    public int CorrentExp;
    public int CorrentLevelExp;//��ǰ�ȼ�����ֵ����

    public event Action LevelUPed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Level", 1);
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
        if ( CorrentExp >= CorrentLevelExp)
        {
            CorrentExp -= CorrentLevelExp;
            CorrentLevel++;
            PlayerPrefs.SetInt("Level",CorrentLevel);
            Debug.Log(PlayerPrefs.GetInt("Level", 1));
            CorrentLevelExp = 1200 + 400 * (CorrentLevel - 1);
            LevelUPed.Invoke();
        }
    }
}
