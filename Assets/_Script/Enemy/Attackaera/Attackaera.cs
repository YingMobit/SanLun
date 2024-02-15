using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackaera : MonoBehaviour
{
    public int AttackValue;
    public PlayerController Player_scr;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Åö×²");
        if (collision.gameObject .tag == "Player") 
        {
            Debug.Log("Åö×²");
            Player_scr = collision.gameObject.GetComponent<PlayerController>();
            if (Time.time - Player_scr.Last_Be_Attacked_time > 0.5f) { Player_scr.Health -= AttackValue; Player_scr.Last_Be_Attacked_time = Time.time; }
        }
    }

}
