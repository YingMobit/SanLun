using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMountor : MonoBehaviour
{
    public AnimationData Animation;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ChooseAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChooseAnimation()
    {
        int seed = DateTime.Now.GetHashCode();
        System.Random rand = new System.Random(seed);
        int choose = rand.Next(0, Animation.AnimatorControllers.Count);
        animator.runtimeAnimatorController = Animation.AnimatorControllers[choose];
    }

}
