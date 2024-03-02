using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private const string FirstLaunchKey = "FirstLaunch";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(FirstLaunchKey))
        {
            PlayerPrefs.DeleteAll();// ��һ��������Ϸ���������PlayerPrefs
            PlayerPrefs.SetInt(FirstLaunchKey, 1);// ���ñ�־����ֹ�ٴ����
            PlayerPrefs.Save();
        }
    }
}