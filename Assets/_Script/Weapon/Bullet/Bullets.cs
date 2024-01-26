using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public Rigidbody2D player_rigidbody;
    public Transform transform;
    public GameObject Weapon;
    public USP weapon_;

    public string fromGun;

    public float bullet_damage;
    [Range(10, 50)] public float bullet_speed;
    public int bullet_penetration_times;
    public int bullet_life_time;

    public float life_time;

    [Header("指向鼠标")]
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
        if (Weapon != null) weapon_ = Weapon.GetComponent<USP>(); else Debug.Log("未找到武器");
        fromGun = GameObject.Find("Player").name;
        rigidbody = GetComponent<Rigidbody2D>();
        TurnToPoint();
        DataInitial();
        Debug.Log("Bullet初始化完成");
    }

    // Update is called once per frame
    void Update()
    {
        AutoDead();
    }

    void DataInitial()
    {
        if (weapon_ != null)
        {
            bullet_penetration_times = weapon_.Fac_Penetration_Quantity;
            bullet_damage = weapon_.Fac_Damage;
            rigidbody.velocity = bullet_speed * transform.up + new Vector3(player_rigidbody.velocity.x, player_rigidbody.velocity.y, 0);
            Debug.Log("?"+bullet_speed*Mathf.Sin(angle/Mathf.Deg2Rad)+bullet_speed*Mathf.Cos(angle / Mathf.Deg2Rad));
        }
        else Debug.Log("未找到武器脚本");
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
    }

    void TurnToPoint()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        angle = Mathf.Atan2(relative_pos.x, relative_pos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
}
