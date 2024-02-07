using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Simple_data",menuName = "Simple_data",order = 1)]

public class Simple_data: ScriptableObject
{
    [Header("BasicData")]
    public float BAS_Speed;
    public float BAS_MaxHealth;
    public float BAS_Atackvalue;
    public float BAS_Attackarea;
    public float BAS_AttackSpeed;
    public int Exp_reward;

}
