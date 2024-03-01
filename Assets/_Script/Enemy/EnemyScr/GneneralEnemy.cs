
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GneneralEnemy : MonoBehaviour
{
    public Tilemap tilemap;
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
    public Tilemap Ground;
    public Tilemap Obstacle;
    public GameObject Ground_obj;
    public GameObject Obstacle_obj;
    public PathFindingManager pathFindingManager;

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
    public bool AvoidingObstacle;
    public Vector3 Chasing_dir; 
    public Color CriticalHit;
    public Coroutine death;
    public Coroutine FindingPath;
           GameObject new_Attackarea;

    public event Action Die;

    public void Start()
    {
        pathFindingManager = FindAnyObjectByType<PathFindingManager>();
        tilemap = GameObject.Find("FloorTilemap").GetComponent<Tilemap>();
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
        GetTile();
        if (!Attacking && !BeHittingBack && !Dead && !AvoidingObstacle) Chase();
        if (!Attacking && (Vector3.Distance(transform.position, Player.transform.position) <= FAC_Attackarea) && !Dead && !AvoidingObstacle) StartCoroutine(Attack());
        if (Health <= 0 && death == null) death = StartCoroutine(Death());
    }

    void GetTile()
    {
        if (Ground == null)
        {
            Ground_obj = GameObject.Find("FloorTilemap");
            Ground = Ground_obj.GetComponent<Tilemap>();
        }
        if (Obstacle == null)
        {
            Obstacle_obj = GameObject.Find("ObstacleTilemap");
            Obstacle = Obstacle_obj.GetComponent<Tilemap>();
        }
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
        float angle;//���ɹ���ָʾ���ķ���
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

        if (collision.gameObject.CompareTag("Obstacle") && Vector3.Distance(transform.position ,Player.transform.position )<30)
        {
            if (FindingPath != null) StopCoroutine(FindingPath);
            FindingPath = StartCoroutine(ObstacleAvoidance());
        }
    }

    IEnumerator BeHitBack()
    {
        BeHittingBack = true;
        float Force = bullets.HitBackForce;
        float angle = (bullet.transform.rotation.eulerAngles.z-270)*Mathf.Deg2Rad;
        Vector2 Dirction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        rigidbody.AddForce(Dirction * Force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        BeHittingBack = false;
    }

    IEnumerator Death()
    {
        Dead = true;
        StopCoroutine(Attack());
        Die.Invoke();
        exp.CorrentExp += BAS_data.Exp_reward;
        if (new_Attackarea != null) Destroy(new_Attackarea);
        Body.enabled = false;
        Body2.enabled = false;
        HealthRewarding();
        animator.Play("Death");
        PlayerPrefs.SetInt("Point",PlayerPrefs.GetInt("Point",0)+BAS_data.Exp_reward);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void HealthRewarding()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < HealthRewardChance) Instantiate(HealthReward, transform.position, Quaternion.identity);
    }

    IEnumerator ObstacleAvoidance()
    {
        AvoidingObstacle = true;
        if (Ground != null && Obstacle != null)
        {
            BoundsInt ground = Ground.cellBounds;
            yield return new WaitForEndOfFrame();
            List<AStarTile> path = null;
            path = pathFindingManager.FindPath(Ground.WorldToCell(transform.position), Ground.WorldToCell(Player.transform.position), pathFindingManager.InitialTile(Ground, Obstacle), Ground, Obstacle);
            if (path != null)
            {
                Debug.Log(new Vector2(path[path.Count-1].x, path[path.Count-1].y), gameObject);
                do
                {
                    AStarTile nextnode = path[0];
                    Vector3 WorldPos = Ground.CellToWorld(new Vector3Int(nextnode.x, nextnode.y, 0));
                    Vector3 nowPos = transform.position;
                    StartCoroutine(MoveToPosition(WorldPos,Vector3.Distance(transform.position ,Player.transform.position )/FAC_Speed, new Vector3Int(nextnode.x, nextnode.y, 0)));
                    path.Remove(nextnode);
                }
                while (path.Count > 0);
            }
            AvoidingObstacle = false;
        }
        yield return null;
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float moveTime, Vector3Int targetCell)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;

            if (Ground.WorldToCell(transform.position) == targetCell)
            {
                transform.position = targetPosition;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPosition;
    }

}