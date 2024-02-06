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

    public float bullet_damage;
    [Range(10, 50)] public float bullet_speed;
    public int bullet_penetration_times;
    public int bullet_life_time;

    public float life_time;

    [Header("÷∏œÚ Û±Í")]
    public Vector3 MousePos;
    public Vector3 relative_pos;
    public float angle =90;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
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
            rigidbody.velocity = bullet_speed * transform.up + new Vector3(player_rigidbody.velocity.x, player_rigidbody.velocity.y, 0);
        }
        else Debug.Log("Œ¥’“µΩŒ‰∆˜Ω≈±æ");
    }

    void AutoDead()
    {
        life_time += Time.deltaTime;
        if (life_time >= bullet_life_time|| bullet_penetration_times < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            bullet_penetration_times--;
        }
        if(collision.gameObject.tag=="Barrier")
        {
            bullet_penetration_times--;
            collision.gameObject.GetComponent<Defence>().DealDamage(50f);
        }
    }

    void TurnToPoint()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        angle = Mathf.Atan2(relative_pos.x, relative_pos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
}
