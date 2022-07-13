using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S01_treasureBox_UniSprint : MonoBehaviour
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
            //******宝箱4打开时，获得万向冲刺技能******
            PlayerController.isUniversalSprint = true;//可以使用万向冲刺技能

            hasOpenDialog = true;
            //将文字赋给对话框文字组件
            dialogBoxText.text = "获得万向冲刺技能！\n按left Shift发动，冲刺方向为鼠标所指的方向\n--Q键跳过--".Replace("\\n", "\n");
            DialogBox.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E) && hasOpenDialog)
        {
            DialogBox.SetActive(false);
        }
    }
    
}
