using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager: MonoBehaviour
{
    public float Bufon_Reloading_time =1f;
    public float Bufon_Shooting_Interval =1f;
    public float Bufon_Damage =1f;
    public float Bufon_Magazine_Capacity =0;
    public int Bufon_Penetration_Quantity =0;

    // Start is called before the first frame update
    void Start()
    {
        Bufon_Reloading_time = 1f;
        Bufon_Shooting_Interval = 1f;
        Bufon_Damage = 1f;
        Bufon_Magazine_Capacity = 0f;
        Bufon_Penetration_Quantity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
