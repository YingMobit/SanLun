using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBO : MonoBehaviour
{
    public Timer timer_scr;
    public Rigidbody2D rigidbody;
    public GameObject Player;
    public PBO_data BAS_data;

    [Header("FactData")]
    public float FAC_Speed;
    public float FAC_MaxHealth;
    public float FAC_Atackvalue;

    [Header("RealTimeData")]
    public float Health;
    public Vector3 Chasing_dir; 

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        timer_scr = GameObject.Find("Timer").GetComponent<Timer>();
        DataInitial();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DataInitial()
    { 
        FAC_Speed = BAS_data.BAS_Speed;
        FAC_MaxHealth = Mathf.RoundToInt(BAS_data.BAS_MaxHealth + 0.3f * timer_scr.timer);
        FAC_Atackvalue = Mathf.RoundToInt(BAS_data.BAS_Atackvalue + 0.05f * timer_scr.timer);

    }

    void Attack()
    {
    
    }

    void Chase()
    {
        Chasing_dir = Player.transform.position - transform.position;
        rigidbody.velocity = Chasing_dir * FAC_Speed;
    }

    void Death()
    { 
    
    }
}
