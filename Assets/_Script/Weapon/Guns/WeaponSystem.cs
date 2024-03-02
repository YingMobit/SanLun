using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour,IGuns
{
    public SoundEffectController bgmController;
    public BuffManager_Weapon Buff;
    public GameObject Bullet;
    public SpriteRenderer sprite_renderer;
    [Header("基础参数")]
    public List<GunData> GunDatas;
    public GunData BasData;
    public PlayerController playerController;

    [Header("Buff")]
    public float BufOn_Reloading_time=1;
    public float BufOn_Shooting_Interval=1;
    public float BufOn_Damage=1;
    public float BufOn_Magazine_Capacity=0;
    public int BufOn_Penetration_Quantity=0;
    public float Bufon_HitBackForce = 1f;

    [Header("实际参数")]
    public float Fac_Reloading_time;
    public float Fac_Shooting_Interval;
    public int Fac_Damage;
    public float Fac_Magazine_Capacity;
    public int Fac_Penetration_Quantity;
    public float Fac_HitBackForce;
    public AudioClip ShootingAudio;
    public AudioClip ReloadAudio;

    [Header("动态参数")]
    public float Bullet_Remained;
    public bool Reloading;
    public string Weapon_Name;
    public float last_shoot_time;
    public float Last_AudioPlayed_Time;

    void Start()
    {
        playerController = transform.GetComponentInParent<PlayerController>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager_Weapon>();
        gameObject.name = PlayerPrefs.GetString("InitWeapon","USP"); 
        Weapon_Name = gameObject.name;
        Buff.OnDataChanged_Weapon+=DataInitial;
        DataInitial();
    }

    public void DataInitial()//计算实际参数
    {
        Weapon_Name = gameObject.name;
        BufOn_Reloading_time = Buff.Bufon_Reloading_time;
        BufOn_Shooting_Interval = Buff.Bufon_Shooting_Interval;
        BufOn_Damage = Buff.Bufon_Damage;
        BufOn_Magazine_Capacity = Buff.Bufon_Magazine_Capacity;
        BufOn_Penetration_Quantity = Buff.Bufon_Penetration_Quantity;

        switch (Weapon_Name)
        {
            case "USP":
                BasData = GunDatas[0];
            break;

            case "AK47":
                BasData = GunDatas[1];
            break;

            case "M4A1":
                BasData = GunDatas[2];
            break;

            case "M249":
                BasData = GunDatas[3];
            break;

            case "Revolver":
                BasData = GunDatas[4];
            break;

            default: break;
        }//基础参数赋值
        sprite_renderer.sprite = BasData.sprite;
        ShootingAudio = BasData.ShootAudio;

        Fac_Reloading_time = BasData.Bas_Reloading_time * BufOn_Reloading_time;
        Fac_Shooting_Interval = BasData.Bas_Shooting_Interval * BufOn_Shooting_Interval;
        Fac_Damage = Mathf.RoundToInt( BasData.Bas_Damage * BufOn_Damage);
        Fac_Magazine_Capacity = BasData.Bas_Magazine_Capacity + BufOn_Magazine_Capacity;
        Fac_Penetration_Quantity = BasData.Bas_Penetration_Quantity + BufOn_Penetration_Quantity;
        Fac_HitBackForce = BasData.Bas_HitBack * Bufon_HitBackForce; 
        ReloadAudio = BasData.ReloadAudio;

        Bullet_Remained = Fac_Magazine_Capacity;
    }

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (!playerController.dead)
        {
            Weapon_Name = gameObject.name;
            switch (gameObject.name)
            {
                case "USP":
                case "Revolver":
                    if (Input.GetMouseButtonDown(0))
                    {
                        Shoot();
                    }
                    break;

                default:
                    if (Input.GetMouseButton(0))
                    {
                        Shoot();
                    }
                    break;
            }
            if (Input.GetKeyDown(KeyCode.R)) { StartCoroutine(ReLoad(Fac_Reloading_time)); }
            if (Bullet_Remained <= 0) { StartCoroutine(ReLoad(Fac_Reloading_time)); }
        }
    }

    public IEnumerator ReLoad(float reloading_time)
    {
        if (!Reloading)
        { 
            Reloading = true;
            bgmController.PLayAudio(ReloadAudio);
            yield return new WaitForSeconds(reloading_time);
            Bullet_Remained = Fac_Magazine_Capacity;
            Reloading = false; 
        }
    }

    public void Shoot()
    {  
            if (!Reloading)
            {
                if (Time.time - last_shoot_time > Fac_Shooting_Interval)
                {
                    last_shoot_time = Time.time;
                    if (Time.time - Last_AudioPlayed_Time > 0.06f) { bgmController.PLayAudio(ShootingAudio); Last_AudioPlayed_Time = Time.time; }
                    GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                    Bullets bullet_scr = bullet.GetComponent<Bullets>();
                    bullet_scr.bullet_damage = Fac_Damage;
                    bullet_scr.bullet_penetration_times = Fac_Penetration_Quantity;
                    Bullet_Remained--;
                }
            }
    }

}
