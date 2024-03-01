using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    [Header("读条相关")]
    public float delay = 6.0f; // 玩家需要停留的时间
    private float timer = 0.0f; // 计时器
    private bool IsPlayerInTrigger = false; // 玩家是否在触发区域内的标志
    public GameObject ProtalBar; // 读条bar

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
        ProtalBar.transform.GetChild(1).GetComponent<Image>().fillAmount = timer / delay; // 更新进度条
        ProtalBar.transform.GetChild(2).GetComponent<Text>().text = timer + "/"+ delay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = 0.0f; // 重置计时器
            IsPlayerInTrigger = true;
            ProtalBar.gameObject.SetActive(true); // 显示进度条
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
    // 读条
    private void LoadTime()
    {
        if (ProtalBar.gameObject.activeSelf)
        {
            if (IsPlayerInTrigger)// 如果进度条处于激活状态，但玩家不在触发区内，则减少进度条
            {
                timer += Time.deltaTime;
                UpdateProgressBar();
                if (timer >= delay)
                {
                    SceneLoader.Instance.SwichScene();
                    ProtalBar.gameObject.SetActive(false); // 隐藏进度条
                    PlayerPrefs.SetInt("PointState", 1);//1 成功 0 死亡 -1无数据/已读取
                }
            }
            else
            {
                timer -= Time.deltaTime * 1.5f;
                UpdateProgressBar();
                if (timer <= 0.0f)
                {
                    ProtalBar.gameObject.SetActive(false);// 当计时器为0时销毁进度条
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
                SceneManager.LoadScene("Base"); // 加载场景
                ProtalBar.gameObject.SetActive(false); // 隐藏进度条
            }
        }
    }*/
}
