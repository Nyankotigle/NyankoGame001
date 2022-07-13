using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("跑步速度")]
    public float playerRunSpeed;
    [Header("起跳速度")]
    public float playerJumpSpeed;
    [Header("冲刺键")]
    public KeyCode SprintKey;
    [Header("冲刺速度")]
    public float playerSprintSpeed;
    [Header("冲刺持续时间")]
    public float sprintTime;
    [Header("冲刺冷却时间")]
    public float sprintWaitTime;
    [Header("冲刺特效")]
    public GameObject SprintEffect;
    [Header("攻击键")]
    public KeyCode AttackKey;    
    [Header("背后的剑")]
    public GameObject Swards;
    [Header("普通攻击剑光效")]
    public GameObject normalAttackSwardLight;    
    [Header("剑攻击范围物体")]
    public GameObject swardAttackRange;
    [Header("玩家重力大小")]
    public int playerGravityScale;
    [Header("左轮")]
    public GameObject Revolver;
    [Header("相机")]
    public Camera mainCamera;
    [Header("子弹发射点")]
    public GameObject bulletStartPos;
    [Header("捶地键")]
    public KeyCode BeatGroundKey;
    [Header("捶地速度")]
    public float beatGroundSpeed;
    [Header("捶地特效")]
    public GameObject BeatGroundEffect;
    [Header("捶地攻击范围物体")]
    public GameObject beatGroundRange;
    [Header("蓄力攻击范围物体")]
    public GameObject chargeCutRange;
    [Header("min蓄力攻击范围物体")]
    public GameObject minChargeCutRange;
    [Header("格挡范围物体")]
    public GameObject parryRange;
    [Header("格挡粒子特效")]
    public GameObject parryEffect;
    [Header("蓄力粒子特效")]
    public GameObject chargeEffect;
    [Header("回旋剑")]
    public GameObject throwSword;
    [Header("扔回旋剑的位置")]
    public Transform throwSwordPos;
    [Header("移动轨迹灰尘特效")]
    public GameObject dustTrackEffect;
    [Header("chargeCut灰尘特效位置")]
    public Transform chargeCutDustPos;

    [Header("玩家蓝值")]
    public int playerBlueNum;
    [Header("蓝条恢复1个单位所用的时间")]
    public float blueRestoreTime;
    [Header("冲刺消耗蓝值")]
    public int sprintCostBlue;
    [Header("格挡消耗蓝值")]
    public int parryCostBlue;
    [Header("min蓄力消耗蓝值")]
    public int minChargeCutCostBlue;
    [Header("蓄力消耗蓝值")]
    public int chargeCutCostBlue;
    [Header("捶地消耗蓝值")]
    public int beatGroundCostBlue;


    GameObject jumpTwoPlumeEffect;  //二段跳羽毛特效预制体
    GameObject beatGroundDustEffect;//溅起的灰尘特效预制体
    GameObject bulletPrefab;        //子弹预制体

    public bool canInput;
    public bool canRun;
    public bool canJump;
    public bool canSprint;

    /// <summary>
    /// 玩家是否拥有技能控制
    /// </summary>
    public static bool HasClimb;
    public static bool HasSprint;
    public static bool HasSecondJump;
    public static bool HasSword;
    public static bool HasJapSword;
    public static bool HasBeatGround;
    public static bool HasFlashLight;

    public static bool playerFaceRight; //玩家方向向右
    public static bool useSword;        //用剑
    public static bool useRevolver;     //用左轮
    public static bool isUniversalSprint;//是否是万向冲刺

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerFeet;//地面检测
    private PolygonCollider2D swordCollider;        //剑攻击时的多边形碰撞体
    private PolygonCollider2D beatGroundCollider;   //捶地攻击时的多边形碰撞体
    private PolygonCollider2D chargeCutCollider;    //蓄力攻击时的多边形碰撞体
    private PolygonCollider2D minChargeCutCollider; //min蓄力攻击时的多边形碰撞体
    private BoxCollider2D parryCollider;            //格挡范围碰撞体
    private Animator playerAnimator;
    private Animator sprintEffectAnimator;//冲刺特效动画
    private Animator revolverAnimator;//左轮动画
    private Animator normalAttackAnim;//普通攻击剑光效动画
    private AudioSource chargeAudioSource;//蓄力音效
    private float chargeTime;//蓄力时间
    private float pressAttackButtonTime;//记录按下普通攻击键的时间
    private float jumpBtnDownTime;//跳跃键按下的时间

    private Vector3 mousePos;//鼠标位置
    private float revolverAngle;//左轮的角度，0度水平正方向，90度垂直向上
    private float sprintAngle;//万向冲刺的角度
    private Vector2 sprintEffectOriginVector;//冲刺特效相对玩家坐标初始位置

    private bool isOnGround;//玩家是否在地面
    private bool wasOnGround;//玩家上一帧是否在地面
    private bool isJumpButtonDown;//跳跃键处于按下状态
    private bool isFirstJump;//是否是一段跳
    private bool CanSecondJump = true;//可以二段跳
    private bool isSecondJump = false;//是否是二段跳
    private bool isClimb;//玩家是否在爬墙
    private bool isClimbJump;//是否是爬墙跳
    private bool isBeatGround;//是否在捶地
    private bool isCharge;//是否在蓄力
    private bool isNormalAttack;//是否在普通攻击
    private bool hasResetGravity;//失重状态后是否恢复重力
    private bool canRestoreBlue;//可以恢复蓝条
    private bool hasPressAttackButton;//已经按下了普通攻击键
    private bool isShowDustTrack;//显示移动时的灰尘轨迹

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = playerGravityScale;//初始化重力大小
        playerAnimator = GetComponent<Animator>();
        sprintEffectAnimator = SprintEffect.GetComponent<Animator>();//获取冲刺特效动画组件
        normalAttackAnim = normalAttackSwardLight.GetComponent<Animator>();//获取普通攻击剑光效动画组件
        playerFeet = GetComponent<BoxCollider2D>();
        jumpTwoPlumeEffect = Resources.Load<GameObject>("Prefebs/plumeEffectOne");  //加载二段跳羽毛粒子特效预制体
        bulletPrefab = Resources.Load<GameObject>("Prefebs/Bullet");                //加载子弹预制体
        beatGroundDustEffect = Resources.Load<GameObject>("Prefebs/BeatGroundDustEffect");//加载灰尘特效预制体
        swordCollider = swardAttackRange.GetComponent<PolygonCollider2D>();         //获取剑攻击时的多边形碰撞体
        beatGroundCollider = beatGroundRange.GetComponent<PolygonCollider2D>();     //获取捶地攻击时的多边形碰撞体
        chargeCutCollider = chargeCutRange.GetComponent<PolygonCollider2D>();       //获取蓄力攻击时的多边形碰撞体
        minChargeCutCollider = minChargeCutRange.GetComponent<PolygonCollider2D>(); //获取min蓄力攻击时的多边形碰撞体
        parryCollider = parryRange.GetComponent<BoxCollider2D>();                   //获取格挡范围碰撞体
        revolverAnimator = Revolver.GetComponent<Animator>();//获取左轮的动画组件    

        BlueBar.blueMax = playerBlueNum;//初始化蓝条最大值
        BlueBar.blueNow = playerBlueNum;//初始化蓝条当前值

        useSword = true;//是否用剑
        useRevolver = false;//是否用左轮
        normalAttackSwardLight.SetActive(false);//隐藏剑光效
        BeatGroundEffect.SetActive(false);//隐藏捶地特效
        isBeatGround = false;//不在捶地
        isUniversalSprint = false;//使用万向冲刺
        sprintEffectOriginVector = SprintEffect.transform.position - transform.position;//记录冲刺特效相对玩家位置的初始位置向量

        hasResetGravity = true;//已经恢复重力
        canRestoreBlue = true;
        hasPressAttackButton = false;
        canRun = true;//可以左右移动
        isShowDustTrack = false; 

        //////******初始拥有技能控制******//////
        HasSprint = false;
        HasSecondJump = false;
        HasSword = false;
        HasJapSword = false;
        HasBeatGround = false;
        HasFlashLight = false;
        HasClimb = false;
    }

    // Update is called once per frame
    void Update()
    {        
        //Flip();
        Run();
        Jump();
        CheckIsOnGround();
        Attack();
        Sprint();
        Climb();
        SpinRevolver();
        BeatGround();
        ChargeCut();
        playerGravity();
        RestoreBlue();
        ShowDustTrack();
    }

    /*
    /// <summary>
    /// player左右翻转
    /// </summary>
    void Flip()
    {
        //是否有水平速度
        bool playerHasXAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if(playerRigidbody.velocity.x > 0.1f)//有向右的水平速度
            {
                playerFaceRight = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                Swards.transform.localPosition = new Vector3(0, 0, 1);                  
            }
            if (playerRigidbody.velocity.x < -0.1f)//有向左的水平速度
            {
                playerFaceRight = false;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                Swards.transform.localPosition = new Vector3(0, 0, -1);//移动剑的垂直位置，使其始终在玩家身后                 
            }
        }
    }
    */

    /// <summary>
    /// 跑步和相关动画切换
    /// </summary>
    void Run()
    {
        if (!canInput || !canRun)
        {
            return;
        }
        //水平移动
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * playerRunSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVel;

        //////*****替代Flip()*****//////
        if (moveDir > 0)
        {
            playerFaceRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            Swards.transform.localPosition = new Vector3(0, 0, 1);
        }
        if (moveDir < 0)
        {
            playerFaceRight = false;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            Swards.transform.localPosition = new Vector3(0, 0, -1);//移动剑的垂直位置，使其始终在玩家身后 
        }


        //是否有水平速度
        bool playerHasXAxisSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        if (isOnGround)//只在地面播放跑步动画，在空中可以左右移动，但不播放跑步动画
        {            
            playerAnimator.SetBool("Run", playerHasXAxisSpeed);
        }
        else
        {
            playerAnimator.SetBool("Run", false);
        }
        PlayRunAudio(playerHasXAxisSpeed);//播放脚步音效

    }


    /// <summary>
    /// 跳跃和相关动画切换
    /// </summary>
    void Jump()
    {
        if (!canInput)
        {
            return;
        }
      
        if (Input.GetButtonDown("Jump"))//按下跳跃键
        {                      
            if (isOnGround)//如果从地面起跳开始一段跳
            {
                isClimbJump = false;//一旦落地，取消爬墙跳状态，不能无限进行二段跳
                isShowDustTrack = true;//显示灰尘轨迹
                Invoke("StopShowJumpDustEffect", 0.1f);//停止显示灰尘轨迹
               
                Vector2 jumpVel = new Vector2(0.0f, playerJumpSpeed);
                playerRigidbody.velocity = Vector2.up * jumpVel;

                isFirstJump = true;
                AudioManger.Instance.PlayAudio("hero_jump", transform.position);//播放一段跳音效
            }
            else
            {
                //如果是爬墙跳状态
                if (isClimbJump)
                {
                    isFirstJump = true;//isFirstJump永远设为真，可以在空中连续二段跳爬墙
                    isClimbJump = false;//但每次进入爬墙状态只能解锁一次，需要下次离墙后再触发
                }
                
                //否则，如果从一段跳起跳并且可以二段跳则开始二段跳
                if (isFirstJump && CanSecondJump && HasSecondJump)
                {
                    isSecondJump = true;//标记二段跳状态用于触发捶地技能
                    Vector2 jumpVel = new Vector2(0.0f, playerJumpSpeed);
                    playerRigidbody.velocity = Vector2.up * jumpVel;
                    isFirstJump = false;
                    //在触发二段跳的位置实例化羽毛粒子特效物体
                    var plumeEffectObj = Instantiate(jumpTwoPlumeEffect, transform.position, Quaternion.Euler(180, 0, 0));
                    plumeEffectObj.transform.position += new Vector3(0, 0.8f, 0);//为了效果，调高特效生成位置
                    AudioManger.Instance.PlayAudio("jump_out_of_snow", transform.position);//播放二段跳音效
                }
            }
        }     
        bool playerHasUpSpeed = playerRigidbody.velocity.y > 0.1f;
        playerAnimator.SetBool("JumpUp", playerHasUpSpeed);//有向上的速度时，播放JumpUp动画
        bool playerHasDownSpeed = playerRigidbody.velocity.y < -0.1f;
        playerAnimator.SetBool("JumpDown", playerHasDownSpeed);//有向下的速度时，播放JumpDown动画
    }
    void StopShowJumpDustEffect()
    {
        isShowDustTrack = false;//停止显示灰尘轨迹
    }
    /*
    void JumpLevel()
    {
        if (isJumpButtonDown)
        {
            jumpBtnDownTime += Time.deltaTime;

            if (Input.GetButtonUp("Jump") || jumpBtnDownTime > 1.0f)//弹起跳跃键
            {
                isJumpButtonDown = false;

                isClimbJump = false;//一旦落地，取消爬墙跳状态，不能无限进行二段跳
                isShowDustTrack = true;//显示灰尘轨迹
                Invoke("StopShowJumpDustEffect", 0.1f);//停止显示灰尘轨迹

                if (jumpBtnDownTime > 0.5f)
                {
                    jumpBtnDownTime = 0.5f;
                }
                Vector2 jumpVel = new Vector2(0.0f, playerJumpSpeed * (0.8f + jumpBtnDownTime / 2.0f));
                playerRigidbody.velocity = Vector2.up * jumpVel;
                jumpBtnDownTime = 0;

                isFirstJump = true;
                AudioManger.Instance.PlayAudio("hero_jump", transform.position);//播放一段跳音效
            }
        }        
    }
    */

    /// <summary>
    /// 检查是否在地面以及捶地技能
    /// </summary>
    void CheckIsOnGround()
    {        
        wasOnGround = isOnGround;//更新上一帧是否在地面的bool
        //通过脚部的碰撞体是否接触到Ground层来判断是否在地面
        isOnGround = playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        //Debug.Log(isOnGround);   
        if (isOnGround && !wasOnGround)//上一帧不在地面这一帧在地面时，说明正好落地
        {
            //检测到捶地下降状态、有捶地技能
            if (isBeatGround && HasBeatGround)
            {
                isBeatGround = false;
                isShowDustTrack = false;//停止显示灰尘轨迹

                //在捶地的位置实例化灰尘粒子特效物体
                var dustEffectObj = Instantiate(beatGroundDustEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                dustEffectObj.transform.position -= new Vector3(0, 1.3f, 0);//调整特效生成位置

                BeatGroundEffect.SetActive(true);//显示捶地特效
                beatGroundCollider.enabled = true;//启用捶地攻击范围碰撞体
                CameraShake.camShake.Shake();//调用相机震动
                Invoke("HideBeatGroundEffect", 0.5f);//0.5s后隐藏捶地特效和停用捶地攻击范围碰撞体
                AudioManger.Instance.PlayAudio("BeatGround_0", transform.position);//播放捶地音效
            }
            else
            {
                AudioManger.Instance.PlayAudio("hero_land_soft", transform.position);//播放落地音效
            }            
            isSecondJump = false;//结束二段跳状态
        }
    }
    void HideBeatGroundEffect()
    {
        BeatGroundEffect.SetActive(false);
        beatGroundCollider.enabled = false;//停用捶地攻击范围碰撞体
    }

    /// <summary>
    /// 普通攻击和左轮
    /// </summary>
    void Attack()
    {        
        Revolver.SetActive(useRevolver);//用左轮时才显示左轮

        if (HasSword)
        {
            Swards.SetActive(true);//获取剑时才显示背后的剑
        }
        else
        {
            Swards.SetActive(false);//隐藏背后的剑
        }
        
        if (!canInput || isCharge || !HasSword || isNormalAttack)//蓄力或者正在普通攻击时时不能普通攻击
        {
            return;
        }

        if (useSword)//用剑
        {
            if (Input.GetKeyDown(AttackKey) || Input.GetMouseButtonDown(0))//按下攻击键或鼠标左键
            {
                
                hasPressAttackButton = true;//标识按键按下，开始计时
                pressAttackButtonTime = 0f;
            }
            if (hasPressAttackButton)
            {
                pressAttackButtonTime += Time.deltaTime;
                //若0.2s后还未抬起左键，则视为发动回旋剑
                if (pressAttackButtonTime > 0.2f)
                {
                    hasPressAttackButton = false;//停止计时
                    pressAttackButtonTime = 0f;
                    Instantiate(throwSword, throwSwordPos.position, transform.rotation);//在预设的位置实例化回旋剑
                    AudioManger.Instance.PlayAudio("ThrowSwordSound_3", transform.position);//播放发动回旋剑音效  
                }
            }
             
            if ((Input.GetKeyUp(AttackKey) || Input.GetMouseButtonUp(0)) && hasPressAttackButton )//弹起攻击键或鼠标左键
            {
                hasPressAttackButton = false;//停止计时
                isNormalAttack = true;//开始普通攻击，普通攻击还未结束时不能进入蓄力

                Swards.SetActive(false);//攻击动画开始，隐藏背后的剑
                normalAttackSwardLight.SetActive(true); //显示剑光效              
                swordCollider.enabled = true;//剑攻击时多边形碰撞体生效
                playerAnimator.SetTrigger("Attack");
                normalAttackAnim.SetTrigger("normalAttack");
                AudioManger.Instance.PlayAudio("sword_4", transform.position);//播放普通攻击音效  

                Invoke("NormalAttackEnd", 0.1f);
            }

        }
        else if (useRevolver)
        {
            if (Input.GetKeyDown(AttackKey) || Input.GetMouseButtonDown(0))//按下攻击键或鼠标左键
            {
                revolverAnimator.SetTrigger("Fire");//播放射击动画
                AudioManger.Instance.PlayAudio("RevolverFire", transform.position);//播放射击音效
                //实例化子弹
                Instantiate(bulletPrefab, bulletStartPos.transform.position, bulletStartPos.transform.rotation);
            }
        }
    }
    /// <summary>
    /// 普通攻击结束动画帧事件
    /// </summary>
    void NormalAttackEnd()
    {
        swordCollider.enabled = false;//攻击结束剑多边形碰撞体无效
        Swards.SetActive(true);//攻击动画结束，显示背后的剑
        normalAttackSwardLight.SetActive(false);//隐藏剑光效
        isNormalAttack = false;//结束普通攻击
    }
    /// <summary>
    /// 根据鼠标位置旋转左轮
    /// </summary>
    void SpinRevolver()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);//将鼠标位置转换为世界坐标
        Vector3 revolverDir = (mousePos - Revolver.transform.position).normalized;//求左轮到鼠标方向的单位向量
        revolverAngle = Mathf.Atan2(revolverDir.y, Mathf.Abs(revolverDir.x)) * Mathf.Rad2Deg;//转换为与X轴(不分正负)的夹角，范围[-90°,90°]
        Revolver.transform.localRotation = Quaternion.Euler(0, 0, revolverAngle);//这样左轮不会指向背后
    }



    AudioSource runAudioSource;//脚步声
    public bool isPlayRunAudio;//正在播放脚步声
    /// <summary>
    /// 初始化玩家脚步声音播放物体
    /// </summary>
    public void InitRunAudioObj()
    {
        runAudioSource = AudioManger.Instance.PlayAudio("hero_run_footsteps_stone", transform, 2);
        runAudioSource.Stop();
    }    
    /// <summary>
    /// 播放脚步声音
    /// </summary>
    public void PlayRunAudio(bool hasXSpeed)
    {
        if (runAudioSource == null)
        {
            InitRunAudioObj();// 初始化玩家脚步声音播放物体
        }
        if (hasXSpeed && isOnGround && !isPlayRunAudio)
        {
            isPlayRunAudio = true;
            runAudioSource.Play();
        }
        else if ((!hasXSpeed) || (!isOnGround))
        {
            runAudioSource.Stop();//没有水平速度或者没有在地面，都停止播放脚步声
            isPlayRunAudio = false;
        }
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    void Sprint()
    {
        if (!canInput || isCharge || !HasSprint)
        {
            return;
        }
        if (Input.GetKeyDown(SprintKey) && canSprint && (BlueBar.blueNow >= sprintCostBlue))//蓝值够
        {
            BlueBar.blueNow -= sprintCostBlue;//消耗相应的蓝值

            if (!isUniversalSprint)//普通冲刺
            {
                sprintEffectAnimator.SetTrigger("SprintEffect");//播放冲刺特效动画
                playerAnimator.SetBool("Sprint", true);//播放冲刺动作
                AudioManger.Instance.PlayAudio("hero_shade_dash_1", transform.position);//播放冲刺音效
                StartCoroutine(SprintMove(sprintTime));//启动冲刺移动协程SprintMove            
                StartCoroutine(SprintWait(sprintWaitTime));//启动冲刺冷却协程SprintWait
            }            
            else//万向冲刺
            {
                mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);//将鼠标位置转换为世界坐标
                Vector2 sprintDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;//求玩家到鼠标方向的单位向量
                sprintAngle = Mathf.Atan2(sprintDir.y, sprintDir.x) * Mathf.Rad2Deg;//转换为与X轴正向的夹角
                SprintEffect.transform.eulerAngles = new Vector3(0, 0, sprintAngle);//这种方式可以不受player左右翻转的影响
                //transform.localRotation = Quaternion.Euler(0, 0, sprintAngle);

                float sRad, dx, dy;
                
                /*
                 * 取消了根据速度方向翻转玩家，则不需要以下的判断
                bool mouseAtRight = mousePos.x - transform.position.x > 0;//鼠标在玩家的右边
                //如果鼠标不在玩家面向的那一边，冲刺时玩家会转向，冲刺特效需要绕y轴对称
                //所以冲刺特效本地旋转（180 - sprintAngle），绕玩家旋转角度取反，相对玩家水平位置也取反
                if (playerFaceRight != mouseAtRight)
                {
                    SprintEffect.transform.eulerAngles = new Vector3(0, 0, 180 - sprintAngle);
                    sRad = -sprintAngle * Mathf.Deg2Rad;//旋转角度
                    //每次都从冲刺特效相对玩家的初始位置开始旋转                                         
                    dx = -sprintEffectOriginVector.x;//(x1 - x2)
                    dy = sprintEffectOriginVector.y;//(y1 - y2)
                }
                else//鼠标在玩家面向的那一边，冲刺时玩家不会转向
                {
                    sRad = sprintAngle * Mathf.Deg2Rad;//旋转角度
                    //每次都从冲刺特效相对玩家的初始位置开始旋转                                         
                    dx = sprintEffectOriginVector.x;//(x1 - x2)
                    dy = sprintEffectOriginVector.y;//(y1 - y2)
                }
                */

                sRad = sprintAngle * Mathf.Deg2Rad;//旋转角度
                //每次都从冲刺特效相对玩家的初始位置开始旋转                                         
                dx = sprintEffectOriginVector.x;//(x1 - x2)
                dy = sprintEffectOriginVector.y;//(y1 - y2)

                //代入公式
                float newx = dx * Mathf.Cos(sRad) - dy * Mathf.Sin(sRad) + transform.position.x;
                float newy = dy * Mathf.Cos(sRad) + dx * Mathf.Sin(sRad) + transform.position.y;
                SprintEffect.transform.position = new Vector2(newx, newy);  
                //点绕点的旋转：               
                //（x1，y1）为要转的点，（x2,y2）为中心点，如果是顺时针角度为θ
                // x = (x1 - x2)cosθ - (y1 - y2)sinθ + x2
                // y = (y1 - y2)cosθ + (x1 - x2)sinθ + y2

                sprintEffectAnimator.SetTrigger("SprintEffect");//播放冲刺特效动画
                playerAnimator.SetBool("Sprint", true);//播放冲刺动作
                AudioManger.Instance.PlayAudio("hero_shade_dash_1", transform.position);//播放冲刺音效
                StartCoroutine(UniversalSprintMove(sprintTime, sprintDir));//启动万向冲刺移动协程UniversalSprintMove        
                StartCoroutine(SprintWait(sprintWaitTime));//启动冲刺冷却协程SprintWait
            }
        }
    }
    /// <summary>
    /// 冲刺移动协程
    /// </summary>
    IEnumerator SprintMove(float time)
    {
        canInput = false;//冲刺时停止输入        
        playerRigidbody.gravityScale = 0;//冲刺时取消重力
        isShowDustTrack = true;//显示灰尘轨迹
        //根据玩家当前方向设置冲刺速度
        float sprintSpeed;
        if (transform.localRotation == Quaternion.Euler(0, 0, 0))//向右
        {
            sprintSpeed = playerSprintSpeed;
        }
        else//向左
        {
            sprintSpeed = -playerSprintSpeed;
        }
        playerRigidbody.velocity = new Vector2(sprintSpeed, 0f);

        yield return new WaitForSeconds(time);//等待一段时间（冲刺时间）后返回
        
        canInput = true;//恢复输入
        playerRigidbody.gravityScale = playerGravityScale;//恢复重力
        isShowDustTrack = false;//停止显示灰尘轨迹
        playerAnimator.SetBool("Sprint", false);//停止播放冲刺动作
    }
    /// <summary>
    /// 万向冲刺移动协程
    /// </summary>
    IEnumerator UniversalSprintMove(float time, Vector2 sprintDir)
    {
        canInput = false;//冲刺时停止输入
        isShowDustTrack = true;//显示灰尘轨迹
        //playerRigidbody.gravityScale = 0;//冲刺时取消重力
        //根据鼠标相对玩家的方向设置冲刺速度
        //Debug.Log(sprintDir);
        //由于sprintDir的模在各个方向上大小不同，竖直和水平较大，斜向较小，所以要加以修正
        playerRigidbody.velocity = playerSprintSpeed * sprintDir / (1.5f*(Mathf.Pow(sprintDir.x, 2) + Mathf.Pow(sprintDir.y, 2)));

        yield return new WaitForSeconds(time);//等待一段时间（冲刺时间）后返回

        canInput = true;//恢复输入
        isShowDustTrack = false;//停止显示灰尘轨迹
        //playerRigidbody.gravityScale = playerGravityScale;//恢复重力
        playerAnimator.SetBool("Sprint", false);//停止播放冲刺动作
    }
    /// <summary>
    /// 冲刺冷却协程
    /// </summary>
    IEnumerator SprintWait(float time)
    {
        canSprint = false;//冲刺冷却中不能再冲刺
        yield return new WaitForSeconds(time);//等待冷却时间后返回
        canSprint = true;//恢复冲刺
    }



    /// <summary>
    /// 爬墙状态
    /// </summary>
    void Climb()
    {
        if (isClimb)//爬墙状态时，如果没有水平速度，则竖直速度也置0，即离墙后才能跳
        {           //避免在墙上垂直跳时，由于重力为0，一直上升的情况
            if(playerRigidbody.velocity.x == 0)
            {
                playerRigidbody.velocity = new Vector2(0, 0);
            }            
        }
    }
    /// <summary>
    /// 爬墙
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canInput || !HasClimb || isCharge)
        {
            return;
        }
        //当碰撞的对象为Wall的BoxCollider时
        if (other.gameObject.CompareTag("climbWall") &&
            other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            //Debug.Log("爬墙");            
            playerRigidbody.gravityScale = 0;//通过将重力设置为0，实现在墙上停留
            playerRigidbody.velocity = new Vector2(0, 0);//速度归0
            isClimb = true;//进入爬墙状态
            playerAnimator.SetBool("Climb", true);
        }
        
    }
    /// <summary>
    /// 离墙
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!canInput || !HasClimb)
        {
            return;
        }
        //当碰撞的对象为Wall的BoxCollider时
        if (other.gameObject.CompareTag("climbWall") &&
            other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            //Debug.Log("离墙");
            playerRigidbody.gravityScale = playerGravityScale;//恢复重力
            isClimb = false;//结束爬墙状态
            isClimbJump = true;//开始爬墙跳状态
            playerAnimator.SetBool("Climb", false);
        }
        
    }


    /// <summary>
    /// 捶地技能
    /// </summary>
    void BeatGround()
    {
        //二段跳状态下处于最高点附近（速度在一定范围内）
        if(isSecondJump && playerRigidbody.velocity.y > -5f && playerRigidbody.velocity.y < 5f )
        {
            //按下捶地键并且蓝值够消耗时
            if (Input.GetKeyDown(BeatGroundKey) && (BlueBar.blueNow >= beatGroundCostBlue))
            {
                BlueBar.blueNow -= beatGroundCostBlue;//扣掉捶地消耗的蓝值

                isSecondJump = false;
                isBeatGround = true;
                Vector2 beatGroundVel = new Vector2(0.0f, beatGroundSpeed);
                playerRigidbody.velocity = Vector2.down * beatGroundVel;//设置捶地速度
                AudioManger.Instance.PlayAudio("HeavyAttack_2", transform.position);//播放叫喊音效  
                isShowDustTrack = true;//显示灰尘轨迹
            }   

        }
    }

    /// <summary>
    /// 蓄力斩
    /// </summary>
    void ChargeCut()
    {
        if (!canInput || isNormalAttack || !HasJapSword)//普通攻击还未结束时不能进入蓄力
        {
            return;
        }

        if (chargeAudioSource == null)//如果没有创建蓄力音效播放物体，则创建并加载蓄力音效，标记为1
        {
            chargeAudioSource = AudioManger.Instance.PlayAudio("Charge2_加快", transform, 1);
            chargeAudioSource.Stop();//但是不播放
        }

        if (isCharge)//如果处于蓄力状态则记录蓄力的时间
        {
            chargeTime += Time.deltaTime;
        }       

        //落地的情况下按下鼠标右键开始格挡，一段时间后转为蓄力
        if (Input.GetMouseButtonDown(1) && isOnGround && (BlueBar.blueNow >= parryCostBlue))//蓝值要够格挡
        {
            chargeTime = 0f;//初始化蓄力时间
            isCharge = true;//开始蓄力
            AudioManger.Instance.PlayAudio("拔刀_低音量", transform.position);//播放拔刀音效             
            StartCoroutine("ChargeAutoQuit");//启动蓄力超时自动退出协程
            chargeAudioSource.Play();//播放蓄力音效

            BlueBar.blueNow -= parryCostBlue;//扣掉格挡消耗的蓝值

            playerAnimator.SetBool("Parry", true);  //播放格挡动画            
            parryEffect.SetActive(true);            //显示格挡粒子特效
            parryCollider.enabled = true;           //启用格挡范围碰撞体

            StartCoroutine("ParryToCharge");//启动格挡转蓄力协程
        }
        //格挡或者蓄力时鼠标右键弹起
        if (Input.GetMouseButtonUp(1) && isCharge)
        {
            //Debug.Log(chargeTime);
            isCharge = false;//停止蓄力
            if (chargeTime < 0.3f)//蓄力时间小于0.3s
            {                
                playerAnimator.SetBool("Parry", false); //停止格挡动画
                parryEffect.SetActive(false);           //隐藏格挡粒子特效
                parryCollider.enabled = false;          //停用格挡范围碰撞体

                StopCoroutine("ParryToCharge");//停止格挡转蓄力协程
                StopCoroutine("ChargeAutoQuit");//停止蓄力超时自动退出协程
                chargeAudioSource.Stop();//停止播放蓄力音效
            }
            else if (chargeTime < 2.5f)//蓄力时间小于2.5s
            {                
                StopCoroutine("ChargeAutoQuit");            //停止蓄力超时自动退出协程
                playerAnimator.SetBool("Charge", false);    //停止播放蓄力动画
                chargeEffect.SetActive(false);              //隐藏蓄力粒子特效
                chargeAudioSource.Stop();                   //停止播放蓄力音效
            }
            else if (chargeTime < 3.1f)//蓄力时间大于于2.5s，小于3.1s，***蓄力成功***
            {
                //如果蓝值够蓄力攻击的，则发动蓄力攻击
                if(BlueBar.blueNow >= chargeCutCostBlue)
                {
                    BlueBar.blueNow -= chargeCutCostBlue;//扣掉蓄力攻击消耗的蓝值
                    playerRigidbody.velocity = new Vector2(0, 0);//发动蓄力攻击时速度归零
                    canInput = false;//发动蓄力攻击时不能有其他输入

                    chargeTime = 0f;
                    StopCoroutine("ChargeAutoQuit");            //停止蓄力超时自动退出协程
                    playerAnimator.SetBool("Charge", false);    //停止播放蓄力动画
                    chargeEffect.SetActive(false);              //隐藏蓄力粒子特效

                    AudioManger.Instance.PlayAudio("HeavyAttack_2", transform.position);//播放叫喊音效   
                    playerAnimator.SetTrigger("Cut");           //播放斩击动画
                    AudioManger.Instance.PlayAudio("BeatGround_0", transform.position); //播放斩地音效
                    CameraShake.camShake.Shake();//调用相机震动
                    chargeCutCollider.enabled = true;           //启用蓄力攻击范围碰撞体
                    Invoke("DisableChargeCutCollider", 0.5f);   //0.5s后停用蓄力攻击范围碰撞体
                    Instantiate(beatGroundDustEffect, chargeCutDustPos.position, Quaternion.Euler(-90, 0, 0));
                    //Instantiate(beatGroundDustEffect, chargeCutDustPos.position, Quaternion.Euler(0, 0, 0));
                }
                else//否则当蓄力不成功处理
                {
                    StopCoroutine("ChargeAutoQuit");            //停止蓄力超时自动退出协程
                    playerAnimator.SetBool("Charge", false);    //停止播放蓄力动画
                    chargeEffect.SetActive(false);              //隐藏蓄力粒子特效
                    chargeAudioSource.Stop();                   //停止播放蓄力音效
                }
                
            }
        }
        //格挡或者蓄力时鼠标左键按下
        if (Input.GetMouseButtonDown(0) && isCharge)
        {
            isCharge = false;//停止蓄力
            StopCoroutine("ChargeAutoQuit");//停止蓄力超时自动退出协程
            chargeAudioSource.Stop();       //停止播放蓄力音效    

            if (chargeTime < 0.3f)//蓄力时间小于0.3s，即格挡状态按下左键
            {
                playerAnimator.SetBool("Parry", false); //停止格挡动画
                parryEffect.SetActive(false);           //隐藏格挡粒子特效
                parryCollider.enabled = false;          //停用格挡范围碰撞体
                StopCoroutine("ParryToCharge");         //停止格挡转蓄力协程                
            }
            else//蓄力时间大于0.3s，即蓄力状态按下左键，启动minChargeCut攻击
            {
                //如果蓝值够min蓄力攻击的，则发动min蓄力攻击
                if (BlueBar.blueNow >= minChargeCutCostBlue)
                {
                    BlueBar.blueNow -= minChargeCutCostBlue;//扣掉min蓄力攻击消耗的蓝值

                    chargeEffect.SetActive(false);  //隐藏蓄力粒子特效
                    AudioManger.Instance.PlayAudio("挥刀", transform.position); //播放挥刀音效
                    playerAnimator.SetBool("minChargeCut", true);               //播放minChargeCut动画
                    minChargeCutCollider.enabled = true;                        //启用minChargeCut攻击范围碰撞体
                    Invoke("StopMinChargeCut", 0.3f);//0.3后停止播放minChargeCut动画，并停用minChargeCut攻击范围碰撞体                    
                }
                else//否则，相当于蓄力结束
                {
                    StopCoroutine("ChargeAutoQuit");            //停止蓄力超时自动退出协程
                    playerAnimator.SetBool("Charge", false);    //停止播放蓄力动画
                    chargeEffect.SetActive(false);              //隐藏蓄力粒子特效
                    chargeAudioSource.Stop();                   //停止播放蓄力音效
                }
                
            }

        }
    }
    //////格挡动画转为蓄力动画
    IEnumerator ParryToCharge()
    {
        yield return new WaitForSeconds(0.3f);
        //由于时间误差，执行到这里的时候chargeTime已经大于0.3s
        //如果在0.3s之后的这段时间内，右键抬起或者左键按下，就不能再继续播放蓄力动画和特效
        //所以先判断一下isCharge
        if (isCharge)
        {
            playerAnimator.SetBool("Charge", true); //播放蓄力动画
            chargeEffect.SetActive(true);           //显示蓄力粒子特效
        }
        parryEffect.SetActive(false);           //隐藏格挡粒子特效
        parryCollider.enabled = false;          //停用格挡范围碰撞体
        Invoke("StopParryAnim", 0.01f);         //延迟一会儿停止格挡动画，避免直接转换到Idle
    }
    //////停止格挡动画
    void StopParryAnim()
    {
        playerAnimator.SetBool("Parry", false);
    }
    //////蓄力3.1s后还未抬起右键，则蓄力超时
    IEnumerator ChargeAutoQuit()
    {
        yield return new WaitForSeconds(3.1f);
        isCharge = false;//停止蓄力
        playerAnimator.SetBool("Charge", false);                    //停止播放蓄力动画
        chargeEffect.SetActive(false);                              //隐藏蓄力粒子特效
        AudioManger.Instance.PlayAudio("喘息声", transform.position);//播放喘息声
    }
    //////停用蓄力攻击范围碰撞体
    void DisableChargeCutCollider()
    {
        canInput = true;
        chargeCutCollider.enabled = false;
    }
    //////停止播放minChargeCut动画，并停用minChargeCut攻击范围碰撞体
    void StopMinChargeCut()
    {
        minChargeCutCollider.enabled = false;           //停用minChargeCut攻击范围碰撞体
        playerAnimator.SetBool("Charge", false);        //停止播放蓄力动画
        playerAnimator.SetBool("minChargeCut", false);  //停止播放minChargeCut动画
    }


    /// <summary>
    /// 玩家重力控制
    /// </summary>
    void playerGravity()
    {
        if (CameraFollow.cameraRotate)//若相机在旋转
        {
            hasResetGravity = false; //重力待恢复
            playerRigidbody.gravityScale = 0;
            canRun = false;
            /*
            //始终在相机视野的正下方向添加重力
            float gRad = CameraFollow.cameraAngle * Mathf.Deg2Rad;
            float x = Vector2.down.x;
            float y = Vector2.down.y;
            Vector2 gravityVector = new Vector2(x * Mathf.Cos(gRad) - y * Mathf.Sin(gRad)
                , y * Mathf.Cos(gRad) + x * Mathf.Sin(gRad)).normalized;
            //Vector3 gravityVector3 = new Vector3(x * Mathf.Cos(gRad) - y * Mathf.Sin(gRad)
            //    , y * Mathf.Cos(gRad) + x * Mathf.Sin(gRad)
            //    , 0).normalized;
            //Debug.DrawLine(transform.position, transform.position + 40*gravityVector3, Color.red);
            playerRigidbody.AddForce(gravityVector * 40);
            */
        }
        else//若相机不在旋转
        {
            if (!hasResetGravity)//并且没有恢复重力，确保以下操作只进行一次
            {
                playerRigidbody.gravityScale = playerGravityScale;//则恢复重力
                hasResetGravity = true;//已经恢复重力
                canRun = true;
            }
        }        
    }

    /// <summary>
    /// 恢复蓝条
    /// </summary>
    void RestoreBlue()
    {
        if((BlueBar.blueNow < BlueBar.blueMax) && canRestoreBlue)
        {
            canRestoreBlue = false;//计时时锁定恢复
            StartCoroutine("RestoreOneBlue");//启动定时恢复一个蓝协程
        }
    }
    //////定时恢复一个蓝
    IEnumerator RestoreOneBlue()
    {
        yield return new WaitForSeconds(blueRestoreTime);
        BlueBar.blueNow += 1;
        canRestoreBlue = true;        
    }

    /// <summary>
    /// 格挡武器成功时后退
    /// </summary>
    public void ParriedBack()
    {
        canRun = false;//停止Run()对玩家左右方向速度的控制
        playerRigidbody.velocity = new Vector2(playerFaceRight ? -5 : 5, 0);//向玩家朝向的反方向添加一个后退速度
        Invoke("ParriedBackEnd", 0.2f);
    }
    void ParriedBackEnd()
    {
        canRun = true;//恢复Run()对玩家左右方向速度的控制
    }


    /// <summary>
    /// 显示移动时的灰尘轨迹
    /// </summary>
    void ShowDustTrack()
    {
        if (isShowDustTrack)
        {
            Instantiate(dustTrackEffect, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}
