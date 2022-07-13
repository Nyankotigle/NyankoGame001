using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("玩家位置组件")]
    public Transform playerTransform; 
    [Header("不跟随镜头旋转的背景")]
    public Transform backgroundTransform;
    [Header("相机上的主音源")]
    public AudioSource mainAudioSource;
    [Header("全局光照")]
    public GameObject globalLightObj;
    private GlobalLight2D globalLight2d;//全局光照管理

    Vector2 camMoveBox;               //触发摄像机移动的活动范围

    public bool cameraIsMove;         //相机正在移动  

    public static bool cameraRotate;    //相机是否转动
    public static float cameraAngle;    //相机转动的角度

   
    private float warningLightRed;          //警报灯光的红色rgb值
    private bool isIncreaseRed;             //增加或者减少警报灯光的红色rgb值

    void Start()
    {
        camMoveBox = new Vector2(2, 2);        
        cameraIsMove = false;

        cameraAngle = 0f;
        cameraRotate = false;

        warningLightRed = 1;
        globalLight2d = globalLightObj.GetComponent<GlobalLight2D>();//获取全局光照的GlobalLight2D组件
    }

    // Update is called once per frame
    void Update()
    {

        if (cameraIsMove)//当镜头开始移动时跟随玩家，并且距离玩家一定范围时停止移动
        {
            FollowPlayer();
        }
        else
        {
            CheckBoundary();//当镜头未移动时检测玩家是否超出camMoveBox范围，若是，则移动镜头
        }

        CameraRotation();
        WarningLight();
    }


    /// <summary>
    /// 相机旋转函数
    /// </summary>
    public void CameraRotation()
    {
        if (cameraRotate)
        {
            cameraAngle += 0.2f;
            if (cameraAngle == 360f)
            {
                cameraAngle = 0f;
            }
            transform.localRotation = Quaternion.Euler(0, 0, cameraAngle);
            backgroundTransform.localRotation = Quaternion.Euler(0, 0, -cameraAngle);
        }        
    }

    /// <summary>
    /// 恢复相机初始位置
    /// </summary>
    public void StopCameraRotation()
    {
        //恢复背景音乐
        mainAudioSource.clip = Resources.Load<AudioClip>("musicFile/Title");
        mainAudioSource.Play();
        //恢复全局光照颜色
        globalLight2d.SetGlobalLightColor(new Color(1, 1, 1));
        //恢复相机和背景角度
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        backgroundTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    /// 警报灯光闪烁
    /// </summary>
    void WarningLight()
    {
        if (CameraFollow.cameraRotate)
        {
            if (warningLightRed >= 5.0f)//当red值大于等于5时开始减小
            {
                isIncreaseRed = false;
            }
            if (warningLightRed <= 1.0f)//当red值小于等于1时开始增加
            {
                isIncreaseRed = true;
            }
            //每一帧减小或者增加0.1
            if (isIncreaseRed)
            {
                warningLightRed += 0.1f;
            }
            else
            {
                warningLightRed -= 0.1f;
            }
            //通过warningLightRed设置全局光照的颜色实现灯光在浅红和深红之间循环渐变
            globalLight2d.SetGlobalLightColor(new Color(warningLightRed, 1, 1));
        }
    }

    public void FollowPlayer()
    {
        if(playerTransform == null)
        {
            return;
        }
        Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        //利用线性插值，相机每帧向玩家位置移动0.08倍的距离，控制相机平滑移动的关键
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.08f);
        //与玩家距离小于0.5时相机不移动
        float distance = Vector3.Distance(targetPos, transform.position);
        if (distance < 0.5f)
        {
            cameraIsMove = false;
        }
    }

    public void CheckBoundary()
    {
        if (playerTransform == null)
        {
            return;
        }
        float leftDistance = 0; //玩家和主相机的水平距离
        if (playerTransform.position.x < transform.position.x) //在左边
        {
            leftDistance = transform.position.x - playerTransform.position.x;

        }
        else
        {
            leftDistance = playerTransform.position.x - transform.position.x;
        }
        if (leftDistance > camMoveBox.x * 0.5f)//玩家和主相机的水平距离大于水平视野范围的一半
        {
            Debug.Log("超出视野范围X轴");
            cameraIsMove = true;
        }

        float uDDistance = 0;   //玩家和主相机的垂直距离

        if (playerTransform.position.y < transform.position.y)
        {
            uDDistance = transform.position.y - playerTransform.position.y;
        }
        else
        {
            uDDistance = playerTransform.position.y - transform.position.y;
        }

        if (uDDistance > camMoveBox.y * 0.5f)//玩家和主相机的垂直距离大于垂直视野范围的一半
        {
            Debug.Log("超出视野范围Y轴");
            cameraIsMove = true;
        }
    }
}
