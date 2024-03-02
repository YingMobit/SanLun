
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
    public RectTransform Recttransform;
    public GameObject BackGround;
    public Button button;
    public List<BuffData> Data;
    private BuffData selected_data;
    public BuffFunc Function;
    public Image HighLight;

    public int BuffCount;
    public string FuncName;
    public bool Active;
    void Start()
    {
        Recttransform = GetComponent<RectTransform>();
        button = GameObject.Find("Confirm").GetComponent<Button>();
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
        int seed = DateTime.Now.GetHashCode()+Mathf.RoundToInt(transform.position.x)*1000;
        System.Random random = new System.Random(seed);
        BuffCount = random.Next(0,Data.Count);
        selected_data = Data[BuffCount];
        FuncName = "Buff_" + BuffCount;
        Internalcode.text = selected_data.InternalCode.ToString();
        Buffname.text = selected_data.BuffName.ToString();
        Buffdescription.text = selected_data.Buffdescription.ToString();
    }

    public void AddFunc()
    {
        var func = Function.GetType().GetMethod(FuncName);
        button.onClick.RemoveAllListeners();
        if (func != null) button.onClick.AddListener(() => { func.Invoke(Function, null); });
        HighLight.enabled = true;
        HighLight.rectTransform.position = Recttransform.position;
    }

    private void OnDisable()
    {
        Active = false;
    }
}
