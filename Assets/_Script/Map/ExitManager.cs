using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ExitManager : MonoBehaviour
{
    [Header("�������")]
    public float delay = 1.0f; // �����Ҫͣ����ʱ��
    private float timer = 0.0f; // ��ʱ��
    private bool isPlayerInTrigger = false; // ����Ƿ��ڴ��������ڵı�־

    [Header("Portal Bar Prefab")]
    public GameObject portalBarPrefab; // ����bar��Ԥ����
    public GameObject ArrowPrefab;            //����
    private GameObject portalBarInstance; // portalBar��ʵ��
    private GameObject arrow;           // ʵ��
    public GameObject canvas; // ����
    public Transform player;
    public float visibilityDistance = 20f; // ���Ӿ���
    private UnityEngine.UI.Image progressBar; // ������Image���
    private Text progressText; // ������Text���


    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }
        arrow = Instantiate(ArrowPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        arrow.GetComponent<TrackingArrow>().exit = gameObject.transform;
        arrow.GetComponent<TrackingArrow>().player = player;
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }
        if (isPlayerInTrigger || (portalBarInstance != null && timer > 0))
        {
            UpdateLoadTime();
            UpdateProgressBar();
        }
        if(arrow!=null)
        {
            ShowArrow();
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
        PlayerPrefs.SetInt("Point", PlayerPrefs.GetInt("Point", 0) + Health.BarrierDestroyCount + Health.ExitGenerateCount * 100);
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

    private void ShowArrow()
    {
        float distance = Vector3.Distance(player.position, gameObject.transform.position);
        if(distance> visibilityDistance)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
