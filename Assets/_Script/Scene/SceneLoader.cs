using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // 声明
    public GameObject PauseMenu;            // 暂停菜单
    public Animator transition;         // 过场
    public float transitionTime = 1f;           // 过场时间


    // 函数
    // 外部函数
    public void ContinueGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackBase()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //StartCoroutine(LoadSceneAsync("Base"));
        StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1));

    }

    // 内部函数
    private void Start()
    {
        PauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(!PauseMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("换场景");
            StartCoroutine(LoadSceneByCrossFade(SceneManager.GetActiveScene().buildIndex == 1 ? 0 : 1));
        }
    }

    private void PauseGame()// 暂停游戏
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
