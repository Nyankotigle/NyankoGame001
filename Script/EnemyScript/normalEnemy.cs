using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//普通敌人normalEnemy的抽象父类
public abstract class normalEnemy : MonoBehaviour
{
    [Header("血量")]
    public int health;
    [Header("伤害")]
    public int damage;
    [Header("受到伤害时变红持续时间")]
    public float hurtFlashTime;
    [Header("流血特效")]
    public GameObject bloodEffect;
    [Header("血烟特效")]
    public GameObject bloodSmokeEffect;
    [Header("掉落金币预制体")]
    public GameObject dropCoin;

    private Animator enemyAnimator;
    private SpriteRenderer flyEnemy01SpriteRender;//敌人的渲染器
    private Color originalColor;//原始颜色

    private PlayerHealth playerHealth;//玩家血量管理类

    // Start is called before the first frame update
    public void Start()
    {
        flyEnemy01SpriteRender = GetComponent<SpriteRenderer>();//获取渲染器
        originalColor = flyEnemy01SpriteRender.color;//获取原始颜色
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    /// <summary>
    /// 销毁敌人
    /// </summary>
    void DestroyEnemy()
    {
        Destroy(gameObject);//死亡烟雾动画播放完后销毁敌人
    }

    /// <summary>
    /// 受到格挡后退
    /// </summary>
    public virtual void BeParried()
    {

    }

    /// <summary>
    /// 敌人掉血
    /// </summary>
    public void TakeDamage(int damage)
    {
        health -= damage;

        FlashColor(hurtFlashTime);//掉血的红色闪烁效果
        Instantiate(bloodSmokeEffect, transform.position, Quaternion.Euler(0, 0, 0));//实例化血雾特效
        Instantiate(bloodEffect, transform.position, Quaternion.Euler(0, 0, 0));//实例化流血粒子特效

        if (health <= 0)
        {
            enemyAnimator.SetTrigger("IsDead");//播放死亡动画
            Invoke("DestroyEnemy", 0.3f);//0.3s后销毁敌人 
            Instantiate(dropCoin, transform.position, Quaternion.identity);//实例化金币
            return;
        }
        
    }
    /// <summary>
    /// 掉血变红
    /// </summary>
    void FlashColor(float time)
    {
        flyEnemy01SpriteRender.color = Color.red;
        Invoke("ResetColor", time);//一段时间后恢复颜色
    }
    /// <summary>
    /// 恢复敌人原来的颜色
    /// </summary>
    void ResetColor()
    {
        flyEnemy01SpriteRender.color = originalColor;
    }

    /// <summary>
    /// 对玩家的伤害
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {    
        //当碰撞的对象为player的胶囊碰撞体时
        if(other.gameObject.CompareTag("playerV") && 
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if(playerHealth != null)//避免玩家死亡时出错
            {
                playerHealth.DamagePlayer(damage);//调用玩家伤害函数
            }            
        }
    }
}
