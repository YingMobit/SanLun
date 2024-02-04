using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    public Sprite Death;
    public BuffController_Player Buff;
    public SpriteRenderer sprite_renderer;//playerbody的sprite_renderer

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

    [Header("玩家状态")]
    public bool dead;
    public float Health;

    [SerializeField]
    [Tooltip("Class")]
    public Rigidbody2D rigidbody;

    [SerializeField]
    [Tooltip("玩家移动")]
    public float move_dir_x;
    public float move_dir_y;

    private void Awake()
    {
        Buff = GameObject.Find("BuffManager").GetComponent<BuffController_Player>();
        Buff.OnDataChanged_Player += DataInitial;
        sprite_renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
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

        Health = Fac_MaxHealth;
    }

    private void Update()
    {
        if (!dead)
        {
            Move();
            TurnToPointer();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            Debug.Log("BeAttacked");
        }
    }

    void Dead()
    { 
        dead = true;
        sprite_renderer.sprite = Death;
    }
}
