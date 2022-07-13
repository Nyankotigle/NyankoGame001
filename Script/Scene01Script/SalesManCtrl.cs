using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalesManCtrl : MonoBehaviour
{
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;
    [Header("商人的日本刀")]
    public GameObject japSword;

    private Animator salesManAnim;
    private bool isAroundSalesMan;  //玩家是否在商人附近
    private int dialogNum;          //对话的序号
    private bool hasSoldJapSword;   //已经卖出日本刀

    // Start is called before the first frame update
    void Start()
    {
        salesManAnim = GetComponent<Animator>();
        isAroundSalesMan = false;
        hasSoldJapSword = false;
        dialogNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAroundSalesMan && !hasSoldJapSword)
        {
            switch (dialogNum)
            {
                case 0:
                    dialogBoxText.text = "你好，盗梦者".Replace("\\n", "\n");
                    break;
                case 1:
                    dialogBoxText.text = "我为什么会在这儿？这不重要。".Replace("\\n", "\n");
                    break;
                case 2:
                    dialogBoxText.text = "这把刀是我好不容易得到的，你要的话我可以卖给你！只要40个金币！\n按W键购买".Replace("\\n", "\n");
                    break;
            }
            dialogNum = ++dialogNum % 3;
            DialogBox.SetActive(true);//当在商人附近并且按下E的时候，显示对话框
        }
        if (Input.GetKeyDown(KeyCode.W) && isAroundSalesMan && !hasSoldJapSword)
        {
            if(CoinNumCtrl.coinNumNow < 50)
            {
                dialogBoxText.text = "钱币不够！".Replace("\\n", "\n");
            }
            else
            {
                CoinNumCtrl.coinNumNow -= 50;
                hasSoldJapSword = true;
                japSword.SetActive(false);
                PlayerController.HasJapSword = true;
                StarNumCtrl.starNumNow += 1;
                AudioManger.Instance.PlayAudio("捡星星音效", transform.position);//播放捡星星音效
                GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>().health = 100;//恢复玩家血量
                HealthBar.healthNow = 100;//更新血条
                dialogBoxText.text = "好吧！他现在属于你了！\n短按鼠标右键可以隔挡，长按右键可以蓄力 \n蓄力时点击左键发动小蓄力招式，蓄力到极限后释放右键发动大蓄力招式\n另外，你真是一个不服输的家伙，我帮你治好了你身上的伤，我只能帮你到这了！".Replace("\\n", "\n");
            }
            DialogBox.SetActive(true);//当在商人附近并且按下E的时候，显示对话框
        }
        if (Input.GetKeyDown(KeyCode.E) && isAroundSalesMan && hasSoldJapSword)
        {
            dialogBoxText.text = "好好练习使用这把刀吧！前面还有更多挑战等着你呢！祝你好运！".Replace("\\n", "\n");
        }
    }

    //当检测到碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundSalesMan = true;//在商人附近
            if(GameObject.FindGameObjectWithTag("playerV").transform.position.x < transform.position.x)
            {
                salesManAnim.SetBool("FaceLeft", true);
            }
            else
            {
                salesManAnim.SetBool("FaceRight", true);
            }
            
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundSalesMan = false;
            DialogBox.SetActive(false);//离开商人，隐藏对话框
            salesManAnim.SetBool("FaceRight", false);
            salesManAnim.SetBool("FaceLeft", false);
        }
    }
}
