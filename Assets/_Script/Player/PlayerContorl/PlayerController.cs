
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    public Sprite Death;
    public GameObject Shield_pref;
    public BuffController_Player Buff;
    public ExpCountor ExpCountor;
    public Animator animator;
    public GameObject CurePop;
    public GameObject DeadUI; 
    public HealthStick healthStick;

    private bool point_dir;
    public float angle;
    private Vector3 relative_pos;
    public Vector3 MousePos;

    [Header("基础属性")]
    public float Turningspeed;
    public float Bas_MoveSpeed;
    public int Bas_MaxHealth;

    [Header("Buff")]
    public int Bufon_Health;
    public float Bufon_Speed;

    [Header("实际属性")]
    public float Fac_Speed;
    public float Fac_SuckBloodChance;
    public bool Whether_Generate_Shield;
    public int HealthReward_Value;
    public int Blood_suck_value;
    public int fac_MaxHealth = 100;
    public int Fac_MaxHealth
    { 
        get { return fac_MaxHealth; }
        set
        {
            if (value != fac_MaxHealth)
            { 
                fac_MaxHealth = value;
                HealthDataChanged.Invoke();
            }
        }
    }


    [Header("实时参数")]
    public bool dead;
    public float Last_Be_Attacked_time;
    public GameObject Shield;
    public float Last_Shield_Time;
    public int health = 100;
    public bool Invincible;
    public int Health
    {
        get {return health;}
        set
        {
            if (value != health)
            {
                health = value;
                HealthDataChanged.Invoke();
            }
        }
    }

    public Color health_hell_color;

    [SerializeField]
    [Tooltip("Class")]
    public Rigidbody2D rigidbody;

    [SerializeField]
    [Tooltip("玩家移动")]
    public float move_dir_x;
    public float move_dir_y;

    public event Action HealthDataChanged;

    private void Start()
    {
        DeadUI = GameObject.Find("Canvas").transform.GetChild(6).gameObject;
        ExpCountor = FindAnyObjectByType<ExpCountor>();
        healthStick = FindFirstObjectByType<HealthStick>();
        HealthDataChanged += healthStick.Update;
        animator = gameObject.GetComponentInChildren<Animator>();
        Buff = GameObject.Find("BuffManager").GetComponent<BuffController_Player>();
        Buff.OnDataChanged_Player += DataInitial;
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        DataInitial();
        Health = Fac_MaxHealth;
    }

    void DataInitial()
    {
        Bufon_Health = Buff.Bufon_Health;
        Bufon_Speed = Buff.Bufon_Speed;

        Fac_Speed = Bufon_Speed * Bas_MoveSpeed;
        Fac_MaxHealth = Bufon_Health + Bas_MaxHealth;
        Fac_SuckBloodChance = Buff.bufon_blood_suck_chance;

        HealthReward_Value = Buff.Healthreward_Value;
        Blood_suck_value = Buff.Blood_suck_value;
    }

    private void Update()
    {
        if (!dead&& Whether_Generate_Shield) { GenerateShield(); }
        if (Health <= 0) Dead();
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            Move();
            TurnToPointer();
        }
    }
    void Move()
    {
        move_dir_x = Input.GetAxis("Horizontal");
        move_dir_y = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector2(move_dir_x, move_dir_y).normalized*Fac_Speed;
        if (move_dir_x != 0 || move_dir_y != 0) animator.SetBool("Moving", true);
        else animator.SetBool("Moving", false);
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
        Collider2D Body = GetComponent<Collider2D>();
        Body.enabled = false;
        DeadUI.SetActive(true);
        DeadUI.transform.GetChild(3).GetComponent<Text>().text = "数据统计\r\n昵称："+ PlayerPrefs.GetString("playerName", "Guest" + UnityEngine.Random.Range(1000, 10000).ToString()) + "\r\n积分："+ PlayerPrefs.GetInt("Point", 0) +"\r\n等级："+ PlayerPrefs.GetInt("Level", 1); 
        PlayerPrefs.SetInt("PointState",0);
        PlayerPrefs.SetInt("Level", ExpCountor.CorrentLevel);
    }

    public void SuckBlood()
    { 
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        float chance = (float)rand.NextDouble();
        if (chance <= Fac_SuckBloodChance)
        {
            Debug.Log("吸血");
            Health += Blood_suck_value;
            if (Health > Fac_MaxHealth) Health = Fac_MaxHealth;
            GameObject newCurePop = Instantiate(CurePop,transform.position,Quaternion.identity);
            newCurePop .transform.SetParent(transform);
            TextMesh text = newCurePop.GetComponent<TextMesh>(); 
            text.color = health_hell_color;
            text.characterSize = 2.25f;
            text.text = Blood_suck_value.ToString();
        }
    }

    void GenerateShield()
    {
        if (Shield == null && Time.time - Last_Shield_Time >= 20)
        {
            Shield = Instantiate(Shield_pref,transform.position ,Quaternion.identity);
            Shield.transform.SetParent(transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision .gameObject .CompareTag("HealthReward"))
        {
            Health += HealthReward_Value;
            Health = Health > Fac_MaxHealth? Fac_MaxHealth:Health;
            GameObject newCurePop = Instantiate(CurePop, transform.position, Quaternion.identity);
            newCurePop.transform.SetParent(transform);
            TextMesh text = newCurePop.GetComponent<TextMesh>();
            text.color = health_hell_color;
            text.text = HealthReward_Value.ToString();
            Destroy(collision.gameObject);
        }
    }

}
