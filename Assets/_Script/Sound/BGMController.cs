using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{

    public static BGMController BGM;
    public AudioSource Audio;

    // Start is called before the first frame update
    void Awake()
    {
        if (BGM == null)
        {
            BGM = this;
            DontDestroyOnLoad(BGM);
        }
        else Destroy(gameObject);
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
