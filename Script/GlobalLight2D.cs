using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class GlobalLight2D : MonoBehaviour
{
    private Light2D globalLight2D;

    // Start is called before the first frame update
    void Start()
    {
        globalLight2D = GetComponent<Light2D>();//获取全局光照的Light2D组件
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 调整全局光照的颜色
    /// </summary>
    public void SetGlobalLightColor(Color color)
    {
        globalLight2D.color = color;
    }

    /// <summary>
    /// 调整全局光照的亮度
    /// </summary>
    public void SetGlobalLightIntensity(float intensity)
    {
        globalLight2D.intensity = intensity;
    }
}
