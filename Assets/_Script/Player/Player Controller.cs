using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Class")]
    private Camera cam;
    public Rigidbody2D rigidbody;

    [SerializeField]
    [Tooltip("FaceToPointer")]
    Vector3 MousePos;
    private int point_dir;
    public float angle;
    private Vector3 relative_pos;

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
        TurnToPoint();
        Move();   
    }

    void TurnToPoint()
    {
        MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        relative_pos = MousePos - transform.position;
        point_dir = relative_pos.x < 0? 1:-1;
        angle = Mathf.Atan2(relative_pos.x, relative_pos.y)*Mathf.Rad2Deg; 
        //angle = angle * Mathf.Rad2Deg;
        //Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Lerp(transform.rotation,quaternion,Turningspeed*Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0,0,-angle));
        
    }

    void Move()
    {
        move_dir_x = Input.GetAxis("Horizontal");
        move_dir_y = Input.GetAxis("Vertical");
        rigidbody.velocity = new Vector2(move_dir_x*MoveSpeed,move_dir_y*MoveSpeed);
    }
}
