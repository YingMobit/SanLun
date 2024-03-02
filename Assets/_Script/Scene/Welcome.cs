using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Welcome : MonoBehaviour
{
    // ����
    public InputField inputField;           // �ǳ������
    public GameObject WelcomePanel;         // ��ӭ����
    private const string FirstLaunchKey = "FirstLaunch";

    // ����
    // �ⲿ����
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
    // �ڲ�����
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(FirstLaunchKey))
        {
            PlayerPrefs.DeleteAll();// ��һ��������Ϸ���������PlayerPrefs
            PlayerPrefs.SetInt(FirstLaunchKey, 1);// ���ñ�־����ֹ�ٴ����
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
