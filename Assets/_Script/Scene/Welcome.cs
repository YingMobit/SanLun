using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Welcome : MonoBehaviour
{
    // 声明
    public InputField inputField;           // 昵称输入框
    public GameObject WelcomePanel;         // 欢迎界面
    private const string FirstLaunchKey = "FirstLaunch";

    // 函数
    // 外部函数
    public void StartGame()
    {
        if(inputField.text != null)
        {
            PlayerPrefs.SetString("playerName", inputField.text);
        }
        else
        {
            PlayerPrefs.SetString("playerName", "Guest" + Random.Range(1000, 10000).ToString());
        }
        PlayerPrefs.SetInt("PointState", -1);
        PlayerPrefs.SetInt("IsFirst", 1);
        SceneManager.LoadScene(1);
    }
    // 内部函数
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(FirstLaunchKey))
        {
            PlayerPrefs.DeleteAll();// 第一次启动游戏，清空所有PlayerPrefs
            PlayerPrefs.SetInt(FirstLaunchKey, 1);// 设置标志，防止再次清空
            PlayerPrefs.Save();
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsFirst", 1) == 0)
        {
            inputField.text = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1000, 10000).ToString());
        }
        else
        {
            inputField.text = null;
        }
    }
}
