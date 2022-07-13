using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatBossCtrl : MonoBehaviour
{
    [Header("血量")]
    public int maxHealth;
    [Header("受到伤害时变红持续时间")]
    public float hurtFlashTime;
    [Header("流血特效")]
    public GameObject bloodEffect;
    [Header("血烟特效")]
    public GameObject bloodSmokeEffect;
    [Header("掉落金币预制体")]
    public GameObject dropCoin;
    [Header("停止追击位置")]
    public Transform stopChasePos;
    [Header("移动范围左边")]
    public Transform leftRangePos;
    [Header("移动范围右边")]
    public Transform rightRangePos;
    [Header("武器攻击范围")]
    public GameObject armsAttackRange;
    [Header("对话框物体组件")]
    public GameObject DialogBox;
    [Header("对话框文字组件")]
    public Text dialogBoxText;
    [Header("boss看守的门door01_2")]
    public GameObject door01_2;
    [Header("隐藏宝箱的门door01_3")]
    public GameObject door01_3;
    [Header("boss血条")]
    public GameObject bossHealthBar;

    public static bool batBossIsDead;//batBoss死亡

    private Transform playerPos;//玩家位置
    private int xMoveSpeed;//batBoss移动速度
    private int health;//batBoss当前血量

    private Animator batBossAnimator;
    private BoxCollider2D batBossFeet;//脚部碰撞器，地面检测
    private SpriteRenderer batBossSpriteRender;//batBoss的渲染器
    private Color originalColor;//原始颜色
    private Rigidbody2D batBossRigidbody;
    private BoxCollider2D armsCollider;
    private Vector2 startPos;//初始位置
    private Door01Ctrl door01_2Ctrl;
    private Door01Ctrl door01_3Ctrl;

    private bool isOnGround;    //是否在地面
    private bool isAttack;      //正在攻击
    private bool canAttack;     //可以攻击
    private bool isStiff;       //硬直状态
    private bool isParriedBack; //被格挡击退
    private bool isBack;        //回到初始位置
    private float WaitAttackTime; //等待攻击时间
    private bool hasPlaySuccessAudio;//已经播放成功击败boss音效
    private bool findPlayer;    //发现玩家，开始战斗



    // Start is called before the first frame update
    void Start()
    {
        batBossSpriteRender = GetComponent<SpriteRenderer>();//获取渲染器
        originalColor = batBossSpriteRender.color;//获取原始颜色
        batBossRigidbody = GetComponent<Rigidbody2D>();
        batBossFeet = GetComponent<BoxCollider2D>();//获取脚部碰撞器

        batBossAnimator = GetComponentInChildren<Animator>();
        armsCollider = armsAttackRange.GetComponent<BoxCollider2D>();
        armsCollider.enabled = false;
        startPos = transform.position;
        playerPos = GameObject.FindGameObjectWithTag("playerV").GetComponent<Transform>();//获取玩家位置
        door01_2Ctrl = door01_2.GetComponent<Door01Ctrl>();
        door01_2Ctrl.canEnter = false;//door01_2初始不能打开
        door01_3Ctrl = door01_3.GetComponent<Door01Ctrl>();
        door01_3Ctrl.canEnter = true;//door01_3初始能打开

        health = maxHealth;//初始化当前血量

        isAttack = false;
        canAttack = true;
        isStiff = false;
        isParriedBack = false;
        isBack = false;
        batBossIsDead = false;

        hasPlaySuccessAudio = false;
        findPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyMove();
        //EnemyFlip();
        CheckOnGround();
        Flip();
        moveDir();
        Walk();
        Attack();
        RestoreHealth();
    }


    /// <summary>
    /// 检测batBoss是否在地面
    /// </summary>
    void CheckOnGround()
    {
        isOnGround = batBossFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// batBoss左右翻转
    /// </summary>
    void Flip()
    {
        //是否有水平速度
        bool playerHasXAxisSpeed = Mathf.Abs(batBossRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (batBossRigidbody.velocity.x > 0.1f)//有向右的水平速度
            {               
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (batBossRigidbody.velocity.x < -0.1f)//有向左的水平速度
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);               
            }
        }
    }

    /// <summary>
    /// batBoss移动方向
    /// </summary>
    void moveDir()
    {
        if(playerPos == null || isStiff)
        {
            return;
        }
        if(transform.position.x < stopChasePos.position.x)//过了停止追击点就回城
        {
            xMoveSpeed = 10;
            isBack = true;
        }
        else
        {
            if (isBack)//如果是回城状态
            {
                if(transform.position.x > startPos.x)//回城成功
                {
                    isBack = false;
                    xMoveSpeed = 0;
                }
            }
            else
            {
                //如果与玩家水平距离小于10，且竖直距离小于3
                if (Mathf.Abs(transform.position.x - playerPos.position.x) < 10 && Mathf.Abs(transform.position.y - playerPos.position.y) < 3 && isOnGround)
                {
                    if (!findPlayer)//首次发现玩家，开始战斗
                    {
                        findPlayer = true;
                        BossHealthBar.healthMax = health;
                        BossHealthBar.healthNow = health;
                        BossHealthBar.bossName = "异形蝙蝠";
                        bossHealthBar.SetActive(true);//显示boss血条

                    }
                    if (playerPos.position.x < transform.position.x)//在玩家右边
                    {
                        xMoveSpeed = -4;
                    }
                    if (playerPos.position.x > transform.position.x)//在玩家左边
                    {
                        xMoveSpeed = 4;
                    }               

                }
                //如果与玩家水平距离大于10，且竖直距离小于3，在地面，并且玩家在停止追击点右侧的时候，开始起跳动作
                if (Mathf.Abs(transform.position.x - playerPos.position.x) > 10 && Mathf.Abs(transform.position.y - playerPos.position.y) < 3 
                    && isOnGround && playerPos.position.x > stopChasePos.position.x)
                {
                    if (playerPos.position.x < transform.position.x )//在玩家右边
                    {
                        batBossRigidbody.velocity = new Vector2(-20, 25);
                        xMoveSpeed = -10;
                    }
                    if (playerPos.position.x > transform.position.x )//在玩家左边
                    {
                        batBossRigidbody.velocity = new Vector2(20, 25);
                        xMoveSpeed = 10;
                    }                
                }
            }
        }        
    }

    /// <summary>
    /// Walk和相关动画切换
    /// </summary>
    void Walk()
    {
        if (isStiff || isAttack || isParriedBack)
        {
            return;
        }
        Vector2 batBossVel = new Vector2(xMoveSpeed, batBossRigidbody.velocity.y);
        batBossRigidbody.velocity = batBossVel;
        //是否有水平速度
        bool playerHasXAxisSpeed = Mathf.Abs(batBossRigidbody.velocity.x) > Mathf.Epsilon;
        batBossAnimator.SetBool("Walk", playerHasXAxisSpeed);
        PlayRunAudio(playerHasXAxisSpeed);//播放脚步音效

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
            InitRunAudioObj();// 初始化脚步声音播放物体
        }
        if (hasXSpeed && !isPlayRunAudio)
        {
            isPlayRunAudio = true;
            runAudioSource.Play();
        }
        else if (!hasXSpeed)
        {
            runAudioSource.Stop();//没有水平速度停止播放脚步声
            isPlayRunAudio = false;
        }
    }


    /// <summary>
    /// BatBoss攻击
    /// </summary>
    void Attack()
    {
        if (playerPos == null)
        {
            return;
        }
        if (playerPos == null)
        {
            return;
        }
        if (isStiff)
        {
            return;
        }
        if (Mathf.Abs(transform.position.x - playerPos.position.x) < 6 && canAttack
            && Mathf.Abs(transform.position.y - playerPos.position.y) < 2)
        {
            canAttack = false;
            WaitAttackTime = Random.Range(0.5f, 1.5f);
            Invoke("SoundBeforeAttack", WaitAttackTime);//随机等待1-2之间的一个时间后开始播放攻击前摇音效  
        }
    }   
    void SoundBeforeAttack()
    {
        AudioManger.Instance.PlayAudio("batBoss咆哮", playerPos.position);//播放攻击前摇音效
        Invoke("StartAttackAnim", 0.5f);//攻击前摇音效后0.5s开始切换到攻击动画
    }
    void StartAttackAnim()
    {
        isAttack = true;
        batBossAnimator.SetBool("Attack", true);
        Invoke("EnableArmsCollider", 0.1f);//
    }
    void EnableArmsCollider()
    {
        armsCollider.enabled = true;
        Invoke("DisableArmsCollider", 0.5f);//
    }
    void DisableArmsCollider()
    {
        armsCollider.enabled = false;
        Invoke("StopAttackAnim", 0.1f);//
    }
    void StopAttackAnim()
    {
        isAttack = false;
        batBossAnimator.SetBool("Attack", false);
        Invoke("CanNextAttack", 0.5f);//
    }
    void CanNextAttack()
    {
        canAttack = true;
    }


    /// <summary>
    /// 受到格挡后退
    /// </summary>
    public void BeParried()
    {
        isParriedBack = true;
        if (PlayerController.playerFaceRight)//向玩家面向的方向后退
        {
            batBossRigidbody.velocity = new Vector2(10, 0);
        }
        else
        {
            batBossRigidbody.velocity = new Vector2(-10, 0);
        }
        Invoke("ParriedBackEnd", 0.5f);
    }
    void ParriedBackEnd()
    {
        isParriedBack = false;//停止后退
    }

    /// <summary>
    /// 武器受到格挡
    /// </summary>
    public void ArmsBeParried()
    {
        BeParried();
        isStiff = true;//硬直状态
        batBossAnimator.SetBool("Hurt", true);
        Invoke("StopStiff", 3f);
    }
    void StopStiff()
    {
        isStiff = false;
        batBossAnimator.SetBool("Hurt", false);
    }

    /// <summary>
    /// 敌人掉血
    /// </summary>
    public void TakeDamage(int damage)
    {        
        health -= damage;
        if(health < 0)
        {
            health = 0;
        }
        BossHealthBar.healthNow = health;//更新血条
        if (damage >= 10)//如果受到大于等于10点伤害
        {
            isStiff = true;//进入硬直状态
            batBossAnimator.SetBool("Hurt", true);
            Invoke("StopStiff", 3.0f);
        }
        if (health <= 0)
        {
            batBossAnimator.SetBool("Death" , true);//播放死亡动画
            bossHealthBar.SetActive(false);//隐藏血条
            Invoke("HideBatBoss", 0.5f);//0.5s后隐藏敌人 
            batBossIsDead = true;
            Instantiate(dropCoin, transform.position, Quaternion.identity);//实例化掉落物品

            if (!hasPlaySuccessAudio)//如果还没有播放成功击败boss音效则播放
            {
                AudioManger.Instance.PlayAudio("解谜成功音效2", transform.position);//播放成功击败boss音效
                hasPlaySuccessAudio = true;//已经播放成功击败boss音效
            }

            door01_2Ctrl.canEnter = true;//boss死后door01_2可以打开，玩家可以通往控制室
            door01_3Ctrl.canEnter = false;//boss死后door01_3无法打开，隐藏的宝箱无法获取

            PlayerController.isUniversalSprint = true;//可以使用万向冲刺技能
            dialogBoxText.text = "获得蝙蝠之力，可以使用万向冲刺技能，鼠标位置可以控制冲刺的方向！".Replace("\\n", "\n");
            DialogBox.SetActive(true);
            Invoke("HideDialogBox", 4f);

            return;
        }
        FlashColor(hurtFlashTime);//掉血的红色闪烁效果
        Instantiate(bloodSmokeEffect, transform.position, Quaternion.Euler(0, 0, 0));//实例化血雾特效
        Instantiate(bloodEffect, transform.position, Quaternion.Euler(0, 0, 0));//实例化流血粒子特效
    }
    void HideDialogBox()
    {
        DialogBox.SetActive(false);
        Destroy(gameObject);
    }
    /// <summary>
    /// 销毁敌人
    /// </summary>
    void HideBatBoss()
    {
        gameObject.SetActive(false);//死亡烟雾动画播放完后销毁敌人
    }


    /// <summary>
    /// 掉血变红
    /// </summary>
    void FlashColor(float time)
    {
        batBossSpriteRender.color = Color.red;
        Invoke("ResetColor", time);//一段时间后恢复颜色
    }
    /// <summary>
    /// 恢复敌人原来的颜色
    /// </summary>
    void ResetColor()
    {
        batBossSpriteRender.color = originalColor;
    }

    /// <summary>
    /// player死亡后恢复boss血量
    /// </summary>
    void RestoreHealth()
    {
        if (PlayerHealth.playerIsDead)//如果检测刀玩家死亡
        {
            health = maxHealth;//重置当前血量
            BossHealthBar.healthNow = health;
            bossHealthBar.SetActive(false);//隐藏血条
            findPlayer = false;//需要等待再次发现玩家
        }
    }

}
