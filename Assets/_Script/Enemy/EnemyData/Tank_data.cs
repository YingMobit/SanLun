using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tank_data", menuName = "Tank_data", order = 1)]
public class Tank_data : ScriptableObject
{
    [Header("BasicData")]
    public float BAS_Speed;
    public float BAS_MaxHealth;
    public float BAS_Atackvalue;
    public float BAS_Attackarea;
    public int Exp_reward;
}
