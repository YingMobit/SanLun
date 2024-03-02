using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    [Header("�������")]
    public float delay = 6.0f; // �����Ҫͣ����ʱ��
    private float timer = 0.0f; // ��ʱ��
    private bool isPlayerInTrigger = false; // ����Ƿ��ڴ��������ڵı�־

    [Header("Portal Bar Prefab")]
    public GameObject portalBarPrefab; // ����bar��Ԥ����
    private GameObject portalBarInstance; // portalBar��ʵ��
    public GameObject canvas;           // ����
    private Image progressBar; // ������Image���
    private Text progressText; // ������Text���

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
            ShowPortalBar(true); // ��������ʾ������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            ShowPortalBar(false); // ���ز��������ٽ�����
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
            ShowPortalBar(false); // ���ز����ٽ�����
        }
    }

    private void UpdateProgressBar()
    {
        if (portalBarInstance == null) return;

        progressBar.fillAmount = timer / delay; // ���½�����
        progressText.text = $"{timer:F1}/{delay}"; // ʹ���ַ�����ֵ�͸�ʽ��
    }

    private void CompleteLoading()
    {
        PlayerPrefs.SetInt("PointState", 1); //1 �ɹ� 0 ���� -1������/�Ѷ�ȡ
        SceneLoader.Instance.SwichScene();
        ShowPortalBar(false); // ���ز����ٽ�����
    }

    private void ShowPortalBar(bool show)
    {
        if (show && portalBarInstance == null)
        {
            portalBarInstance = Instantiate(portalBarPrefab, Quaternion.identity,canvas.transform);

            progressBar = portalBarInstance.transform.GetChild(1).GetComponent<Image>();
            progressText = portalBarInstance.transform.GetChild(2).GetComponent<Text>();
            timer = 0.0f; // ���ü�ʱ��
        }
        else if (!show && portalBarInstance != null)
        {
            Destroy(portalBarInstance);
            portalBarInstance = null;
        }
    }
}
