using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private const string FirstLaunchKey = "FirstLaunch";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(FirstLaunchKey))
        {
            PlayerPrefs.DeleteAll();// 第一次启动游戏，清空所有PlayerPrefs
            PlayerPrefs.SetInt(FirstLaunchKey, 1);// 设置标志，防止再次清空
            PlayerPrefs.Save();
        }
    }
}