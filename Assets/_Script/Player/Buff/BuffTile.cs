
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class BuffTile : MonoBehaviour
{
    public Text Internalcode;
    public Text Buffname;
    public Text Buffdescription;
    public GameObject BackGround;
    public Button button;
    public List<BuffData> Data;
    private BuffData selected_data;
    public BuffFunc Function;

    public int BuffCount;
    public string FuncName;
    public bool Active;
    void Start()
    {
        button = GetComponent<Button>();
        Function = GameObject.Find("BuffFunc").GetComponent<BuffFunc>();
        Internalcode = transform.Find("Text (Internal_code)").GetComponent<Text>();
        Buffname = transform.Find("Text (BuffName)").GetComponent<Text>();
        Buffdescription = transform.Find("Text (Buffdescription)").GetComponent<Text>();
    }
    private void Update()
    {
        if (!Active)
        {
            SelectBuff();
            AddFunc();
            Active = true;
        }
    }
    void SelectBuff()
    {
        Debug.Log("SelectBuff");
        int seed = DateTime.Now.GetHashCode()+Mathf.RoundToInt(gameObject.transform .position.x);
        System.Random random = new System.Random(seed);
        BuffCount = random.Next(0,Data.Count);
        selected_data = Data[BuffCount];
        FuncName = "Buff_" + BuffCount;
        Internalcode.text = selected_data.InternalCode.ToString();
        Buffname.text = selected_data.BuffName.ToString();
        Buffdescription.text = selected_data.Buffdescription.ToString();
    }

    void AddFunc()
    {
        Debug.Log("Add");
        var func = Function.GetType().GetMethod(FuncName);
        button.onClick.RemoveAllListeners();
        if (func != null) button.onClick.AddListener(() => { func.Invoke(Function, null); });
        else Debug.Log("未找到对应Buff功能");
    }

    private void OnDisable()
    {
        Active = false;
    }
}
