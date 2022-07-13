using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalEffectDestroy : MonoBehaviour
{
    [Header("特效持续时间")]
    public float effectTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, effectTime);//时间到后销毁对象
    }

    // Update is called once per frame
    void Update()
    {

    }
}
