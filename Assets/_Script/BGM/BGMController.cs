using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{ 
    public AudioSource audioSource;
    public void PLayAudio(AudioClip Audio) { audioSource.PlayOneShot(Audio);}
    
}
