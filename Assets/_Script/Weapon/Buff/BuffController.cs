using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{

    public BuffManager_Weapon buffManager;

    // Start is called before the first frame update
    void Start()
    {
        buffManager = GameObject.Find("BuffManager").GetComponent<BuffManager_Weapon>();
        //StartCoroutine(Test());
    }

    public void Update_bufon_Reloading_time(float magnification)//magnification为倍率，用小数表示eg:20%->magnification = 0.2
    {
        buffManager.Bufon_Reloading_time -= buffManager.Bufon_Reloading_time * magnification;
    }
    public void Update_bufon_Shooting_Interval(float magnification)//magnification为倍率，用小数表示eg:20%->magnification = 0.2
    {
        buffManager.Bufon_Shooting_Interval -= buffManager.Bufon_Shooting_Interval * magnification;
    }
    public void Update_bufon_Damage(float magnification)//magnification为倍率，用小数表示eg:20%->magnification = 0.2
    {
        buffManager.Bufon_Damage += buffManager.Bufon_Damage * magnification;
    }
    public void Update_bufon_Magazine_Capacity(int gain)//gain为增加量
    {
        buffManager.Bufon_Magazine_Capacity += gain;
    }
    public void Update_bufon_Penetration_Quantity(int gain)
    {
        buffManager.Bufon_Penetration_Quantity += gain;
    }

    IEnumerator Test()
    {
        Update_bufon_Reloading_time(0.2f);
        Update_bufon_Shooting_Interval(0.2f);
        Update_bufon_Damage(0.2f);
        Update_bufon_Magazine_Capacity(20);
        Update_bufon_Penetration_Quantity(20);
        Update_bufon_Reloading_time(0.2f);
        Update_bufon_Shooting_Interval(0.2f);
        Update_bufon_Damage(0.2f);
        Update_bufon_Magazine_Capacity(20);
        Update_bufon_Penetration_Quantity(20);

        yield return null;
    }
}
