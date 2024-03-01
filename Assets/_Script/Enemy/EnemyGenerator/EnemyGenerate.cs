using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnemyGenerate : MonoBehaviour
{
    //public List<GameObject> Enemys;
    public GameObject Simple;
    public GameObject PBO;
    public GameObject Tank;
    public GameObject Player;
    public GameObject ObstacleTile;
    public GameObject GroundTile;
    public ExpCountor exp_countor;
    public Tilemap Ground;
    public Tilemap Obstacle;

    public int current_level =1;
    [Range(0, 100)] public float Closest_generate_distance;
    [Range(0,10)]public float generate_interval;
    public float last_generate_time;
    public Vector3 Player_Pos;

    // Start is called before the first frame update
    void Start()
    {
        exp_countor = GameObject.FindFirstObjectByType<ExpCountor>();
        exp_countor.LevelUPed += LevelUP;
        StartCoroutine(GetTile());
    }

    IEnumerator GetTile()
    { 
        yield return new WaitForSeconds(0.5f);
        //GroundTile = GameObject.Find("FloorTilemap");
        //Ground = GroundTile.GetComponent<Tilemap>();
        ObstacleTile = GameObject.Find("ObstacleTilemap");
        Obstacle = ObstacleTile.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ground == null) 
        {
            GroundTile = GameObject.Find("FloorTilemap");
            Ground = GroundTile.GetComponent<Tilemap>();
        }
        if (Obstacle == null)
        {
            ObstacleTile = GameObject.Find("ObstacleTilemap");
            Obstacle = ObstacleTile.GetComponent<Tilemap>();
        }
        if (Time.time - last_generate_time >= generate_interval&& Obstacle != null&& Ground != null) Generate(ChooseEnemy());
    }

    void LevelUP() { current_level++; generate_interval -= 0.03f; }

    void Generate(GameObject Enemy)
    {
        if (Ground != null && Obstacle != null)
        {
            Instantiate(Enemy, ChoosePos(), Quaternion.identity);
            last_generate_time = Time.time;
        }
    }

    Vector3 GeneratePos()
    {
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        Vector3 generate_pos;
        Ray ray;
        RaycastHit2D hit;
        do
        {
            float random_distance = (float)rand.NextDouble() * Closest_generate_distance + Closest_generate_distance;
            float angle = (float)rand.NextDouble() * Mathf.PI;
            Player_Pos = Player.transform.position;
            generate_pos = Player_Pos + new Vector3(random_distance * Mathf.Cos(angle), random_distance * Mathf.Sin(angle), 0);
            Debug.Log(generate_pos);
            ray = new Ray(generate_pos - transform.forward, transform.forward);
            Debug.DrawRay(generate_pos - transform.forward, transform.forward,Color.red, Mathf.Infinity);
            Debug.Log(Physics2D.Raycast(generate_pos - transform.forward, transform.forward, Mathf.Infinity));
            hit = Physics2D.Raycast(generate_pos-transform.forward,transform.forward,Mathf.Infinity,0);
            if (hit.collider == null) continue;
        }
        while (!hit.collider.CompareTag("Ground"));
        return generate_pos;
    }

    //真有bug：切完场景会丢失tilemap而且怪还是有在外面的|||或者你改用raycast。草地已经加上了tilemapcollider2d和compositecollider2d为了修之前的bug
    /*
     * do
     * {20-50范围里头随机一个vector3的世界坐标}
     * while（！MapGenerator.Instance.EnemyPos(vector3的世界坐标)）
     */
    Vector3 ChoosePos()
    {
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        bool isPositionFar =false;
        Vector3 Position = new Vector3(0,0,0);
        if (Ground != null)
        {
            BoundsInt Bounds = Ground.cellBounds;
            if (Bounds.xMin + 1 < Bounds.xMax - 1 && Bounds.yMin + 1 < Bounds.yMax - 1)
            {
                do
                {
                    Debug.Log(new Vector2Int(Bounds.xMin + 1, Bounds.xMax - 1));
                    Debug.Log(new Vector2Int(Bounds.yMin + 1, Bounds.yMax - 1));
                    int X = rand.Next(Bounds.xMin + 1, Bounds.xMax - 1);
                    int Y = rand.Next(Bounds.yMin + 1, Bounds.yMax - 1);
                    Vector3Int spawnPosition = new Vector3Int(X, Y, 0);
                    Position = Ground.CellToWorld(spawnPosition);
                    isPositionFar = Vector3.Distance(Position, Player.transform.position) >= Closest_generate_distance;
                }
                while (!MapGenerator.Instance.EnemyPos(Position) || !isPositionFar);
                return Position;
            }
        }
        return Position;

    }
    GameObject ChooseEnemy()
    {
        if (current_level <= 4)
        {
            return Simple;
        }

        else if (current_level <= 7)
        {
            int seed = DateTime.Now.GetHashCode();
            System.Random rand = new System.Random(seed);
            float random_num = (float)(rand.NextDouble());
            if (random_num < 0.7) return Simple;
            else return PBO;
        }

        else
        {
            int seed = DateTime.Now.GetHashCode();
            System.Random rand = new System.Random(seed);
            float random_num = (float)(rand.NextDouble());
            if (random_num<0.65) return Simple;
            else if (random_num<0.9) return PBO;
            else return Tank;
        }
    }
}
