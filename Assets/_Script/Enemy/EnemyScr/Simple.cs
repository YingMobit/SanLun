using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple : MonoBehaviour
{
    public Timer timer_scr;
    public Rigidbody2D rigidbody;
    public GameObject Player;
    public Simple_data BAS_data;
    public SpriteRenderer sprite_renderer;
    public Sprite Attackarea;

    [Header("FactData")]
    public float FAC_Speed;
    public float FAC_MaxHealth;
    public float FAC_Atackvalue;
    public float FAC_Attackarea;

    [Header("RealTimeData")]
    public float Health;
    public bool Attacking;
    public Vector3 Chasing_dir;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        timer_scr = GameObject.Find("Timer").GetComponent<Timer>();
        DataInitial();

    }

    // Update is called once per frame
    void Update()
    {
        Chase();
        if (!Attacking && Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea) Attack();
        if (Health <= 0) Death();
    }

    void DataInitial()
    {
        FAC_Speed = BAS_data.BAS_Speed;
        FAC_MaxHealth = Mathf.RoundToInt(BAS_data.BAS_MaxHealth + 0.3f * timer_scr.timer);
        FAC_Atackvalue = Mathf.RoundToInt(BAS_data.BAS_Atackvalue + 0.05f * timer_scr.timer);

    }

    void Attack()
    {
        Attacking = true;
        Vector3 Attack_pos;//生成攻击指示器的位置
        //Attack_pos = ;
        Attacking = false;
    }

    void Chase()
    {
        Chasing_dir = Player.transform.position - transform.position;
        rigidbody.velocity = Chasing_dir * FAC_Speed;
        if (transform .position.x < Player.transform.position.x ) { transform.rotation = Quaternion.Euler(0,0,180); }
    }

    void Death()
    {

    }
}
