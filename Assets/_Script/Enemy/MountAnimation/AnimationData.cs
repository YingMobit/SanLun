using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "AnimationData", order = 1)]

public class AnimationData : ScriptableObject
{
    public List<RuntimeAnimatorController> AnimatorControllers;
}
