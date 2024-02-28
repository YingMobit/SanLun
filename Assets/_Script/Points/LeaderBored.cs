using System.Collections;
using System.Linq; // Add this for sorting
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
    private bool canSwitchLeader;           // 是否可以切换排行榜
    public GameObject scorePrefab;          // 预制体框
    public Transform scoreParent;           // content


    [System.Serializable]
    public class Score
    {
        public string name;
        public int score;
        public int level;
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
    public void ShowLeaderBored()
    {
        //TODO：添加一个按钮一旦进入trigger那么就激活按钮E，按下激活面板
    }
    public void AddScore()
    {
        //TODO：添加一个panel在Base场景激活需要从出口走,绑定按钮
        //TODO：数据自动传递
        //StartCoroutine(AddNewPlayer();
    }

    public void ShowLevelsLeader()
    {
        //rootObject = JsonUtility.FromJson<RootObject>(jsonText);// 通过对scroes的process已经获取到了rootObject
        if(canSwitchLeader)
        {
            canSwitchLeader = false;
            //TODO:只有在true的时候才能显示按钮
            ClearExistingScores(); // 清数据

            if (rootObject != null && rootObject.dreamlo.leaderboard.entry != null)
            {
                // 排序
                var sortedScores = rootObject.dreamlo.leaderboard.entry.ToList();
                sortedScores.Sort((s1, s2) => s2.score.CompareTo(s1.score));

                for (int i = 0; i < sortedScores.Count; i++)
                {
                    DisplayLevel(sortedScores[i], i + 1);
                }
            }
            canSwitchLeader = true;
        }
    }

    public void ShowScoresLeader()
    {
        if(canSwitchLeader)
        {
            canSwitchLeader = false;
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
            canSwitchLeader = true;
        }
    }

    // 内部函数
    private void Start()
    {
        //StartCoroutine(AddNewPlayer("13", 20,2,"good")); // 测试添加新玩家
        StartCoroutine(DownLoad()); // 获取
    }

    IEnumerator AddNewPlayer(string playerName, int score,int level,string text, int attempt = 0)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + privateCode + "/add/" + UnityWebRequest.EscapeURL(playerName) + "/" + score + "/" + level + "/" +text);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error adding score: " + request.error);
            if (attempt < maxRetries)
            {
                Debug.Log("Retrying to add score...");
                StartCoroutine(AddNewPlayer(playerName, score,level,text, attempt + 1));
            }
        }
        else
        {
            //TODO:添加一个Panel用来提示输入错误
            Debug.Log("Score added successfully!");
        }
    }

    IEnumerator DownLoad(int attempt = 0)
    {
        canSwitchLeader = false;
        if (scoreParent.gameObject.GetComponent<Text>() != null)
        {
            Destroy(scoreParent.gameObject.GetComponent<Text>());
        }
        UnityWebRequest request = UnityWebRequest.Get(url + publicCode + "/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error getting scores: " + request.error);
            if (attempt < maxRetries)
            {
                Debug.Log("Retrying to get scores...");
                StartCoroutine(DownLoad(attempt + 1));
            }
            else
            {
                scoreParent.gameObject.AddComponent<Text>().text = "排行榜获取失败";
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
            canSwitchLeader = true;
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
        if (texts.Length >= 4)
        {
            texts[0].text = rank.ToString();
            texts[1].text = entry.name;
            texts[2].text = entry.score.ToString();
            texts[3].text = entry.text;
        }
    }

    private void DisplayLevel(Score entry, int rank)
    {
        GameObject newScoreRow = Instantiate(scorePrefab, scoreParent);
        Text[] texts = newScoreRow.GetComponentsInChildren<Text>();
        if (texts.Length >= 4)
        {
            texts[0].text = rank.ToString();
            texts[1].text = entry.name;
            texts[2].text = entry.level.ToString();
            texts[3].text = entry.text;
        }
    }
}
