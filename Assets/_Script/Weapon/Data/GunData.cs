using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GunData",menuName ="GunData",order =1)]
public class GunData : ScriptableObject
{
    public float Bas_Reloading_time;
    public float Bas_Shooting_Interval;
    public float Bas_Damage;
    public float Bas_Magazine_Capacity;
    public int Bas_Penetration_Quantity;
    public float Bas_HitBack;
    public Sprite sprite;
    public AudioClip ShootAudio;
    public AudioClip ReloadAudio;
}
