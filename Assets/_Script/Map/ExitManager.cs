using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ExitManager : MonoBehaviour
{
    [Header("�������")]
    public float delay = 6.0f; // �����Ҫͣ����ʱ��
    private float timer = 0.0f; // ��ʱ��
    private bool isPlayerInTrigger = false; // ����Ƿ��ڴ��������ڵı�־

    [Header("Portal Bar Prefab")]
    public GameObject portalBarPrefab; // ����bar��Ԥ����
    private GameObject portalBarInstance; // portalBar��ʵ��
    public GameObject canvas; // ����
    private UnityEngine.UI.Image progressBar; // ������Image���
    private Text progressText; // ������Text���

    private void Update()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        if (isPlayerInTrigger || (portalBarInstance != null && timer > 0))
        {
            UpdateLoadTime();
            UpdateProgressBar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            isPlayerInTrigger = true;
            ShowPortalBar(true); // ��������ʾ������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited");
            isPlayerInTrigger = false;
            // ���������ؽ���������������Ȼ�ݼ���0����ʧ
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
        else if (timer <= 0.0f && portalBarInstance != null && !isPlayerInTrigger)
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
        PlayerPrefs.SetInt("PointState", 1); //1 �ɹ� 0 ���� -1 ������/�Ѷ�ȡ
        ShowPortalBar(false); // ���ز����ٽ�����
        SceneLoader.Instance.SwichScene();
    }

    private void ShowPortalBar(bool show)
    {
        if (show && portalBarInstance == null)
        {
            portalBarInstance = Instantiate(portalBarPrefab, Vector3.zero, Quaternion.identity, canvas.transform);//new Vector3(500f, 428f, 0)
            //RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            RectTransform portalBarRectTransform = portalBarInstance.GetComponent<RectTransform>();
            //float positionY = (canvasRectTransform.sizeDelta.y / 4); // �����߶ȵ�1/
            portalBarRectTransform.anchoredPosition = new Vector2(0, 380f);
            progressBar = portalBarInstance.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
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
