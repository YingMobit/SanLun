using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using JetBrains.Annotations;

public class EnemyGenerate : MonoBehaviour
{

    public GameObject Simple;
    public GameObject PBO;
    public GameObject Tank;
    public GameObject Player;
    public ExpCountor exp_countor;

    public int current_level =1;
    [Range(10, 100)] public float Closest_generate_distance;
    [Range(0,3)]public float generate_interval;
    public float last_generate_time;
    public Vector3 Player_Pos;

    // Start is called before the first frame update
    void Start()
    {
        exp_countor = GameObject.FindFirstObjectByType<ExpCountor>();
        exp_countor.LevelUPed += LevelUP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - last_generate_time >= generate_interval) Generate(ChooseEnemy());
    }

    void LevelUP() { current_level++; }

    void Generate(GameObject Enemy)
    {
        int seed = DateTime.Now.GetHashCode();
        float Farest_generate_distance = 3 * Closest_generate_distance;
        System.Random rand = new System.Random(seed);
        float random_distance = rand.Next(1, 3)*2*Closest_generate_distance;
        float angle = (float)rand.NextDouble()*Mathf.PI;
        Player_Pos = Player.transform.position;
        Vector3 generate_pos = Player_Pos + new Vector3(random_distance*Mathf.Cos(angle),random_distance*Mathf.Sin(angle),0);
        Instantiate(Enemy,generate_pos,Quaternion.identity);
        last_generate_time = Time.time;
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
