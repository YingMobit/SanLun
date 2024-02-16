using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    public Sprite Death;
    public GameObject Shield_pref;
    public BuffController_Player Buff;
    public Animator animator;

    private bool point_dir;
    public float angle;
    private Vector3 relative_pos;
    public Vector3 MousePos;

    [Header("基础属性")]
    public float Turningspeed;
    public float Bas_MoveSpeed;
    public float Bas_MaxHealth;

    [Header("Buff")]
    public float Bufon_Health;
    public float Bufon_Speed;

    [Header("实际属性")]
    public float Fac_MaxHealth;
    public float Fac_Speed;
    public float Fac_SuckBloodChance;
    public bool Whether_Generate_Shield;

    [Header("实时参数")]
    public bool dead;
    public float Health;
    public float Last_Be_Attacked_time;
    public GameObject Shield;
    public float Last_Shield_Time;

    [SerializeField]
    [Tooltip("Class")]
    public Rigidbody2D rigidbody;

    [SerializeField]
    [Tooltip("玩家移动")]
    public float move_dir_x;
    public float move_dir_y;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Buff = GameObject.Find("BuffManager").GetComponent<BuffController_Player>();
        Buff.OnDataChanged_Player += DataInitial;
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        DataInitial();
    }

    void DataInitial()
    {
        Bufon_Health = Buff.Bufon_Health;
        Bufon_Speed = Buff.Bufon_Speed;

        Fac_Speed = Bufon_Speed * Bas_MoveSpeed;
        Fac_MaxHealth = Bufon_Health + Bas_MaxHealth;
        Fac_SuckBloodChance = Buff.bufon_blood_suck_chance;

        Health = Fac_MaxHealth;
    }

    private void Update()
    {
        if (!dead)
        {
            Move();
            TurnToPointer();
            if (Whether_Generate_Shield) GenerateShield();
        }
        if (Health <= 0) Dead();
    }
    void Move()
    {
        move_dir_x = Input.GetAxis("Horizontal");
        move_dir_y = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector2(move_dir_x* Fac_Speed, move_dir_y*Fac_Speed);
    }

    void TurnToPointer()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        point_dir = relative_pos.x > 0 ? true : false;
        if (!point_dir) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Dead()
    { 
        dead = true;
        animator.SetBool("Dead",true);
    }

    public void SuckBlood()
    { 
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        float chance = (float)rand.NextDouble();
        if (chance <= Fac_SuckBloodChance)
        {
            Debug.Log("吸血");
            Health += 5;
            if (Health > Fac_MaxHealth) Health = Fac_MaxHealth;
        }
    }

    void GenerateShield()
    {
        if (Shield == null && Time.time - Last_Shield_Time >= 20)
        {
            Collider2D Body = GetComponent<Collider2D>();
            Body.enabled = false;
            Shield = Instantiate(Shield_pref,transform.position ,Quaternion.identity);
            Shield.transform.SetParent(transform);
        }
    }
}
