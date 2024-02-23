using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains.Annotations;
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
    [Range(0,3)]public float generate_interval;
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
        GroundTile = GameObject.Find("FloorTilemap");
        Ground = GroundTile.GetComponent<Tilemap>();
        ObstacleTile = GameObject.Find("ObstacleTilemap");
        Obstacle = ObstacleTile.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - last_generate_time >= generate_interval) Generate(ChooseEnemy());
    }

    void LevelUP() { current_level++; generate_interval -= 0.03f; }

    void Generate(GameObject Enemy)
    {
        Instantiate(Enemy, ChoosePos(), Quaternion.identity);
        last_generate_time = Time.time;
    }

    Vector3 GeneratePos()
    {
        int seed = DateTime.Now.GetHashCode();
        float Farest_generate_distance = 3 * Closest_generate_distance;
        System.Random rand = new System.Random(seed);
        float random_distance = rand.Next(1, 3) * 2 * Closest_generate_distance;
        float angle = (float)rand.NextDouble() * Mathf.PI;
        Player_Pos = Player.transform.position;
        Vector3 generate_pos = Player_Pos + new Vector3(random_distance * Mathf.Cos(angle), random_distance * Mathf.Sin(angle), 0);
        Debug.Log(generate_pos);
        Ray ray = new Ray(generate_pos,transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,5))
        {
            if (hit.collider.CompareTag("Ground")) return generate_pos;
            else return GeneratePos();
        }
        else return GeneratePos();
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
        BoundsInt Bounds = Ground.cellBounds;
        bool isPositionValid =false;
        Vector3 Position;
        do
        {
            int X = rand.Next(Bounds.xMin, Bounds.xMax);
            int Y = rand.Next(Bounds.yMin, Bounds.yMax);
            Vector3Int spawnPosition = new Vector3Int(X, Y, 0);
            Position = Ground.CellToWorld(spawnPosition);
            if ((Ground.HasTile(spawnPosition) && !Obstacle.HasTile(spawnPosition))&&( Vector3.Distance(Player.transform.position, Position) >= Closest_generate_distance)) isPositionValid = true;
        }
        while (!isPositionValid);
        return Position;
    }
    GameObject ChooseEnemy()
    {
        if (current_level <= 4)
        {
            return Simple;
        }

        else if (current_level <= 10)
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
