using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;
    [Header("在这里输入对话框中要显示的文字")]
    public string dialogText;

    private bool isAroundBillboard;//玩家是否接触到告示牌
    private bool hasOpenDialog;

    // Start is called before the first frame update
    void Start()
    {
        //Inspector编辑框会自动把\n转换为\\n，这里再转换回去，就可以通过\n输入换行符
        dialogText = dialogText.Replace("\\n", "\n");
        hasOpenDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAroundBillboard)
        {
            dialogBoxText.text = dialogText;//将文字赋给对话框文字组件
            DialogBox.SetActive(true);//当接触到告示牌并且按下E的时候，显示对话框
            hasOpenDialog = true;
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
            isAroundBillboard = true;//接触到告示牌
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundBillboard = false;
            DialogBox.SetActive(false);//离开告示牌，隐藏对话框
        }
    }

}
