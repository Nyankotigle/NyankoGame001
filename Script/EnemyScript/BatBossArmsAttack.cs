using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBossArmsAttack : MonoBehaviour
{
    [Header("武器伤害")]
    public int armsDamage;

    private PlayerHealth playerHealth;//玩家血量管理类

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();

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
            if (playerHealth != null)//避免玩家死亡时出错
            {
                playerHealth.DamagePlayer(armsDamage);//调用玩家伤害函数
            }
        }
    }
}
