using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1LeversCtrl : MonoBehaviour
{
    [Header("挡杆物体列表（从右上角开始）")]
    public List<GameObject> leverObjList;
    [Header("相机上的主音源")]
    public AudioSource mainAudioSource;
    [Header("TeleporterBar进度条mask")]
    public GameObject teleporterBar;
    

    private List<LeverCtrl> leverCtrlList;  //挡杆控制器列表
    private List<bool> lastStatusList;      //上一次各挡杆开启状态列表
    private List<int> leverIndex;           //状态发生变化的挡杆索引列表
    private BoxCollider2D mainLeverCollider;  //主lever的碰撞体
    private bool hasPlaySuccessAudio;       //已经播放解谜成功音效 
    private bool hasPlayWarningAudio;       //已经开始播放警报音效

    // Start is called before the first frame update
    void Start()
    {        
        //根据挡杆物体列表初始化挡杆控制器列表
        leverCtrlList = new List<LeverCtrl>();
        for(int i = 0; i < leverObjList.Count; i++)
        {
            leverCtrlList.Add(leverObjList[i].GetComponent<LeverCtrl>());//获取每一个挡杆的控制器
        }
        
        //根据挡杆控制器列表初始化上一次各挡杆开启状态列表
        lastStatusList = new List<bool>();
        for (int i = 0; i < leverObjList.Count; i++)
        {
            lastStatusList.Add(leverCtrlList[i].leverIsOn);//记录每一个挡杆的初始开启状态
        }

        //初始化状态发生变化的挡杆索引列表
        leverIndex = new List<int>();

        //获取主lever的碰撞体，并初始化为不可用
        mainLeverCollider = leverObjList[leverObjList.Count-1].GetComponent<BoxCollider2D>();
        mainLeverCollider.enabled = false;

        TeleporterBar.powerMax = 8;//能量条最大值设为挡杆的个数8
        TeleporterBar.powerNow = 4;//能量条当前值初始化为4

        hasPlaySuccessAudio = false;
        hasPlayWarningAudio = false;
    }

    // Update is called once per frame
    void Update()
    {
        //找出0-7中开启状态发生变化的挡杆,并且记录其索引
        for (int i = 0; i < leverObjList.Count - 1; i++)
        {
            if(leverCtrlList[i].leverIsOn != lastStatusList[i])
            {
                leverIndex.Add(i);
            }
        }
        //如果有一对挡杆状态发生变化
        if(leverIndex.Count == 2)
        {            
            //并且变化之前的开启状态相同
            if (lastStatusList[leverIndex[0]] == lastStatusList[leverIndex[1]])
            {
                //则回滚变化
                leverCtrlList[leverIndex[0]].leverIsOn = lastStatusList[leverIndex[0]];
                leverCtrlList[leverIndex[1]].leverIsOn = lastStatusList[leverIndex[1]];                
            }
            leverIndex.Clear();//清空状态发生变化的挡杆索引列表
        }
        //记录本次挡杆的开启状态列表
        for (int i = 0; i < leverCtrlList.Count - 1; i++)
        {
            lastStatusList[i] = leverCtrlList[i].leverIsOn;
        }


        //如果0-3（所有上层挡杆）关闭，4-7（所有下层挡杆）打开，可以控制主挡杆（开启失重）      
        //即powerNow为0，能量条显示为满时，可以控制主挡杆（开启失重）
        TeleporterBar.powerNow = 0;//能量条初始化为满格状态
        for (int i = 0; i <= 3; i++)
        {
            if (leverCtrlList[i].leverIsOn)
            {
                TeleporterBar.powerNow++;//0-3有打开（水平）的，则powerNow加1
            }
        }
        for (int i = 4; i <= 7; i++)
        {
            if (!leverCtrlList[i].leverIsOn)
            {
                TeleporterBar.powerNow++;//4-7有关闭（竖直）的，则powerNow加1
            }
        }
        //如果检测完后，powerNow仍然为0，则此时完成谜题，能量条为满格，可以打开主挡杆
        if(TeleporterBar.powerNow == 0)
        {
            mainLeverCollider.enabled = true;//主挡杆的collider生效
            if (!hasPlaySuccessAudio)//如果还没有播放成功音效则播放
            {
                AudioManger.Instance.PlayAudio("解谜成功音效2", GameObject.FindGameObjectWithTag("playerV").transform.position);//播放解谜成功音效
                hasPlaySuccessAudio = true;//已经播放成功音效
            }            
        }
        else
        {
            mainLeverCollider.enabled = false;
        }

        //当开启主挡杆时，开启失重状态，同时播放警报音效
        if (leverCtrlList[leverObjList.Count-1].leverIsOn)
        {            
            if (!hasPlayWarningAudio)
            {
                CameraFollow.cameraRotate = true;

                mainAudioSource.clip = Resources.Load<AudioClip>("musicFile/警报声1");
                mainAudioSource.Play();
                teleporterBar.SetActive(false);//隐藏teleporterBar上的进度条，以免镜头旋转时露馅
                hasPlayWarningAudio = true;
            }            
        }        
    }
    
}
