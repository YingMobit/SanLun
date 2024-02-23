using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    public float delay = 5.0f; // �����Ҫͣ����ʱ��
    public GameObject ProtalBar; // ����bar

    private float timer = 0.0f; // ��ʱ��
    private bool IsPlayerInTrigger = false; // ����Ƿ��ڴ��������ڵı�־

    private void Start()
    {
        ProtalBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ProtalBar.gameObject.activeSelf)
        {
            if (!IsPlayerInTrigger)// ������������ڼ���״̬������Ҳ��ڴ������ڣ�����ٽ�����
            {
                timer -= Time.deltaTime;
                UpdateProgressBar();
                if (timer <= 0.0f)
                {
                    ProtalBar.gameObject.SetActive(false);// ����ʱ��Ϊ0ʱ���ٽ�����
                }
            }
        }
    }
    private void UpdateProgressBar()
    {
        ProtalBar.transform.GetChild(1).GetComponent<Image>().fillAmount = timer / delay; // ���½�����
        ProtalBar.transform.GetChild(2).GetComponent<Text>().text = timer + "/"+ delay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ProtalBar.gameObject.SetActive(true); // ��ʾ������
            timer = 0.0f; // ���ü�ʱ��
            IsPlayerInTrigger = true;
            Debug.Log("jinru");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInTrigger = false;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
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
    }
}
