using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    Vector3 MousePos;
    private bool point_dir;
    public float angle;
    private Vector3 relative_pos;

    [SerializeField]
    [Tooltip("Class")]
    public Rigidbody2D rigidbody;


    [SerializeField,Range(1,10)] public float Turningspeed;

    [SerializeField]
    [Tooltip("Íæ¼ÒÒÆ¶¯")]
    [Range(1,100)]public float MoveSpeed;
    public float move_dir_x;
    public float move_dir_y;

    private void Awake()
    {
        cam = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        TurnToPointer();
    }
    void Move()
    {
        move_dir_x = Input.GetAxis("Horizontal");
        move_dir_y = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector2(move_dir_x*MoveSpeed,move_dir_y*MoveSpeed);
    }

    void TurnToPointer()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        point_dir = relative_pos.x > 0 ? true : false;
        if (!point_dir) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            Debug.Log("BeAttacked");
        }
    }
}
