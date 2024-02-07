using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BuffController_Player : MonoBehaviour
{  

    public int bufon_Health;
    public float bufon_Speed = 1;

    public event Action OnDataChanged_Player; 

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
}
