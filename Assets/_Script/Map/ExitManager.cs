using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    [Header("读条相关")]
    public float delay = 6.0f; // 玩家需要停留的时间
    private float timer = 0.0f; // 计时器
    private bool isPlayerInTrigger = false; // 玩家是否在触发区域内的标志

    [Header("Portal Bar Prefab")]
    public GameObject portalBarPrefab; // 读条bar的预制体
    private GameObject portalBarInstance; // portalBar的实例
    public GameObject canvas;           // 画布
    private Image progressBar; // 进度条Image组件
    private Text progressText; // 进度条Text组件

    private void Update()
    {
        if (portalBarInstance != null)
        {
            UpdateLoadTime();
            UpdateProgressBar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("enter");
            isPlayerInTrigger = true;
            ShowPortalBar(true); // 创建并显示进度条
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            ShowPortalBar(false); // 隐藏并可能销毁进度条
        }
    }

    private void UpdateLoadTime()
    {
        float timeChange = isPlayerInTrigger ? Time.deltaTime : -Time.deltaTime * 1.5f;
        timer = Mathf.Clamp(timer + timeChange, 0, delay);

        if (timer >= delay)
        {
            CompleteLoading();
        }
        else if (timer <= 0.0f && !isPlayerInTrigger)
        {
            ShowPortalBar(false); // 隐藏并销毁进度条
        }
    }

    private void UpdateProgressBar()
    {
        if (portalBarInstance == null) return;

        progressBar.fillAmount = timer / delay; // 更新进度条
        progressText.text = $"{timer:F1}/{delay}"; // 使用字符串插值和格式化
    }

    private void CompleteLoading()
    {
        PlayerPrefs.SetInt("PointState", 1); //1 成功 0 死亡 -1无数据/已读取
        SceneLoader.Instance.SwichScene();
        ShowPortalBar(false); // 隐藏并销毁进度条
    }

    private void ShowPortalBar(bool show)
    {
        if (show && portalBarInstance == null)
        {
            portalBarInstance = Instantiate(portalBarPrefab, Quaternion.identity,canvas.transform);

            progressBar = portalBarInstance.transform.GetChild(1).GetComponent<Image>();
            progressText = portalBarInstance.transform.GetChild(2).GetComponent<Text>();
            timer = 0.0f; // 重置计时器
        }
        else if (!show && portalBarInstance != null)
        {
            Destroy(portalBarInstance);
            portalBarInstance = null;
        }
    }
}
