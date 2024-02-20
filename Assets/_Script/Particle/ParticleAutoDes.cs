using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDes : MonoBehaviour
{
    ParticleSystem me;
    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (me && !me.IsAlive()) Destroy(gameObject);
    }
}
