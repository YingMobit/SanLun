using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    public float delay = 5.0f; // 玩家需要停留的时间
    public GameObject ProtalBar; // 读条bar

    private float timer = 0.0f; // 计时器
    private bool IsPlayerInTrigger = false; // 玩家是否在触发区域内的标志

    private void Start()
    {
        ProtalBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ProtalBar.gameObject.activeSelf)
        {
            if (!IsPlayerInTrigger)// 如果进度条处于激活状态，但玩家不在触发区内，则减少进度条
            {
                timer -= Time.deltaTime;
                UpdateProgressBar();
                if (timer <= 0.0f)
                {
                    ProtalBar.gameObject.SetActive(false);// 当计时器为0时销毁进度条
                }
            }
        }
    }
    private void UpdateProgressBar()
    {
        ProtalBar.transform.GetChild(1).GetComponent<Image>().fillAmount = timer / delay; // 更新进度条
        ProtalBar.transform.GetChild(2).GetComponent<Text>().text = timer + "/"+ delay;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ProtalBar.gameObject.SetActive(true); // 显示进度条
            timer = 0.0f; // 重置计时器
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
                SceneManager.LoadScene("Base"); // 加载场景
                ProtalBar.gameObject.SetActive(false); // 隐藏进度条
            }
        }
    }
}
