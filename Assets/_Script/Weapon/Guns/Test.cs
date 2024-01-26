using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour,IGuns
{
    public GameObject bullet;
    public Quaternion Player_rotation;

    public int BulletCapacity_max;
    public int BulletCapacity_now;

    public float reloading_time;
    [Range(0,1)]public float shooting_interval;
    public float last_shoot_time=-10;

    public bool Reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        BulletCapacity_now = BulletCapacity_max;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Shoot");
            Shoot();
        }
    }



    public IEnumerator ReLoad(float reloading_time)
    {
        Reloading = true;
        yield return new WaitForSeconds(reloading_time);
        BulletCapacity_now = BulletCapacity_max;
        Reloading = false;
    }

    public void Shoot()
    {
        if (BulletCapacity_now <= 0) { StartCoroutine(ReLoad(reloading_time)); }
        else
        {
            if (!Reloading)
            {
                if (Time.time - last_shoot_time > shooting_interval)
                {
                    last_shoot_time = Time.time;
                    Player_rotation =transform.parent.rotation;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    BulletCapacity_now--;
                }
            }
        }
    }
}
