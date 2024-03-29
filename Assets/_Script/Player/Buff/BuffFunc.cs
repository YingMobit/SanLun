using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFunc : MonoBehaviour
{

    public BuffController_Player buff_Player;
    public BuffManager_Weapon buff_Weapon;
    public PlayerController playerController;
    public GameObject Weapon;
    public GameObject BuffBackGround;
    

    // Start is called before the first frame update
    void Start()
    {
        buff_Player = GameObject.Find("BuffManager").GetComponent<BuffController_Player>();
        buff_Weapon = GameObject.Find("BuffManager").GetComponent<BuffManager_Weapon>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Weapon = GameObject.Find("PlayerBody").transform .GetChild(0).gameObject;
    }

    public void Buff_0() { buff_Player.Bufon_Speed += 0.1f; CloseScene(); }
    public void Buff_1() { buff_Player.Bufon_Health += 20;playerController.Health_Value += 20; CloseScene(); }
    public void Buff_2() { buff_Player.Bufon_blood_suck_chance += 0.05f; CloseScene(); }
    public void Buff_3() { playerController.Whether_Generate_Shield = true; CloseScene(); }
    public void Buff_4() { buff_Weapon.Bufon_Damage += 0.2f; CloseScene(); }
    public void Buff_5() { buff_Weapon.Bufon_Shooting_Interval = buff_Weapon.Bufon_Shooting_Interval * 0.8f; CloseScene(); }
    public void Buff_6() { buff_Weapon.Bufon_Reloading_time = buff_Weapon.Bufon_Reloading_time * 0.8f; CloseScene(); }
    public void Buff_7() { buff_Weapon.Bufon_Magazine_Capacity = buff_Weapon.Bufon_Magazine_Capacity += 20; CloseScene(); }
    public void Buff_8() { buff_Weapon.Bufon_Critical_Hit_Chance += 0.1f; CloseScene(); }
    public void Buff_9() { buff_Weapon.Bufon_Critical_Hit_Damage ++; CloseScene(); }
    public void Buff_10() { buff_Weapon.Bufon_Penetration_Quantity++; CloseScene(); }
    public void Buff_11() { Weapon.name = "M4A1"; CloseScene(); }
    public void Buff_12() { Weapon.name = "AK47"; CloseScene(); }
    public void Buff_13() { Weapon.name = "M249"; CloseScene(); }
    public void Buff_14() { Weapon.name = "Revolver"; CloseScene(); }
    public void Buff_15() { buff_Player.Healthreward_Value += 5; CloseScene(); }
    public void Buff_16() { buff_Player.Blood_suck_value += 5; CloseScene(); }

    public void Buff_17() { buff_Weapon.Bufon_HitBackForce += 0.2f;CloseScene(); }

    void CloseScene()
    {
        for (int i = 0; i < BuffBackGround.transform.childCount; i++) { GameObject child = BuffBackGround.transform.GetChild(i).gameObject; child.SetActive(false); }
        BuffBackGround.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
