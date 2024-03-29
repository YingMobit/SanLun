using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // 声明
    public GameObject PauseMenu;            // 暂停菜单
    public GameObject DeadMenu;         // 死亡菜单
    public Animator transition;         // 过场
    public float transitionTime = 1f;           // 过场时间
    private Text record;            // 战绩
    public static SceneLoader Instance { get; private set; }           // 静态的 Instance 属性，用于获取单例实例

    // 函数
    // 外部函数
    public void StartGame()
    {
        PlayerPrefs.SetInt("PointState", -1);//1 成功 0 死亡 -1无数据/已读取
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
    }

    public void SwichScene()
    {
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
    }// 未添加 PlayerPrefs.SetInt("PointState", 1);

    public void ContinueGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DeadGame()// TODO：注意这里，希望都改成自然死亡
    {
        //HADDO:是否应该弄成继续游戏/自我了断/退出游戏
        PlayerController playerController = FindObjectOfType<PlayerController>();
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerController.health = 0;
        //TODO:相机缩放/黑幕围拢（只留下玩家处明亮）（半透明），这时候应该条timescale吧？

        // 然后引用DeadMenu.SetActive(true);
        //TODO:设置数据统计
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("PointState", 0);//1 成功 0 死亡 -1无数据/已读取
        Application.Quit();
    }

    // 内部函数
    void Awake()
    {
        if (Instance == null)// 检查是否已经存在一个实例
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);// 如果已经存在实例，销毁当前实例，以确保只有一个实例存在
        }
    }
    private void Start()
    {
        if (PauseMenu.transform.childCount >= 4 && PauseMenu.transform.GetChild(3) != null)
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
                    //HASDO:需要判断在那个场景中CHANGE:不需要，只需要改每个场景的PauseMenu就好
                    PauseGame();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
                {
                    ContinueGame();
                }
            }
        }
        /*if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("换场景");
            StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 2 ? 1 : 2));
        }*/
    }

    private void PauseGame()// 暂停游戏
    {
        //TODO:战绩
        if(record!=null)
        {
            record.text = "击杀敌怪得分："+ PlayerPrefs.GetInt("Point", 0)+"\r\n打破屏障数量："+Health.BarrierDestroyCount+"\r\n解锁出口数量："+Health.ExitGenerateCount;
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
            //LoadSlider.value = progress; // Slider转转转
            if (sync.progress >= 0.9f)// 90%后激活场景
            {
                sync.allowSceneActivation = true;
            }
            yield return null;
        }
    }// 异步加载场景

    IEnumerator LoadSceneByCrossFade(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
