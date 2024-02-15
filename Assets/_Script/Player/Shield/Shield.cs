using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject Player;
    public PlayerController Player_scr;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Player_scr = transform.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack") Destroy(gameObject);
    }

    void OnDestroy()
    {
        Collider2D PlayerBody = Player.GetComponent<Collider2D>();
        PlayerBody.enabled = true;
        Player_scr.Last_Shield_Time = Time.time;
        Player_scr.Last_Be_Attacked_time = Time.time;
    }
}
