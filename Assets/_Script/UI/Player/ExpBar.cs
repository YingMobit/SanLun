using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{

    public Image Exp;
    public Text ExpBarText;
    public Text Level;
    public ExpCountor expcountor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Exp.fillAmount = (float)expcountor.CorrentExp / expcountor.CorrentLevelExp;
        ExpBarText.text = expcountor.CorrentExp.ToString()+"/"+expcountor.CorrentLevelExp.ToString();
        Level.text = "CurrentLevel  " + expcountor.CorrentLevel.ToString();
    }
}
