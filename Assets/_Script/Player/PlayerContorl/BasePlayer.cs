using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BasePlayer : MonoBehaviour
{
    // 声明
    [Header("组件")]
    public Rigidbody2D rigidbody;
    public Animator anitor;

    [Header("移动")]
    public float speed;
    private Vector3 mousePos;
    private Vector3 relativePos;

    private void Update()
    {
        Move();
        Flip();
    }

    private void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");
        rigidbody.velocity = new Vector2(xMove * speed, yMove * speed);
        if (rigidbody.velocity != Vector2.zero)
        {
            anitor.SetBool("Moving", true);
        }
        else
        {
            anitor.SetBool("Moving", false);
        }
    }

    private void Flip()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        relativePos = mousePos - transform.position;
        if (relativePos.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
