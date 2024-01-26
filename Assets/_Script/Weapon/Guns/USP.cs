using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USP : MonoBehaviour,IGuns
{
    public BuffManager Buff;
    public GameObject Bullet;

    [Header("基础参数")]
    public float Bas_Reloading_time;
    public float Bas_Shooting_Interval;
    public float Bas_Damage;
    public float Bas_Magazine_Capacity;
    public int Bas_Penetration_Quantity;

    [Header("Buff")]
    public float BufOn_Reloading_time=1;
    public float BufOn_Shooting_Interval=1;
    public float BufOn_Damage=1;
    public float BufOn_Magazine_Capacity=0;
    public int BufOn_Penetration_Quantity=0;

    [Header("实际参数")]
    public float Fac_Reloading_time;
    public float Fac_Shooting_Interval;
    public float Fac_Damage;
    public float Fac_Magazine_Capacity;
    public int Fac_Penetration_Quantity;

    [Header("动态参数")]
    public float Bullet_Remained;
    public bool Reloading;
    public string Weapon_Name;
    public float last_shoot_time;

    public void DataInitial()//计算实际参数
    {
        BufOn_Reloading_time = Buff.Bufon_Reloading_time;
        BufOn_Shooting_Interval = Buff.Bufon_Shooting_Interval;
        BufOn_Damage = Buff.Bufon_Damage;
        BufOn_Magazine_Capacity = Buff.Bufon_Magazine_Capacity;
        BufOn_Penetration_Quantity = Buff.Bufon_Penetration_Quantity;

        switch (Weapon_Name)
        {
            case "USP":
                Bas_Reloading_time = 1.4f;
                Bas_Shooting_Interval = 0.2f;
                Bas_Damage = 20f;
                Bas_Magazine_Capacity = 14f;
                Bas_Penetration_Quantity = 0;
            break;

            case "AK47":
                Bas_Reloading_time = 2.3f;
                Bas_Shooting_Interval = 0.18f;
                Bas_Damage = 35f;
                Bas_Magazine_Capacity = 25f;
                Bas_Penetration_Quantity = 1;
            break;

            case "M4A1":
                Bas_Reloading_time = 2;
                Bas_Shooting_Interval = 0.15f;
                Bas_Damage = 25f;
                Bas_Magazine_Capacity = 30f;
                Bas_Penetration_Quantity = 0;
            break;

            case "M249":
                Bas_Reloading_time = 4.5f;
                Bas_Shooting_Interval = 0.05f;
                Bas_Damage = 20f;
                Bas_Magazine_Capacity = 100f;
                Bas_Penetration_Quantity = 0;
            break;

            case "Revolver":
                Bas_Reloading_time = 1.4f;
                Bas_Shooting_Interval = 0.4f;
                Bas_Damage = 100f;
                Bas_Magazine_Capacity = 7f;
                Bas_Penetration_Quantity = 2;
            break;

            default: break;

        }

        Fac_Reloading_time = Bas_Reloading_time * BufOn_Reloading_time;
        Fac_Shooting_Interval = Bas_Shooting_Interval * BufOn_Shooting_Interval;
        Fac_Damage = Bas_Damage * BufOn_Damage;
        Fac_Magazine_Capacity = Bas_Magazine_Capacity + BufOn_Magazine_Capacity;
        Fac_Penetration_Quantity = Bas_Penetration_Quantity + BufOn_Penetration_Quantity;

        Bullet_Remained = Fac_Magazine_Capacity;
    }

    // Start is called before the first frame update
    void Start()
    {
        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();
        Weapon_Name = gameObject.name;
        DataInitial();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameObject.name)
        {
            case "USP":
            case "Revolver":
            if (Input.GetMouseButtonDown(0))
            { 
                Shoot(); 
            }
            break;

            default:
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                }
            break;
        }
    }

    public IEnumerator ReLoad(float reloading_time)
    {
        if (!Reloading)
        { 
            Reloading = true;
            //音效播放();
            yield return new WaitForSeconds(reloading_time);
            Bullet_Remained = Fac_Magazine_Capacity;
            Reloading = false; 
        }
    }

    public void Shoot()
    {
        if (Bullet_Remained <= 0) { StartCoroutine(ReLoad(Fac_Reloading_time)); }
        else
        {
            if (!Reloading)
            {
                if (Time.time - last_shoot_time > Fac_Shooting_Interval)
                {
                    last_shoot_time = Time.time;
                    Instantiate(Bullet, transform.position, Quaternion.identity);
                    Bullet_Remained--;
                }
            }
        }
    }

}
