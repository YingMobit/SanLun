using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Guidance : MonoBehaviour
{
    // ����
    public GameObject player;           // ���
    public GameObject DialogPanel;          // �Ի�panel
    public GameObject GuideMovePanel;           // ָ���ƶ�Panel
    public GameObject PointPanel;           // ����
    public Text DialogText;         // �Ի�����ı� ����Ĭ���ȸ�NPC //TODO��������Ƿ���Ҫһ��Panel

    private int page;           // ҳ��
    private string text = "���ӣ������������ˡ�\r\n����������ʧ�˲��ּ��䣿\r\n���£����ӡ�\r\n�ҽ��������һ�С�\r\n����������͵ĵ�12�꣬���ǵĴ�½��һ��а���ħ������Ⱦ�����ħ���������˴�������Ⱦ�ߡ�\r\n��Щ�����Ϊ���࣬�������㽫Ҫ��Եĵ��ˡ�\r\n����ͨ���ƺ�����Ϊ�߼���Ⱦ�ߣ�\"������ħ\"���м���Ⱦ�ߣ�\"Ӱצ����\"���ͼ���Ⱦ�ߣ�\"ţͷ����\"\r\n���ǣ����ӣ����Ѿ�Ϊ��׼����Ӧ��֮�ߡ�\r\n�Ұ����޸��˻���ת��װ�ã����װ�������ܹ���ɱ��Щ��������������ϼ�ȡ������\r\n��Щ�����������ǿ�Ĺؼ���ÿ�����ɱ���ˣ�����ܻ�þ���ֵ����ÿ���������㶼����ѡ��һ��buff��ǿ���Լ���\r\n��Ϊ��ѡ�ߣ�������ӵ�м���������������������Ψһ��ϣ����\r\n������Щ�Ҵ�����Ϊ��½����Ⱦ�������Ѷ�ܵ��ؾ������档\r\n�������������������������ѡ�����������ǣ��Կ���Щ��Ⱦ�ߣ���Ѱ���ؽ����Ǽ�԰��ϣ����\r\n������\r\nǰ����½��ͨ������ʼ��Ϸ�������Ǹ����ڣ����ұ�ľ�ݵ���ʿ������Դ�����������ѡ�ߵ���Ϣ�����а񡿡�";           // �Ի��ı�
    private string[] sentences;         // �Ի��ı��ָ�汾
    private bool IsLastPage => page == sentences.Length-1;         // ���һҳ

                                        
    //Goal�������������
    //SETTING����Ⱦ��������Ψһ�����ؾ��������ȫ����Ⱦ��ɱ�ֵ�exp���ǻ����������������ڻ���һ�������Ϻ�Ϳ����ҵ����ڣ�˵�壩���������ﵫ��һ��ʧ�ܣ��㽫��ٲ���������0���룩
    //CONVERSATION��SETTING�����붴�ھͿ��Ե���硣�ṩһ�����֡�
    //CONVERSATION�����ﱻ��Ⱦ��̫�������ˣ���Ĵ��������ֱ������ˣ�������Ҫ���������Ҿ�������Ѻ������Ƴ��ˣ�����������ֿ���׼�Ĵ��е����ˣ��������Ϲ��򣬳��ڹ���
    //TODO��TImeLineչʾ��������

    // ����
    private void Start()
    {
        DialogPanel.SetActive(false);
        GuideMovePanel.SetActive(false);
        if (PlayerPrefs.GetInt("IsFirst", 1) == 1)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("Moving", false);
            player.GetComponent<BasePlayer>().enabled = false;
            GuideMove();
        }
        else
        {
            if (PlayerPrefs.GetInt("Pointstate", -1) == 1 || PlayerPrefs.GetInt("Pointstate", -1) == 0)
            {
                StartCoroutine(ShowPoint());
            }
        }
    }
    private void Update()
    {
        if(DialogPanel.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextPage();//isLastPage
            }
        }
    }
    private void GuideMove()
    {
        //HASDO:������Ϊ��һ�ν��룬������ָ����ҽ����ƶ����ƶ�֮�󵯳��Ի����������ָ��
        StartCoroutine(ShowGuideMove());
    }
    
    IEnumerator DetectMove()
    {
        while(!Input.GetKeyDown(KeyCode.W)&& !Input.GetKeyDown(KeyCode.A)&& !Input.GetKeyDown(KeyCode.S)&& !Input.GetKeyDown(KeyCode.D))
        {
            yield return null;
        }
        //�ƶ���
        //HASDO:�����ջ�guidemovepanel
        StartCoroutine(WithdrawGuideMove());
        yield return new WaitForSecondsRealtime(1.5f);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("Moving", false);
        player.GetComponent<BasePlayer>().enabled = false;
        GuideSetting();
    }
    private void GuideSetting()
    {
        //TODO:��Ӧ����StopӦ���ǽ�������ƶ�
        DialogPanel.SetActive(true);
        sentences = text.Split(
            new[] { "\r\n","\r","\n"},
            System.StringSplitOptions.RemoveEmptyEntries
        );
        page = 0;
        DialogText.text = sentences[page];
    }
    private void NextPage()
    {
        if(!IsLastPage)
        {
            page++;
            DialogText.text = sentences[page];
            StartCoroutine(DialogShake());
        }
        else
        {
            DialogPanel.SetActive(false);
            PlayerPrefs.SetInt("IsFirst", 0);
            player.GetComponent<BasePlayer>().enabled = true;
        }
    }

    IEnumerator DialogShake()
    {
        DialogPanel.transform.position += new Vector3(0, 2,0);
        yield return new WaitForSecondsRealtime(0.05f);
        DialogPanel.transform.position -= new Vector3(0, 4, 0);
        yield return new WaitForSecondsRealtime(0.05f);
        DialogPanel.transform.position += new Vector3(0, 2, 0);
    }

    IEnumerator ShowGuideMove()
    {
        yield return new WaitForSecondsRealtime(1f);

        GuideMovePanel.SetActive(true);
        float duration = 0.6f; // �ƶ�����ʱ��
        float time = 0; // �Ѿ���ȥ��ʱ��
        RectTransform rectTransform = GuideMovePanel.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector3(-380.5f, -111.01f); // ��ʼλ��
        Vector2 endPosition = new Vector3(-380.5f, 111.01f); // ����λ��

        while (time < duration)
        {
            // �ڿ�ʼ�ͽ���λ��֮���ֵλ��
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime; // ���ӹ�ȥ��ʱ��
            yield return null; // �ȴ���һ֡
        }

        rectTransform.anchoredPosition = endPosition; // ȷ���ƶ�������λ��

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("Moving", false);
        player.GetComponent<BasePlayer>().enabled = true;
        StartCoroutine(DetectMove());
    }

    IEnumerator WithdrawGuideMove()
    {
        float duration = 1.5f; // �ƶ�����ʱ��
        float time = 0; // �Ѿ���ȥ��ʱ��
        RectTransform rectTransform = GuideMovePanel.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector3(-380.5f, 111.01f); // ��ʼλ��
        Vector2 endPosition = new Vector3(-380.5f, -111.01f); // ����λ��

        while (time < duration)
        {
            // �ڿ�ʼ�ͽ���λ��֮���ֵλ��
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime; // ���ӹ�ȥ��ʱ��
            yield return null; // �ȴ���һ֡
        }

        rectTransform.anchoredPosition = endPosition; // ȷ���ƶ�������λ��
        GuideMovePanel.SetActive(false);
    }

    IEnumerator ShowPoint()
    {
        yield return new WaitForSecondsRealtime(1f);

        GuideMovePanel.SetActive(true);
        float duration = 1f; // �ƶ�����ʱ��
        float time = 0; // �Ѿ���ȥ��ʱ��
        RectTransform rectTransform = GuideMovePanel.GetComponent<RectTransform>();
        if(PlayerPrefs.GetInt("Pointstate", -1) == 1)
        {
            GuideMovePanel.GetComponent<Text>().text = "��һ�غ�\r\n���֣�" + PlayerPrefs.GetInt("Point", 0) + "\r\n�ȼ���" + PlayerPrefs.GetInt("Level", 1);
        }
        else if(PlayerPrefs.GetInt("Pointstate", -1) == 0)
        {
            GuideMovePanel.GetComponent<Text>().text = "��һ�غ�\r\n���֣�" + PlayerPrefs.GetInt("Point", 0) / 2 + "(�������)\r\n�ȼ���" + PlayerPrefs.GetInt("Level", 1);
        }

        Vector2 startPosition = new Vector3(-380.5f, -111.01f); // ��ʼλ��
        Vector2 endPosition = new Vector3(-380.5f, 111.01f); // ����λ��

        while (time < duration)
        {
            // �ڿ�ʼ�ͽ���λ��֮���ֵλ��
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime; // ���ӹ�ȥ��ʱ��
            yield return null; // �ȴ���һ֡
        }

        rectTransform.anchoredPosition = endPosition; // ȷ���ƶ�������λ��
        StartCoroutine(WithdrawPoint());
    }

    IEnumerator WithdrawPoint()
    {
        yield return new WaitForSecondsRealtime(3f);
        float duration = 1f; // �ƶ�����ʱ��
        float time = 0; // �Ѿ���ȥ��ʱ��
        RectTransform rectTransform = GuideMovePanel.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector3(-380.5f, 111.01f); // ��ʼλ��
        Vector2 endPosition = new Vector3(-380.5f, -111.01f); // ����λ��

        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition; // ȷ���ƶ�������λ��
        GuideMovePanel.SetActive(false);
    }
}
