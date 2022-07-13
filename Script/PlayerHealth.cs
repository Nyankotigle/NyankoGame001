using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("玩家血量")]
    public int health;
    [Header("受伤闪烁次数")]
    public int blinkNum;
    [Header("受伤闪烁时间")]
    public float blinkTime;
    [Header("背后的剑")]
    public GameObject Swards;
    [Header("左轮")]
    public GameObject Revolver;
    [Header("玩家死亡特效")]
    public GameObject playerDeadEffect;
    [Header("全局光照")]
    public GameObject globalLightObj;

    public static bool playerIsDead;
    public static Vector2 revivePos;//复活位置

    private Renderer playerRender;
    private Animator playerAnimator;
    private GlobalLight2D globalLight2d;//全局光照管理

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.healthMax = health;//初始化血条最大值
        HealthBar.healthNow = health;//初始化血条当前值
        playerRender = GetComponent<Renderer>();
        playerAnimator = GetComponent<Animator>();
        playerDeadEffect = Resources.Load<GameObject>("Prefebs/PlayerDeadEffect");//加载玩家死亡特效预制体
        globalLight2d = globalLightObj.GetComponent<GlobalLight2D>();//获取全局光照的GlobalLight2D组件
        playerIsDead = false;
        revivePos = transform.position;//初始化复活位置
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 结算伤害
    /// </summary>
    public void DamagePlayer(int damage)
    {
        if (playerIsDead)
        {
            return;
        }

        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        HealthBar.healthNow = health;//更新血条
        if(health <= 0)
        {           
            Swards.SetActive(false);//隐藏背后的剑
            PlayerController.useRevolver = false;
            Revolver.SetActive(false);//隐藏左轮
            AudioManger.Instance.PlayAudio("死亡音效2", transform.position);//死亡音效2

            //在死亡位置实例化死亡粒子特效物体
            var playerDeadEffectObj = Instantiate(playerDeadEffect, transform.position, Quaternion.Euler(0, 0, 0));
            playerDeadEffectObj.transform.position -= new Vector3(0, 0, 0);//调整特效生成位置  

            playerAnimator.SetBool("Dead", true);//播放死亡动画
            playerIsDead = true;//标记玩家死亡，以显示死字                       

            //Invoke("DestroyPlayer", 1f);//1s后销毁玩家，并且显示死亡界面 
            Invoke("RevivePlayer1", 1f);//1s后显示死亡界面，并且重生玩家
            return;
        }
        BlinkPlayer(blinkNum, blinkTime);//受伤闪烁
        CameraShake.camShake.Shake();//调用相机震动
        AudioManger.Instance.PlayAudio("playerHurt01", transform.position);//受伤音效
    }
    /// <summary>
    /// 销毁玩家
    /// </summary>
    void DestroyPlayer()
    {
        Destroy(gameObject);
        globalLight2d.SetGlobalLightColor(new Color(100, 0, 0));//设置全局光照的颜色为红色        
    }
    /// <summary>
    /// 在最近的重生点复活玩家
    /// </summary>
    void RevivePlayer1()
    {
        globalLight2d.SetGlobalLightColor(new Color(100, 0, 0));//设置全局光照的颜色为红色
        Invoke("RevivePlayer2", 2f);//2s后在最近的重生点重生玩家
    }
    void RevivePlayer2()
    {
        globalLight2d.SetGlobalLightColor(new Color(1, 1, 1));//恢复全局光照的颜色
        transform.position = revivePos;
        playerAnimator.SetBool("Dead", false);
        health = HealthBar.healthMax;
        HealthBar.healthNow = health;
        playerIsDead = false;
    }



    /// <summary>
    /// 受伤闪烁
    /// </summary>
    void BlinkPlayer(int blinkNum, float blinkTime)
    {
        StartCoroutine(DoBlinks(blinkNum, blinkTime));
    }
    IEnumerator DoBlinks(int blinkNum, float blinkTime)
    {
        for(int i = 0; i < blinkNum * 2; i++)
        {    
            //通过玩家不断显示和隐藏实现闪烁
            playerRender.enabled = !playerRender.enabled;
            yield return new WaitForSeconds(blinkTime);
        }
        playerRender.enabled = true;//最后确保为显示
    }

}
