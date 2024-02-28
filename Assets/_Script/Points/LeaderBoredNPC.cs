using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoredNPC : MonoBehaviour
{
    // ����
    public GameObject LeaderBored;          //  ���а�
    public GameObject GuideIcon;            // ����ͼ�� ���ǰ���E

    // ����
    // �ⲿ����
    public void CloseLeaderBored()
    {
        LeaderBored.SetActive(false);
    }

    // �ڲ�����
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
