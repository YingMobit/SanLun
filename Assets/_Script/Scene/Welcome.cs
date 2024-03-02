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
        SceneManager.LoadScene(1);
    }
    // 内部函数
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsFirst", 1) == 0)
        {
            inputField.text = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1000, 10000).ToString());
        }
    }
}
