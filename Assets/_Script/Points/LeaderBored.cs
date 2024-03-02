using System.Collections;
using System.Drawing;
using System.Linq; // Add this for sorting
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBored : MonoBehaviour
{
    [Header("网站信息")]
    private const string url = "http://www.dreamlo.com/lb/";
    private const string privateCode = "qpe5-EINCEuWoP6uphLelwIa9J5jQ_E0W69gm5p7NPiA";
    private const string publicCode = "65de00218f40bbbe889c871e";
    [Header("下载排行榜")]
    private int maxRetries = 3;         // 重连次数
    private RootObject rootObject;            // 下载到的排行榜数据 json格式（已经用类分）
    [Header("展示排行")]
    public GameObject scorePrefab;          // 预制体框
    public Transform scoreParent;           // content
    [Header("按钮")]
    private bool isLevel;           // 是否是LevelButton
    public GameObject LevelButton;          //切换等级榜
    public GameObject PointButton;          //切换积分榜
    [Header("按钮")]
    public GameObject PopPanel;            // 弹窗
    public Text PopText;            //弹窗文本


    [System.Serializable]
    public class Score
    {
        public string name;
        public int score;
        public int seconds;// 用seconds表示level 一定注意
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

    // 函数
    // 外部函数
    public void AddScore()
    {
        //HASDO：数据自动传递CHANGE:由于无法备注直接在回到这个场景的时候添加
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

        ClearExistingScores(); // 清数据

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // 排序
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
        //rootObject = JsonUtility.FromJson<RootObject>(jsonText);// 通过对scroes的process已经获取到了rootObject

        //SUSDO:只有在true的时候才能显示按钮
        GameObject button = SwitchLeader();
        button.GetComponent<Button>().interactable = false;

        ClearExistingScores(); // 清数据

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // 排序
            var sortedLevels = rootObject.dreamlo.leaderboard.entry.ToList();
            sortedLevels.Sort((s1, s2) => s2.seconds.CompareTo(s1.seconds));

            for (int i = 0; i < sortedLevels.Count; i++)
            {
                DisplayLevel(sortedLevels[i], i + 1);
            }
        }
        button.GetComponent<Button>().interactable = true;
    }



    // 内部函数
    private void Start()
    {
        if(PlayerPrefs.GetInt("PointState", -1) == 1 || PlayerPrefs.GetInt("PointState", -1) == 0)
        {
            AddScore();
        }
        StartCoroutine(DownLoad()); // 获取
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
                PopMessage("成绩提交至排行榜失败,请检查网络");
            }
        }
        PlayerPrefs.SetInt("PointState", -1);
        //HASDO:添加一个Panel用来提示输入错误,将addTextComponent修改成弹窗
    }

    IEnumerator DownLoad(int attempt = 0)
    {
        //TODO:解决逻辑下载时候到底在哪
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
                PopMessage("排行榜下载,请检查网络");
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

        ClearExistingScores(); // 清数据

        if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
        {
            // 排序
            var sortedScores = rootObject.dreamlo.leaderboard.entry.ToList();
            sortedScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

            for (int i = 0; i < sortedScores.Count; i++)
            {
                DisplayScore(sortedScores[i], i + 1);
            }
        }
        else
        {
            PopMessage("排行榜数据丢失");
        }
    }

    private string FormatJsonForArray(string jsonText)
    {
        // 改成数组
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
            texts[2].text = entry.score.ToString() + "分";
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
