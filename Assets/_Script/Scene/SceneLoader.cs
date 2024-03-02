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
    private Text record;            // ս��
    public static SceneLoader Instance { get; private set; }           // ��̬�� Instance ���ԣ����ڻ�ȡ����ʵ��

    // ����
    // �ⲿ����
    public void StartGame()
    {
        PlayerPrefs.SetInt("PointState", -1);//1 �ɹ� 0 ���� -1������/�Ѷ�ȡ
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
    }

    public void SwichScene()
    {
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
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
        if (PauseMenu.transform.GetChild(3) != null)
        {
            record = PauseMenu.transform.GetChild(3).GetChild(0).GetComponent<Text>();
        }
        PauseMenu.SetActive(false);
        if (DeadMenu != null)
        {
            DeadMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameObject.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>().alpha == 0)
        {
            if (!PauseMenu.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
                {
                    //HASDO:��Ҫ�ж����Ǹ�������CHANGE:����Ҫ��ֻ��Ҫ��ÿ��������PauseMenu�ͺ�
                    PauseGame();
                }
            }
        }
        /*if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("������");
            StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
        }*/
    }

    private void PauseGame()// ��ͣ��Ϸ
    {
        //TODO:ս��
        if(record!=null)
        {
            record.text = "��ɱ�йֵ÷֣�"+ PlayerPrefs.GetInt("Point", 0)+"\r\n��������������"+Health.BarrierDestroyCount+"\r\n��������������"+Health.ExitGenerateCount;
        }
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
