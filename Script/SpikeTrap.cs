using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("地刺伤害")]
    public int damage;

    private PlayerHealth playerHealth;
    private bool isOnSpike;//是否停留在地刺上

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家的胶囊碰撞体碰到地刺
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnSpike = true;//玩家在地刺上
            playerHealth.DamagePlayer(damage);//产生伤害
            StartCoroutine(SpikeDamage());//启动连续伤害协程
        }
            
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //玩家的胶囊碰撞体离开地刺
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnSpike = false;//玩家不在地刺上
        }
    }
    //地刺连续伤害协程
    IEnumerator SpikeDamage()
    {
        //当玩家接触到地刺后每隔0.5秒检查玩家是否仍在地刺上，若是继续造成伤害，并开启下一个0.5秒计时
        yield return new WaitForSeconds(0.5f);
        if (isOnSpike)
        {
            playerHealth.DamagePlayer(damage);
            StartCoroutine(SpikeDamage());
        }
    }
}
