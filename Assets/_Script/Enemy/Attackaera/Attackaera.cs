using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackaera : MonoBehaviour
{
    public int AttackValue;
    public PlayerController Player_scr;
    public GameObject Blood;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player_scr = collision.gameObject.GetComponent<PlayerController>();
            if (Player_scr == null) Player_scr = collision.gameObject.GetComponentInParent<PlayerController>();
            if (!Player_scr.Invincible)
            {
                Instantiate(Blood, collision.gameObject.transform.position, Quaternion.identity);
                if (Time.time - Player_scr.Last_Be_Attacked_time > 0.5f) { Player_scr.Health -= AttackValue; Player_scr.Last_Be_Attacked_time = Time.time; }
            }
        }
    }
}
