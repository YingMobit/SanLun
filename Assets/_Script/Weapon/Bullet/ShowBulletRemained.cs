using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBulletRemained : MonoBehaviour
{
    public GameObject Player;
    public WeaponSystem Weapon;
    public Camera maincam;
    RectTransform transform;
    public Image image;
    public Vector3 Correction;
    Vector3 Player_pos_world;
    Vector3 Player_pos_screen;
    float BulletRemained;
    float BulletMaxCount;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Weapon = Player.transform.GetComponentInChildren<WeaponSystem>();
        maincam = Camera.main;
        transform = GetComponent<RectTransform>();
        //image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
        ShowBullets();
    }

    void Chase()
    {
        Player_pos_world = Player.transform.position;
        Player_pos_screen = maincam.WorldToScreenPoint(Player_pos_world);
        transform.position = Player_pos_screen+Correction;
    }

    void ShowBullets()
    {
        BulletMaxCount = Weapon.Fac_Magazine_Capacity;
        BulletRemained = Weapon.Bullet_Remained;
        if (image!=null) image.fillAmount = BulletRemained/BulletMaxCount;
    }
}
