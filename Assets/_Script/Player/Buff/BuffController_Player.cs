using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BuffController_Player : MonoBehaviour
{  

    public int bufon_Health;
    public float bufon_Speed = 1;
    public float bufon_blood_suck_chance;
    public int healthreward_value =5;
    public int blood_suck_value;

    public event Action OnDataChanged_Player; 


    public int Blood_suck_value
    {
        get { return blood_suck_value; }
        set
        {
            if (blood_suck_value != value)
            { 
                blood_suck_value = value;
                OnDataChanged_Player.Invoke(); 
            }
        }
    }
    public int Healthreward_Value
    {
        get { return healthreward_value; }
        set 
        { 
            if (healthreward_value != value) 
            {
                healthreward_value = value;
                OnDataChanged_Player.Invoke();
            }
        }
    }

    public int Bufon_Health
    {
        get { return bufon_Health; }
        set 
        {
            if (value != bufon_Health)
            { 
                bufon_Health = value;
                OnDataChanged_Player.Invoke();
            }
        }
    }

    public float Bufon_Speed
    {
        get { return bufon_Speed; }
        set
        {
            if (value != bufon_Speed)
            { 
                bufon_Speed = value;
                OnDataChanged_Player.Invoke();
            }
        }
    }

    public float Bufon_blood_suck_chance
    {
        get { return bufon_blood_suck_chance; }
        set 
        {
            if (value != bufon_blood_suck_chance)
            { 
                bufon_blood_suck_chance = value;
                OnDataChanged_Player.Invoke();
            }
        }
    }
}
