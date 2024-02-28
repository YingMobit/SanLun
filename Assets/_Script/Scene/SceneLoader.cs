using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // ����
    public GameObject PauseMenu;            // ��ͣ�˵�
    public GameObject DeadMenu;         // �����˵�
    public Animator transition;         // ����
    public float transitionTime = 1f;           // ����ʱ��
    public static SceneLoader Instance { get; private set; }           // ��̬�� Instance ���ԣ����ڻ�ȡ����ʵ��

    // ����
    // �ⲿ����
    public void StartGame()
    {
        PlayerPrefs.SetInt("PointState", -1);//1 �ɹ� 0 ���� -1������/�Ѷ�ȡ
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1));
    }

    public void SwichScene()
    {
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1));
    }// δ��� PlayerPrefs.SetInt("PointState", 1);

    public void ContinueGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DeadGame()// TODO��ע�����ϣ�����ĳ���Ȼ����
    {
        //HADDO:�Ƿ�Ӧ��Ū�ɼ�����Ϸ/�����˶�/�˳���Ϸ
        PlayerController playerController = FindObjectOfType<PlayerController>();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerController.health = 0;
        //TODO:�������/��ĻΧ£��ֻ������Ҵ�����������͸��������ʱ��Ӧ����timescale�ɣ�

        // Ȼ������DeadMenu.SetActive(true);
        //TODO:��������ͳ��
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("PointState", 0);//1 �ɹ� 0 ���� -1������/�Ѷ�ȡ
        Application.Quit();
    }

    // �ڲ�����
    void Awake()
    {
        if (Instance == null)// ����Ƿ��Ѿ�����һ��ʵ��
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);// ����Ѿ�����ʵ�������ٵ�ǰʵ������ȷ��ֻ��һ��ʵ������
        }
    }
    private void Start()
    {
        PauseMenu.SetActive(false);
        DeadMenu.SetActive(false);
    }

    private void Update()
    {
        if(!PauseMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                //TODO:��Ҫ�ж����Ǹ�������
                PauseGame();
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("������");
            StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1));
        }
    }

    private void PauseGame()// ��ͣ��Ϸ
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation sync = SceneManager.LoadSceneAsync(sceneName);
        sync.allowSceneActivation = false;
        while (!sync.isDone)
        {
            float progress = Mathf.Clamp01(sync.progress / 0.9f);
            //LoadSlider.value = progress; // Sliderתתת
            if (sync.progress >= 0.9f)// 90%�󼤻��
            {
                sync.allowSceneActivation = true;
            }
            yield return null;
        }
    }// �첽���س���

    IEnumerator LoadSceneByCrossFade(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
