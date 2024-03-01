using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public Rigidbody2D player_rigidbody;
    public Transform transform;
    public GameObject Weapon;
    public WeaponSystem weapon_scr;
    public BuffManager_Weapon buff;

    public int bullet_damage;
    [Range(10, 50)] public float bullet_speed;
    public int bullet_penetration_times;
    public int bullet_life_time;
    public float Critical_Hit_Chance;
    public int Critical_Hit_Damage;
    public bool isCriticalHit;
    public float life_time;
    public float HitBackForce;

    [Header("÷∏œÚ Û±Í")]
    public Vector3 MousePos;
    public Vector3 relative_pos;
    public float angle =90;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        buff = GameObject .Find("BuffManager").GetComponent<BuffManager_Weapon>();
        player_rigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        cam = Camera.main;
        transform = GetComponent<Transform>();
        Weapon = GameObject.FindGameObjectWithTag("Weapon");
        if (Weapon != null) weapon_scr = Weapon.GetComponent<WeaponSystem>(); else Debug.Log("Œ¥’“µΩŒ‰∆˜");
        rigidbody = GetComponent<Rigidbody2D>();
        TurnToPoint();
        DataInitial();
    }

    // Update is called once per frame
    void Update()
    {
        AutoDead();
    }

    void DataInitial()
    {
        if (weapon_scr != null)
        {
            bullet_penetration_times = weapon_scr.Fac_Penetration_Quantity;
            bullet_damage = weapon_scr.Fac_Damage;
            HitBackForce = weapon_scr.Fac_HitBackForce;
            rigidbody.velocity = bullet_speed * transform.up + new Vector3(player_rigidbody.velocity.x, player_rigidbody.velocity.y, 0);
            Critical_Hit_Chance = buff.Bufon_Critical_Hit_Chance; Critical_Hit_Damage = buff.Bufon_Critical_Hit_Damage;
            WhetherCriticalHit();
        }
        else Debug.Log("Œ¥’“µΩŒ‰∆˜Ω≈±æ");
    }

    void WhetherCriticalHit()
    { 
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        float chance = (float)rand.NextDouble();
        if (chance <= Critical_Hit_Chance) {isCriticalHit = true; bullet_damage = bullet_damage * Critical_Hit_Damage;} 
    }

    void AutoDead()
    {
        life_time += Time.deltaTime;
        if (life_time >= bullet_life_time|| bullet_penetration_times < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bullet_penetration_times--;
        }
        if(collision.gameObject.CompareTag("Barrier"))
        {
            collision.gameObject.GetComponent<Defence>().DealDamage(1f);
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Obstacle")) Destroy(gameObject);
    }

    void TurnToPoint()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        angle = Mathf.Atan2(relative_pos.x, relative_pos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
}
