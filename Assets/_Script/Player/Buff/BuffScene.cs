using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BuffScene : MonoBehaviour
{
    public BGMController bgmController;
    public ExpCountor exp_countor;
    public GameObject BuffUI;
    public AudioClip LevelUP;

    // Start is called before the first frame update
    void Start()
    {
        exp_countor = GameObject.Find("ExpManager").GetComponent<ExpCountor>();
        exp_countor.LevelUPed += BuffChoose;
    }

    void BuffChoose() { StartCoroutine(BuffChoose_IE()); }
    IEnumerator BuffChoose_IE()
    {
        bgmController.PLayAudio(LevelUP);
        yield return new WaitForSeconds(LevelUP.length);
        for (int i = 0; i < BuffUI.transform.childCount; i++) { GameObject child = BuffUI.transform.GetChild(i).gameObject; child.SetActive(true); }
        BuffUI.SetActive(true);
        Time.timeScale =0f;
    }
}
