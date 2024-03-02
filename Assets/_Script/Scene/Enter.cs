using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter : MonoBehaviour
{

    // º¯Êý
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerPrefs.SetInt("Point", 0);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("PointState", -1);
            SceneLoader.Instance.SwichScene();
        }
    }
}
