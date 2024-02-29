using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Guidance : MonoBehaviour
{
    // 声明
    public GameObject player;           // 玩家
    public GameObject DialogPanel;          // 对话panel
    public GameObject GuideMovePanel;           // 指引移动Panel
    public Text DialogText;         // 对话框的文本 这里默认先给NPC //TODO：给玩家是否需要一个Panel

    private int page;           // 页数
    private string text = "孩子，你终于苏醒了。\r\n看起来你迷失了部分记忆？\r\n别害怕，孩子。\r\n我将会告诉你一切。\r\n现在是迷雾纪的第12年，我们的大陆被一种邪恶的魔气所污染，这股魔气催生出了大量的污染者。\r\n这些怪物分为三类，它们是你将要面对的敌人。\r\n我们通常称呼他们为高级污染者：\"瘴气巨魔\"，中级污染者：\"影爪幽灵\"，低级污染者：\"牛头卫兵\"\r\n但是，孩子，我已经为你准备了应对之策。\r\n我帮你修复了魂力转换装置，这个装置让你能够击杀这些怪物，并从它们身上汲取魂力。\r\n这些魂力将是你变强的关键，每当你击杀敌人，你就能获得经验值，而每次升级，你都可以选择一项buff来强化自己。\r\n作为天选者，你天生拥有几乎不死的生命，是我们唯一的希望。\r\n我们这些幸存者因为大陆的污染，不得已躲避到秘境中生存。\r\n在这里，我们依靠像你这样的天选者来保护我们，对抗那些污染者，并寻找重建我们家园的希望。\r\n。。。\r\n前往大陆的通道【开始游戏】就在那个洞口，在右边木屋的骑士处你可以打听到其他天选者的消息【排行榜】。";           // 对话文本
    private string[] sentences;         // 对话文本分割版本
    private bool IsLastPage => page == sentences.Length-1;         // 最后一页

                                        
    //Goal：用来引导玩家
    //SETTING：污染者这里是唯一净土秘境，，外界全被污染，杀怪的exp就是魂力，提升能力，在击溃一定的屏障后就可以找到出口（说清），返回这里但是一旦失败，你将万劫不复（积分0减半）
    //CONVERSATION：SETTING，进入洞口就可以到外界。提供一个左轮。
    //CONVERSATION：这里被污染的泰国严重了，你的大威力左轮被削弱了，现在需要换弹，但我尽力帮你把后坐力移除了，这下你可以又快又准的打中敌人了，告诉屏障规则，出口规则
    //TODO：TImeLine展示怪物屏障

    // 函数
    private void Start()
    {
        DialogPanel.SetActive(false);
        GuideMovePanel.SetActive(false);
        if (PlayerPrefs.GetInt("IsFirst", 1)==1)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("Moving", false);
            player.GetComponent<BasePlayer>().enabled = false;
            GuideMove();
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
        //HASDO:如果玩家为第一次进入，在这里指导玩家进行移动，移动之后弹出对话框进行新手指导
        StartCoroutine(ShowGuideMove());
    }
    
    IEnumerator DetectMove()
    {
        while(!Input.GetKeyDown(KeyCode.W)&& !Input.GetKeyDown(KeyCode.A)&& !Input.GetKeyDown(KeyCode.S)&& !Input.GetKeyDown(KeyCode.D))
        {
            yield return null;
        }
        //移动了
        //HASDO:向上收回guidemovepanel
        StartCoroutine(WithdrawGuideMove());
        yield return new WaitForSecondsRealtime(1.5f);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("Moving", false);
        player.GetComponent<BasePlayer>().enabled = false;
        GuideSetting();
    }
    private void GuideSetting()
    {
        //TODO:不应该是Stop应该是禁用玩家移动
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
        float duration = 0.6f; // 移动持续时间
        float time = 0; // 已经过去的时间
        Vector3 startPosition = new Vector3(734.49f, 567.83f, 0); // 开始位置
        Vector3 endPosition = new Vector3(734.47f, 462.16f, 0); // // debug出来下面位置(734.47f, 462.16f, 0),上面位置(734.49f, 567.83f, 0)

        while (time < duration)
        {
            // 在开始和结束位置之间插值位置
            GuideMovePanel.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime; // 增加过去的时间
            yield return null; // 等待下一帧
        }

        GuideMovePanel.transform.position = endPosition; // 确保移动到最终位置

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("Moving", false);
        player.GetComponent<BasePlayer>().enabled = true;
        StartCoroutine(DetectMove());
    }

    IEnumerator WithdrawGuideMove()
    {
        float duration = 1.5f; // 移动持续时间
        float time = 0; // 已经过去的时间
        Vector3 startPosition = new Vector3(734.47f, 462.16f, 0); // 开始位置
        Vector3 endPosition = new Vector3(734.49f, 567.83f, 0); // 结束位置
        //Debug.Log(GuideMovePanel.GetComponent<RectTransform>().sizeDelta.y);// debug出来高度！！！gousi的是错的105.67
        //Debug.Log(GuideMovePanel.transform.position);// debug出来位置(734.47, 462.16, 0),上面(734.49, 567.83, 0)

        while (time < duration)
        {
            // 在开始和结束位置之间插值位置
            GuideMovePanel.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime; // 增加过去的时间
            yield return null; // 等待下一帧
        }

        GuideMovePanel.transform.position = endPosition; // 确保移动到最终位置
        GuideMovePanel.SetActive(false);
    }
}
