using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Timer timer_scr;
    public Rigidbody2D rigidbody;
    public GameObject Player;
    public GameObject Attackarea;
    public Tank_data BAS_data;
    public SpriteRenderer sprite_renderer;
    public Sprite Attackarea_spr;

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
        DataInitial();

    }

    // Update is called once per frame
    void Update()
    {
        if (!Attacking) Chase();
        Debug.Log(!Attacking && (Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea));
        if (!Attacking && (Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea)) StartCoroutine(Attack());
        if (Health <= 0) Death();
    }

    void DataInitial()
    {
        FAC_Speed = BAS_data.BAS_Speed;
        FAC_MaxHealth = Mathf.RoundToInt(BAS_data.BAS_MaxHealth + 0.3f * Timer.timer);
        FAC_Atackvalue = Mathf.RoundToInt(BAS_data.BAS_Atackvalue + 0.05f * Timer.timer);
        FAC_Attackarea = BAS_data.BAS_Attackarea;
    }

    IEnumerator Attack()
    {
        Debug.Log("Attacking");
        Attacking = true;
        rigidbody.velocity = Vector3.zero;
        Vector3 Relative_pos = Player.transform.position - transform.position;
        float angle;//生成攻击指示器的方向
        angle = Mathf.Atan2(Relative_pos.x, Relative_pos.y) * Mathf.Rad2Deg;
        Debug.Log(angle);
        GameObject new_Attackarea = Instantiate(Attackarea, transform.position + new Vector3(0, BAS_data.BAS_Attackarea, 0), Quaternion.identity);
        new_Attackarea.transform.RotateAround(transform.position, Vector3.forward, -angle);
        new_Attackarea.transform.SetParent(transform);
        new_Attackarea.transform.localScale = new Vector3(BAS_data.BAS_Attackarea / 3, BAS_data.BAS_Attackarea / 3, 1);
        new_Attackarea.transform.SetParent(transform);
        yield return new WaitForSeconds(BAS_data.BAS_AttackSpeed);
        Collider2D Attackerea_collider = new_Attackarea.GetComponent<PolygonCollider2D>();
        Attackerea_collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(new_Attackarea);
        Attacking = false;
    }

    void Chase()
    {
        Chasing_dir = Vector3.Normalize(Player.transform.position - transform.position);
        rigidbody.velocity = Chasing_dir * FAC_Speed;
        if (transform.position.x < Player.transform.position.x) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void Death()
    {

    }
}
