using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("shake相机的动画组件")]
    public Animator cameraAnimator;

    public static CameraShake camShake;//存放CameraShake类自身的静态对象


    // Start is called before the first frame update
    void Start()
    {
        camShake = GetComponent<CameraShake>();//实例化CameraShake对象
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        cameraAnimator.SetTrigger("CameraShake");//启动相机震动动画
    }
}
