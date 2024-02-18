using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GneneralEnemy : MonoBehaviour
{
    public Timer timer_scr;
    public Rigidbody2D rigidbody;
    public Animator animator;
    public GameObject Player;
    public GameObject Attackarea;
    public GameObject DamagePop;
    public Enemy_data BAS_data;
    public ExpCountor exp;
    public Collider2D Body;
    public Collider2D Body2;
    public PlayerController Player_scr;

    [Header("FactData")]
    public float FAC_Speed;
    public float FAC_MaxHealth;
    public int FAC_Atackvalue;
    public float FAC_Attackarea;

    [Header("RealTimeData")]
    public float Health;
    public bool Attacking;
    public bool Dead;
    public Vector3 Chasing_dir;
    GameObject new_Attackarea;

    public event Action Die;

    public void Start()
    {
        Player = GameObject.Find("Player");
        Body = transform.GetChild(0).GetComponent<Collider2D>();
        Body2 = GetComponent<Collider2D>();
        Player_scr = Player.GetComponent<PlayerController>();
        Die += Player_scr.SuckBlood;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        exp = GameObject.Find("ExpManager").GetComponent<ExpCountor>();
        DataInitial();
    }

    // Update is called once per frame
    public void Update()
    {
        if (!Attacking && !Dead) Chase();
        if (!Attacking && (Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea) && !Dead) StartCoroutine(Attack());
        if (Health <= 0) StartCoroutine(Death());
    }

    void DataInitial()
    {
        FAC_Speed = BAS_data.BAS_Speed;
        FAC_MaxHealth = Mathf.RoundToInt(BAS_data.BAS_MaxHealth + BAS_data.Health_gain_bytime * Timer.timer);
        FAC_Atackvalue = Mathf.RoundToInt(BAS_data.BAS_Atackvalue + BAS_data.AttackValue_gain_bytime* Timer.timer);
        FAC_Attackarea = BAS_data.BAS_Attackarea;
        Health = FAC_MaxHealth;
    }

    IEnumerator Attack()
    {
        Attacking = true;
        Body.enabled = false;
        animator.SetBool("Attacking", Attacking);
        rigidbody.velocity = Vector3.zero;
        Vector3 Relative_pos = Player.transform.position - transform.position;
        float angle;//���ɹ���ָʾ���ķ���
        angle = Mathf.Atan2(Relative_pos.x, Relative_pos.y) * Mathf.Rad2Deg;
        Debug.Log(angle);
        new_Attackarea = Instantiate(Attackarea, transform.position + new Vector3(0, BAS_data.BAS_Attackarea, 0), Quaternion.identity);
        new_Attackarea.transform.RotateAround(transform.position, Vector3.forward, -angle);
        new_Attackarea.transform.SetParent(transform);
        new_Attackarea.transform.localScale = new Vector3(BAS_data.BAS_Attackarea / 3, BAS_data.BAS_Attackarea / 3, 1);
        new_Attackarea.transform.SetParent(transform);
        Attackaera attackaera = new_Attackarea.AddComponent<Attackaera>();
        attackaera.AttackValue = FAC_Atackvalue;
        yield return new WaitForSeconds(BAS_data.BAS_AttackSpeed);
        Collider2D Attackerea_collider = new_Attackarea.GetComponent<PolygonCollider2D>();
        Attackerea_collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(new_Attackarea);
        Body.enabled = true;
        Attacking = false;
        animator.SetBool("Attacking", Attacking);
    }

    void Chase()
    {
        Chasing_dir = Vector3.Normalize(Player.transform.position - transform.position);
        rigidbody.velocity = Chasing_dir * FAC_Speed;
        if (transform.position.x < Player.transform.position.x) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            GameObject bullet = collision.gameObject;
            Bullets bullets = bullet.GetComponent<Bullets>();
            Health -= bullets.bullet_damage;
            GameObject new_DamagePop = Instantiate(DamagePop,transform .position,Quaternion.identity);
            new_DamagePop.transform .SetParent(transform);
            TextMesh textMesh = new_DamagePop.GetComponent<TextMesh>();
            textMesh.text = bullets.bullet_damage.ToString();
            
            animator.Play("BeAttacked");
        }
    }
    IEnumerator Death()
    {
        Dead = true;
        StopCoroutine(Attack());
        if (new_Attackarea != null)Destroy(new_Attackarea);
        Body.enabled = false;
        Body2.enabled = false;
        animator.Play("Death");
        yield return new WaitForSeconds(1f);
        exp.CorrentExp += BAS_data.Exp_reward;
        Die.Invoke();
        Destroy(gameObject);
    }
}