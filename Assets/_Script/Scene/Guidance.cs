using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Guidance : MonoBehaviour
{
    // 声明
    public GameObject Barrier;          // 限制玩家初始走位
                                        //Goal：用来引导玩家
                                        //TODO：添加一个panel在Base场景激活需要从出口走,绑定按钮
                                        //TODO：在Base设置一个canvas-panel，一个背景，按键以进入游戏
                                        //TODO：进入游戏提示wasd进行移动，走出第一步用trigger判定，激活对话，刷一个sprite，开始对话引导

    //SETTING：污染者这里是唯一净土秘境，，外界全被污染，杀怪的exp就是魂力，提升能力，在击溃一定的屏障后就可以找到出口（说清），返回这里但是一旦失败，你将万劫不复（积分0减半）
    //CONVERSATION：SETTING，进入洞口就可以到外界。提供一个左轮。
    //TODO：进入游戏后，CONVERSATIOn
    //CONVERSATION：这里被污染的泰国严重了，你的大威力左轮被削弱了，现在需要换弹，但我尽力帮你把后坐力移除了，这下你可以又快又准的打中敌人了，告诉屏障规则，出口规则
    //TODO：始终是左键进入下一个对话
    //TODO：TImeLine展示怪物屏障

    // 函数
    private void Start()
    {
        Barrier.SetActive(true);

    }
    private void GuideMove()
    {

    }
}
