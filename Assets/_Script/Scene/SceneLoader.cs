using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // ����
    public GameObject PauseMenu;            // ��ͣ�˵�
    public Animator transition;         // ����
    public float transitionTime = 1f;           // ����ʱ��


    // ����
    // �ⲿ����
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

    // �ڲ�����
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
