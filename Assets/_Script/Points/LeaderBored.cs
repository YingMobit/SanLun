using System.Collections;
using System.Drawing;
using System.Linq; // Add this for sorting
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBored : MonoBehaviour
{
    [Header("��վ��Ϣ")]
    private const string url = "http://www.dreamlo.com/lb/";
    private const string privateCode = "qpe5-EINCEuWoP6uphLelwIa9J5jQ_E0W69gm5p7NPiA";
    private const string publicCode = "65de00218f40bbbe889c871e";
    [Header("�������а�")]
    private int maxRetries = 3;         // ��������
    private RootObject rootObject;            // ���ص������а����� json��ʽ���Ѿ�����֣�
    [Header("չʾ����")]
    public GameObject scorePrefab;          // Ԥ�����
    public Transform scoreParent;           // content
    [Header("��ť")]
    private bool isLevel;           // �Ƿ���LevelButton
    public GameObject LevelButton;          //�л��ȼ���
    public GameObject PointButton;          //�л����ְ�
    [Header("��ť")]
    public GameObject PopPanel;            // ����
    public Text PopText;            //�����ı�


    [System.Serializable]
    public class Score
    {
        public string name;
        public int score;
        public int seconds;// ��seconds��ʾlevel һ��ע��
        public string text;
        public string date;
    }

    [System.Serializable]
    public class Leaderboard
    {
        public Score[] entry;
    }

    [System.Serializable]
    public class Dreamlo
    {
        public Leaderboard leaderboard;
    }

    [System.Serializable]
    public class RootObject
    {
        public Dreamlo dreamlo;
    }

    // ����
    // �ⲿ����
    public void AddScore()
    {
        //HASDO�������Զ�����CHANGE:�����޷���עֱ���ڻص����������ʱ�����
        string playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1000, 10000).ToString());
        int score = PlayerPrefs.GetInt("Point", 0);
        int level = PlayerPrefs.GetInt("Level", 1);
        if (PlayerPrefs.GetInt("PointState", -1) == 0)
        {
            score = score / 2;
        }
        else if (PlayerPrefs.GetInt("PointState", -1) == 1)
        {
            score = score;
        }
        StartCoroutine(AddNewPlayer(playerName, score,level));
    }

    public void ShowScoresLeader()
    {
        GameObject button = SwitchLeader();
        button.GetComponent<Button>().interactable = false;

        ClearExistingScores(); // ������

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // ����
            var sortedScores = rootObject.dreamlo.leaderboard.entry.ToList();
            sortedScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            for (int i = 0; i < sortedScores.Count; i++)
            {
                DisplayScore(sortedScores[i], i + 1);
            }
        }
        button.GetComponent<Button>().interactable = true;
    }

    public void ShowLevelsLeader()
    {
        //rootObject = JsonUtility.FromJson<RootObject>(jsonText);// ͨ����scroes��process�Ѿ���ȡ����rootObject

        //SUSDO:ֻ����true��ʱ�������ʾ��ť
        GameObject button = SwitchLeader();
        button.GetComponent<Button>().interactable = false;

        ClearExistingScores(); // ������

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // ����
            var sortedLevels = rootObject.dreamlo.leaderboard.entry.ToList();
            sortedLevels.Sort((s1, s2) => s2.seconds.CompareTo(s1.seconds));

            for (int i = 0; i < sortedLevels.Count; i++)
            {
                DisplayLevel(sortedLevels[i], i + 1);
            }
        }
        button.GetComponent<Button>().interactable = true;
    }



    // �ڲ�����
    private void Start()
    {
        if(PlayerPrefs.GetInt("PointState", -1) == 1 || PlayerPrefs.GetInt("PointState", -1) == 0)
        {
            AddScore();
        }
        StartCoroutine(DownLoad()); // ��ȡ
        isLevel = false;
        PopPanel.SetActive(false);
    }

    IEnumerator AddNewPlayer(string playerName, int score,int level, int attempt = 0)
    {
        Debug.Log(score);
        Debug.Log(level);
        UnityWebRequest request = UnityWebRequest.Get(url + privateCode + "/add/" + UnityWebRequest.EscapeURL(playerName) + "/" + score + "/" + level);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            //Debug.LogError("Error adding score: " + request.error);
            if (attempt < maxRetries)
            {
                Debug.Log("Retrying to add score...");
                StartCoroutine(AddNewPlayer(playerName, score, attempt + 1));
            }
            else
            {
                PopMessage("�ɼ��ύ�����а�ʧ��,��������");
            }
        }
        PlayerPrefs.SetInt("PointState", -1);
        //HASDO:���һ��Panel������ʾ�������,��addTextComponent�޸ĳɵ���
    }

    IEnumerator DownLoad(int attempt = 0)
    {
        //TODO:����߼�����ʱ�򵽵�����
        UnityWebRequest request = UnityWebRequest.Get(url + publicCode + "/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            //Debug.LogError("Error getting scores: " + request.error);
            if (attempt < maxRetries)
            {
                Debug.Log("Retrying to get scores...");
                StartCoroutine(DownLoad(attempt + 1));
            }
            else
            {
                PopMessage("���а�����,��������");
            }
        }
        else
        {
            Debug.Log("Scores received: " + request.downloadHandler.text);
            ProcessScores(request.downloadHandler.text);
        }
    }

    private void ProcessScores(string jsonText)
    {
        jsonText = FormatJsonForArray(jsonText);

        rootObject = JsonUtility.FromJson<RootObject>(jsonText);

        ClearExistingScores(); // ������

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // ����
            var sortedScores = rootObject.dreamlo.leaderboard.entry.ToList();
            sortedScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            for (int i = 0; i < sortedScores.Count; i++)
            {
                DisplayScore(sortedScores[i], i + 1);
            }
        }
        else
        {
            PopMessage("���а����ݶ�ʧ");
        }
    }

    private string FormatJsonForArray(string jsonText)
    {
        // �ĳ�����
        if (jsonText.Contains("\"entry\":{"))
        {
            jsonText = jsonText.Replace("\"entry\":{", "\"entry\":[{");
            jsonText = jsonText.Replace("}}}}", "}]}}}");
            Debug.Log(jsonText);
        }
        return jsonText;
    }

    private void ClearExistingScores()
    {
        foreach (Transform child in scoreParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void DisplayScore(Score entry, int rank)
    {
        GameObject newScoreRow = Instantiate(scorePrefab, scoreParent);
        Text[] texts = newScoreRow.GetComponentsInChildren<Text>();
        if (texts.Length >= 3)
        {
            texts[0].text = "No." + rank.ToString();
            texts[1].text = entry.name;
            texts[2].text = entry.score.ToString() + "��";
        }
    }

    private void DisplayLevel(Score entry, int rank)
    {
        GameObject newScoreRow = Instantiate(scorePrefab, scoreParent);
        Text[] texts = newScoreRow.GetComponentsInChildren<Text>();
        if (texts.Length >= 3)
        {
            texts[0].text = "No." + rank.ToString();
            texts[1].text = entry.name;
            texts[2].text = "Lv." + entry.seconds.ToString();
        }
    }

    private GameObject SwitchLeader()
    {
        isLevel = !isLevel;
        LevelButton.SetActive(!LevelButton.activeSelf);
        PointButton.SetActive(!PointButton.activeSelf);
        if(LevelButton.activeSelf)
        {
            return LevelButton;
        }
        else
        {
            return PointButton;
        }
    }

    private void PopMessage(string message)
    {
        PopPanel.SetActive(true);
        PopPanel.transform.GetChild(1).gameObject.SetActive(true);
        PopText.text = message;
    }
}
