using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S01_Console : MonoBehaviour
{
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;
    [Header("cameraFollow组件")]
    public GameObject CameraFollowObj;


    private bool isAroundConsole;//玩家是否接触到控制台
    private bool hasOpenDialog;
    private int dialogTextNum;
    private bool scene1IsDone;//scene1任务完成

    // Start is called before the first frame update
    void Start()
    {
        dialogTextNum = 0;
        hasOpenDialog = false;
        scene1IsDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAroundConsole && scene1IsDone)
        {
            //将文字赋给对话框文字组件
            dialogBoxText.text = "下一关\n--Q键跳过--".Replace("\\n", "\n");
            DialogBox.SetActive(true);//当接触到控制台并且按下E的时候，显示对话框
            hasOpenDialog = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && isAroundConsole && !CameraFollow.cameraRotate && !scene1IsDone)
        {
            switch (dialogTextNum)
            {
                case 0:
                    dialogBoxText.text = "飞船遭到不明外星生物的入侵，你需要到飞船右上角的能源仓重启飞船的动力系统！".Replace("\\n", "\n");
                    break;
                case 1:
                    dialogBoxText.text = "对话2".Replace("\\n", "\n");
                    break;
                case 2:
                    dialogBoxText.text = "对话3".Replace("\\n", "\n");
                    break;
            }
            dialogTextNum = ++dialogTextNum % 3;
            DialogBox.SetActive(true);//当接触到控制台并且按下E的时候，显示对话框
            hasOpenDialog = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && isAroundConsole && CameraFollow.cameraRotate )
        {
            //将文字赋给对话框文字组件
            dialogBoxText.text = "正在调整飞船平衡，重力已恢复正常！\n--Q键跳过--".Replace("\\n", "\n");
            DialogBox.SetActive(true);//当接触到控制台并且按下E的时候，显示对话框
            hasOpenDialog = true;
            CameraFollow.cameraRotate = false;//停止相机旋转
            CameraFollowObj.GetComponent<CameraFollow>().StopCameraRotation();//恢复相机初始位置
            scene1IsDone = true;
        }        
        if (Input.GetKeyDown(KeyCode.Q) && hasOpenDialog)
        {
            DialogBox.SetActive(false);
        }
    }

    //当检测到碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundConsole = true;//接触到控制台
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundConsole = false;
            DialogBox.SetActive(false);//离开控制台，隐藏对话框
        }
    }

}
