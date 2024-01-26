using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuns
{
    void Shoot();
    IEnumerator ReLoad(float reloading_time);
}
