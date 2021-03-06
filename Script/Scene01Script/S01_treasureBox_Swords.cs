using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S01_treasureBox_Swords : MonoBehaviour
{
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;

    private bool hasOpenDialog;
    private TreasureBox treasureBoxCtrl;

    // Start is called before the first frame update
    void Start()
    {
        treasureBoxCtrl = GetComponent<TreasureBox>();
        hasOpenDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (treasureBoxCtrl.isOpened && !hasOpenDialog)
        {
            //******宝箱0打开时，获得短剑******
            PlayerController.HasSword = true;

            hasOpenDialog = true;  
            //将文字赋给对话框文字组件
            dialogBoxText.text = "获得短剑！\n按鼠标左键发动攻击，鼠标的位置可以控制攻击的方向\n--Q键跳过--".Replace("\\n", "\n");
            DialogBox.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q) && hasOpenDialog)
        {
            DialogBox.SetActive(false);
        }
    }
    
}
