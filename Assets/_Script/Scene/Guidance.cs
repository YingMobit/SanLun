using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Guidance : MonoBehaviour
{
    // ����
    public GameObject Barrier;          // ������ҳ�ʼ��λ
                                        //Goal�������������
                                        //TODO�����һ��panel��Base����������Ҫ�ӳ�����,�󶨰�ť
                                        //TODO����Base����һ��canvas-panel��һ�������������Խ�����Ϸ
                                        //TODO��������Ϸ��ʾwasd�����ƶ����߳���һ����trigger�ж�������Ի���ˢһ��sprite����ʼ�Ի�����

    //SETTING����Ⱦ��������Ψһ�����ؾ��������ȫ����Ⱦ��ɱ�ֵ�exp���ǻ����������������ڻ���һ�������Ϻ�Ϳ����ҵ����ڣ�˵�壩���������ﵫ��һ��ʧ�ܣ��㽫��ٲ���������0���룩
    //CONVERSATION��SETTING�����붴�ھͿ��Ե���硣�ṩһ�����֡�
    //TODO��������Ϸ��CONVERSATIOn
    //CONVERSATION�����ﱻ��Ⱦ��̩�������ˣ���Ĵ��������ֱ������ˣ�������Ҫ���������Ҿ�������Ѻ������Ƴ��ˣ�����������ֿ���׼�Ĵ��е����ˣ��������Ϲ��򣬳��ڹ���
    //TODO��ʼ�������������һ���Ի�
    //TODO��TImeLineչʾ��������

    // ����
    private void Start()
    {
        Barrier.SetActive(true);

    }
    private void GuideMove()
    {

    }
}
