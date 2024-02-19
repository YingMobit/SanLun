using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;

    [SerializeField,Range(1,20)]public float speed;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        Chase();
    }

    void Chase()
    {
        transform .position = new Vector3 ( Vector2.Lerp(transform .position ,Player .transform .position ,speed*Time.deltaTime).x, 
            Vector2.Lerp(transform.position, Player.transform.position, speed * Time.deltaTime).y, transform.position .z);
    }
}
