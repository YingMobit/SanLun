using System;
using System.Collections;
using UnityEngine;

public class GneneralEnemy : MonoBehaviour
{
    public Timer timer_scr;
    public Rigidbody2D rigidbody;
    public Animator animator;
    public GameObject Player;
    public GameObject Attackarea;
    public GameObject DamagePop;
    public GameObject HealthReward;
    GameObject bullet;
    public Enemy_data BAS_data;
    public ExpCountor exp;
    public Collider2D Body;
    public Collider2D Body2;
    public Rigidbody2D Rigidbody2d;
    public PlayerController Player_scr;
    public GameObject Blood;
    Bullets bullets;

    [Header("FactData")]
    public float FAC_Speed;
    public float FAC_MaxHealth;
    public int FAC_Atackvalue;
    public float FAC_Attackarea;
    public float HealthRewardChance;

    [Header("RealTimeData")]
    public float Health;
    public bool Attacking;
    public bool Dead;
    public bool BeHittingBack;
    public Vector3 Chasing_dir;
    GameObject new_Attackarea;
    public Color CriticalHit;
    public Coroutine death;

    public event Action Die;

    public void Start()
    {
        Rigidbody2d = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        Body = transform.GetChild(0).GetComponent<Collider2D>();
        Body2 = GetComponent<Collider2D>();
        Player_scr = Player.GetComponent<PlayerController>();
        Die += Player_scr.SuckBlood;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        exp = GameObject.Find("ExpManager").GetComponent<ExpCountor>();
        DataInitial();
        StartCoroutine(FreefromBarrier());
    }

    IEnumerator FreefromBarrier()
    {
        Body.enabled = false;
        Body2.enabled = false;
        yield return new WaitForSeconds(0.1f);
        Body.enabled = true;
        Body2.enabled = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!Attacking&&!BeHittingBack && !Dead) Chase();
        if (!Attacking && (Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea) && !Dead) StartCoroutine(Attack());
        if (Health <= 0 && death == null) death = StartCoroutine(Death());
    }


    void DataInitial()
    {
        FAC_Speed = BAS_data.BAS_Speed;
        FAC_MaxHealth = Mathf.RoundToInt(BAS_data.BAS_MaxHealth + BAS_data.Health_gain_bytime * Timer.timer);
        FAC_Atackvalue = Mathf.RoundToInt(BAS_data.BAS_Atackvalue + BAS_data.AttackValue_gain_bytime * Timer.timer);
        FAC_Attackarea = BAS_data.BAS_Attackarea;
        Health = FAC_MaxHealth;
    }

    IEnumerator Attack()
    {
        Attacking = true;
        Rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        animator.SetBool("Attacking", Attacking);
        rigidbody.velocity = Vector3.zero;
        Vector3 Relative_pos = Player.transform.position - transform.position;
        float angle;//生成攻击指示器的方向
        angle = Mathf.Atan2(Relative_pos.x, Relative_pos.y) * Mathf.Rad2Deg;
        new_Attackarea = Instantiate(Attackarea, transform.position + new Vector3(0, BAS_data.BAS_Attackarea, 0), Quaternion.identity);
        new_Attackarea.transform.RotateAround(transform.position, Vector3.forward, -angle);
        new_Attackarea.transform.SetParent(transform);
        new_Attackarea.transform.localScale = new Vector3(BAS_data.BAS_Attackarea / 3, BAS_data.BAS_Attackarea / 3, 1);
        new_Attackarea.transform.SetParent(transform);
        if (new_Attackarea != null)
        {
            Attackaera attackaera = new_Attackarea.GetComponent<Attackaera>();
            attackaera.AttackValue = FAC_Atackvalue;
        }
        yield return new WaitForSeconds(BAS_data.BAS_AttackSpeed);
        if (new_Attackarea != null)
        {
            Collider2D Attackerea_collider = new_Attackarea.GetComponent<PolygonCollider2D>();
            Attackerea_collider.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(new_Attackarea);
        Attacking = false;
        Rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool("Attacking", Attacking);
    }

    void Chase()
    {
        Chasing_dir = Vector3.Normalize(Player.transform.position - transform.position);
        rigidbody.velocity = Chasing_dir * FAC_Speed;
        if (transform.position.x < Player.transform.position.x) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            bullet = collision.gameObject;
            bullets = bullet.GetComponent<Bullets>();
            Health -= bullets.bullet_damage;
            GameObject new_DamagePop = Instantiate(DamagePop, transform.position, Quaternion.identity);
            new_DamagePop.transform.SetParent(transform);
            TextMesh textMesh = new_DamagePop.GetComponent<TextMesh>();
            if (bullets.isCriticalHit)
            {
                textMesh.color = CriticalHit;
                new_DamagePop.transform.localScale = new Vector3(2.25f * 1.6f, 2.25f * 1.6f, 2.25f * 1.6f);
            }
            else { textMesh.color = Color.red; new_DamagePop.transform.localScale = new Vector3(2.25f, 2.25f, 2.25f); }
            textMesh.text = bullets.bullet_damage.ToString();
            Instantiate(Blood, transform.position, Quaternion.identity);
            animator.Play("BeAttacked");
            StartCoroutine(BeHitBack());
        }
    }

    IEnumerator BeHitBack()
    {
        BeHittingBack = true;
        float Force = bullets.HitBackForce;
        float angle = (bullet.transform.rotation.eulerAngles.z-270)*Mathf.Deg2Rad;
        Vector2 Dirction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        rigidbody.AddForce(Dirction * Force, ForceMode2D.Impulse);
        Debug.Log(angle);
        Debug.Log(Dirction);
        yield return new WaitForSeconds(0.3f);
        BeHittingBack = false;
    }

    IEnumerator Death()
    {
        Dead = true;
        StopCoroutine(Attack());
        Die?.Invoke();
        exp.CorrentExp += BAS_data.Exp_reward;
        if (new_Attackarea != null) Destroy(new_Attackarea);
        Body.enabled = false;
        Body2.enabled = false;
        HealthRewarding();
        animator.Play("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void HealthRewarding()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < HealthRewardChance) Instantiate(HealthReward, transform.position, Quaternion.identity);
    }
}
