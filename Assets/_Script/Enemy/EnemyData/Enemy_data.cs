using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="EnmeyData",menuName ="EnemyData",order =1)]
public class Enemy_data : ScriptableObject
{
    [Header("BasicData")]
    public float BAS_Speed;
    public float BAS_MaxHealth;
    public float BAS_Atackvalue;
    public float BAS_Attackarea;
    public float BAS_AttackSpeed;
    public int Exp_reward;
    public float Health_gain_bytime;
    public float AttackValue_gain_bytime;
}
