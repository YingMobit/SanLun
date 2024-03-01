using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    [Header("�������")]
    public float delay = 6.0f; // �����Ҫͣ����ʱ��
    private float timer = 0.0f; // ��ʱ��
    private bool IsPlayerInTrigger = false; // ����Ƿ��ڴ��������ڵı�־
    public GameObject ProtalBar; // ����bar

    private void Start()
    {
        ProtalBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        LoadTime();
    }

    private void UpdateProgressBar()
    {
        ProtalBar.transform.GetChild(1).GetComponent<Image>().fillAmount = timer / delay; // ���½�����
        ProtalBar.transform.GetChild(2).GetComponent<Text>().text = timer + "/"+ delay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = 0.0f; // ���ü�ʱ��
            IsPlayerInTrigger = true;
            ProtalBar.gameObject.SetActive(true); // ��ʾ������
            Debug.Log("jinru");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsPlayerInTrigger = false;
        }
    }
    // ����
    private void LoadTime()
    {
        if (ProtalBar.gameObject.activeSelf)
        {
            if (IsPlayerInTrigger)// ������������ڼ���״̬������Ҳ��ڴ������ڣ�����ٽ�����
            {
                timer += Time.deltaTime;
                UpdateProgressBar();
                if (timer >= delay)
                {
                    SceneLoader.Instance.SwichScene();
                    ProtalBar.gameObject.SetActive(false); // ���ؽ�����
                    PlayerPrefs.SetInt("PointState", 1);//1 �ɹ� 0 ���� -1������/�Ѷ�ȡ
                }
            }
            else
            {
                timer -= Time.deltaTime * 1.5f;
                UpdateProgressBar();
                if (timer <= 0.0f)
                {
                    ProtalBar.gameObject.SetActive(false);// ����ʱ��Ϊ0ʱ���ٽ�����
                }
            }
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            UpdateProgressBar();
            if (timer >= delay)
            {
                SceneManager.LoadScene("Base"); // ���س���
                ProtalBar.gameObject.SetActive(false); // ���ؽ�����
            }
        }
    }*/
}
