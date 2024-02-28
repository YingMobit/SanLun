using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoredNPC : MonoBehaviour
{
    // 声明
    public GameObject LeaderBored;          //  排行榜
    public GameObject GuideIcon;            // 引导图标 就是按下E

    // 函数
    // 外部函数
    public void CloseLeaderBored()
    {
        LeaderBored.SetActive(false);
    }

    // 内部函数
    private void Start()
    {
        GuideIcon.SetActive(false);
        LeaderBored.SetActive(false);
    }
    private void Update()
    {
        if(GuideIcon.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                LeaderBored.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GuideIcon.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GuideIcon.SetActive(false);
        }
    }
}
