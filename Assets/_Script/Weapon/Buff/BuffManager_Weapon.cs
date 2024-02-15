using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BuffManager_Weapon: MonoBehaviour
{
    public WeaponSystem WeaponSystem;
    public  string weapon_name;
    public  float bufon_reloading_time = 1f;
    public  float bufon_Shooting_Interval = 1f;
    public  float bufon_Damage = 1f;
    public  int bufon_Magazine_Capacity = 0;
    public  int bufon_Penetration_Quantity = 0;

    public event Action OnDataChanged_Weapon; // 定义数据改变事件

    public string Weapon_Name
    { 
        get { return weapon_name; }
        set
        {
            if (value != weapon_name)
            {
                weapon_name = value;
                OnDataChanged_Weapon?.Invoke();
            }
        }
    }

    public float Bufon_Reloading_time
    {
        set
        {
            if (value != bufon_reloading_time)
            {
                bufon_reloading_time = value;
                OnDataChanged_Weapon?.Invoke(); // 触发数据改变事件
            }
        }
        get { return bufon_reloading_time; }
       
    }

    public float Bufon_Shooting_Interval
    {
        get { return bufon_Shooting_Interval; }
        set
        {
            if (value != bufon_Shooting_Interval)
            {
                bufon_Shooting_Interval = value;
                OnDataChanged_Weapon?.Invoke(); // 触发数据改变事件
            }
        }
    }

    public float Bufon_Damage
    {
        get { return bufon_Damage; }
        set
        {
            if (value != bufon_Damage)
            {
                bufon_Damage = value;
                OnDataChanged_Weapon?.Invoke(); // 触发数据改变事件
            }
        }
    }

    public int Bufon_Magazine_Capacity
    {
        get { return bufon_Magazine_Capacity; }
        set
        {
            if (value != bufon_Magazine_Capacity)
            {
                bufon_Magazine_Capacity = value;
                OnDataChanged_Weapon?.Invoke(); // 触发数据改变事件
            }
        }
    }

    public int Bufon_Penetration_Quantity
    {
        get { return bufon_Penetration_Quantity; }
        set
        {
            if (value != bufon_Penetration_Quantity)
            {
                bufon_Penetration_Quantity = value;
                OnDataChanged_Weapon?.Invoke(); // 触发数据改变事件
            }
        }
    }

    public float Bufon_Critical_Hit_Chance =0.1f;

    public int Bufon_Critical_Hit_Damage = 2;
    // Start is called before the first frame update
    void Start()
    {
        WeaponSystem = GameObject.Find("USP").GetComponent<WeaponSystem>();
        Bufon_Reloading_time = 1f;
        Bufon_Shooting_Interval = 1f;
        Bufon_Damage = 1f;
        Bufon_Magazine_Capacity = 0;
        Bufon_Penetration_Quantity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Weapon_Name = WeaponSystem.Weapon_Name;
    }

}
