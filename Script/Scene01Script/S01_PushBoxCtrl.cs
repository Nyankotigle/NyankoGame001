using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S01_PushBoxCtrl : MonoBehaviour
{
    [Header("door01_0")]
    public GameObject door01_0;
    [Header("door01_1")]
    public GameObject door01_1;
    [Header("玩家推箱子时的移动速度")]
    public float pushBoxSpeed;

    public Transform pushBox1;
    public Transform pushBox2;
    public Transform pushBox3;
    public Transform pushBox4;
    public Transform pushBoxTarget1;
    public Transform pushBoxTarget2;
    public Transform pushBoxTarget3;
    public Transform pushBoxTarget4;

    private Vector2 pushBox1Pos;
    private Vector2 pushBox2Pos;
    private Vector2 pushBox3Pos;
    private Vector2 pushBox4Pos;

    private Door01Ctrl door01_0Ctrl;
    private Door01Ctrl door01_1Ctrl;
    private PlayerController playerCtrl;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    private bool hasResetPlayerCtrl;//已经恢复正常的玩家控制
    private bool hasOpenDoor01_1;   //已经打开door01_1

    // Start is called before the first frame update
    void Start()
    {
        //记录箱子初始位置
        pushBox1Pos = pushBox1.position;
        pushBox2Pos = pushBox2.position;
        pushBox3Pos = pushBox3.position;
        pushBox4Pos = pushBox4.position;

        door01_0Ctrl = door01_0.GetComponent<Door01Ctrl>();
        door01_1Ctrl = door01_1.GetComponent<Door01Ctrl>();
        playerCtrl = GetComponent<PlayerController>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        door01_0Ctrl.canEnter = true;//door01_0可以打开
        door01_1Ctrl.canEnter = false;//door01_1不能打开

        hasResetPlayerCtrl = false;
        hasOpenDoor01_1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //按下Q键时，所有箱子回到初始位置
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pushBox1.position = pushBox1Pos;
            pushBox2.position = pushBox2Pos;
            pushBox3.position = pushBox3Pos;
            pushBox4.position = pushBox4Pos;
        }

        //当四个箱子都在指定的位置并且没有设置door01_1可以打开和播放音效的时候
        if ((pushBox1.position == pushBoxTarget1.position || pushBox1.position == pushBoxTarget2.position || 
             pushBox1.position == pushBoxTarget3.position || pushBox1.position == pushBoxTarget4.position) &&
            (pushBox2.position == pushBoxTarget1.position || pushBox2.position == pushBoxTarget2.position ||
             pushBox2.position == pushBoxTarget3.position || pushBox2.position == pushBoxTarget4.position) &&
            (pushBox3.position == pushBoxTarget1.position || pushBox3.position == pushBoxTarget2.position ||
             pushBox3.position == pushBoxTarget3.position || pushBox3.position == pushBoxTarget4.position) &&
            (pushBox4.position == pushBoxTarget1.position || pushBox4.position == pushBoxTarget2.position ||
             pushBox4.position == pushBoxTarget3.position || pushBox4.position == pushBoxTarget4.position) &&
             !hasOpenDoor01_1)
        {
            door01_1Ctrl.canEnter = true;//door01_1可以打开
            AudioManger.Instance.PlayAudio("解谜成功音效2", transform.position);//播放解谜成功音效
            hasOpenDoor01_1 = true;//已经打开door01_1，避免重复播放音效
        }

        //玩家进入door01_0并且没有走出door01_1
        if (door01_0Ctrl.playerCrossDoor && !door01_1Ctrl.playerCrossDoor)
        {
            playerCtrl.canInput = false;                //停止playerController中的控制函数
            playerRigidbody.gravityScale = 0;           //玩家重力设为0
            playerRigidbody.velocity = new Vector2(0,0);//玩家速度设为0

            PushBoxMove();
        }

        //玩家走出door01_1并且没有恢复其正常控制时
        if (door01_1Ctrl.playerCrossDoor && !hasResetPlayerCtrl)
        {
            playerCtrl.canInput = true;
            playerRigidbody.gravityScale = playerCtrl.playerGravityScale;
            hasResetPlayerCtrl = true;
        }
        
    }

    /// <summary>
    /// 推箱子移动
    /// </summary>
    void PushBoxMove()
    {
        Vector2 playerVel;
        //水平移动
        float moveDirH = Input.GetAxis("Horizontal");
        if (moveDirH != 0)
        {
            playerVel = new Vector2(moveDirH * pushBoxSpeed, 0);
            playerRigidbody.velocity = playerVel;
        }
        //垂直移动
        float moveDirV = Input.GetAxis("Vertical");
        if (moveDirV != 0)
        {
            playerVel = new Vector2(0, moveDirV * pushBoxSpeed);
            playerRigidbody.velocity = playerVel;
        }

        bool playerHasXAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("Run", playerHasXAxisSpeed);
        
    }

}
