using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuffScene : MonoBehaviour
{

    public ExpCountor exp_countor;
    public GameObject BuffUI;

    // Start is called before the first frame update
    void Start()
    {
        //BuffUI = GameObject.Find("ChooseBackground");
        exp_countor = GameObject.Find("ExpManager").GetComponent<ExpCountor>();
        exp_countor.LevelUPed += BuffChoose;
    }
    void BuffChoose()
    {
        for (int i = 0; i < BuffUI.transform.childCount; i++) { GameObject child = BuffUI.transform.GetChild(i).gameObject; child.SetActive(true); }
        BuffUI.SetActive(true);
        Time.timeScale =0f;
    }
}
