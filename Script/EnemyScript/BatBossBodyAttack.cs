using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBossBodyAttack : MonoBehaviour
{
    [Header("基础身体伤害")]
    public int bodyDamage;

    private PlayerHealth playerHealth;//玩家血量管理类
    private bool isTouchBoss;//是否接触boss
    private int damage;//接触伤害随单次接触的时间增加而翻倍

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();
        isTouchBoss = false;
        damage = bodyDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 对玩家的伤害
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //当碰撞的对象为player的胶囊碰撞体时
        if (other.gameObject.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isTouchBoss = true;
            if (playerHealth != null)//避免玩家死亡时出错
            {
                playerHealth.DamagePlayer(bodyDamage);//调用玩家伤害函数
                StartCoroutine(BossBodyDamage());//启动连续伤害协程
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //玩家的胶囊碰撞体离开batboss身体
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isTouchBoss = false;//玩家不在接触boss
            damage = bodyDamage;//恢复接触伤害
        }
    }

    //batboss身体连续伤害协程
    IEnumerator BossBodyDamage()
    {
        //当玩家接触到batboss后每隔0.5秒检查玩家是否仍在接触batboss，若是继续造成伤害，并开启下一个0.5秒计时
        yield return new WaitForSeconds(0.5f);
        if (isTouchBoss)
        {
            damage += bodyDamage;//每隔0.5s伤害增加一个bodyDamage
            if (playerHealth != null)//避免玩家死亡时出错
            {
                playerHealth.DamagePlayer(damage);//调用玩家伤害函数
                StartCoroutine(BossBodyDamage());//启动连续伤害协程
            }
        }
    }
}
