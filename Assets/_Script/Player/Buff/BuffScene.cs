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
        BuffUI.SetActive(true);
    }
}
