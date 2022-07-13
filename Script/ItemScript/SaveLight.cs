using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLight : MonoBehaviour
{
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;

    private bool isAroundSaveLight;//玩家是否接触到存档灯
    private bool hasOpenDialog;

    // Start is called before the first frame update
    void Start()
    {
        hasOpenDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAroundSaveLight)
        {
            dialogBoxText.text = "保存成功！".Replace("\\n", "\n");
            DialogBox.SetActive(true);//当接触到存档灯并且按下E的时候，显示对话框
            hasOpenDialog = true;
            PlayerHealth.revivePos = transform.position;//更新重生点
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
            isAroundSaveLight = true;//接触到存档灯
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundSaveLight = false;
            DialogBox.SetActive(false);//离开存档灯，隐藏对话框
        }
    }

}
