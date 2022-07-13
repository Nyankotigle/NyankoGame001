using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap03Damage : MonoBehaviour
{
    [Header("Trap03伤害")]
    public int damage;

    private PlayerHealth playerHealth;
    private bool isOnTrap03;//是否停留在Trap03上

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
        //玩家的胶囊碰撞体碰到Trap03
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap03 = true;                  //玩家在Trap03上
            playerHealth.DamagePlayer(damage);  //产生伤害
            StartCoroutine(Trap03_Damage());      //启动连续伤害协程
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //玩家的胶囊碰撞体离开Trap03
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap03 = false;//玩家不在Trap03上
        }
    }
    //Trap03连续伤害协程
    IEnumerator Trap03_Damage()
    {
        //当玩家接触到Trap03后每隔0.5秒检查玩家是否仍在Trap03上，若是继续造成伤害，并开启下一个0.5秒计时
        yield return new WaitForSeconds(0.5f);
        if (isOnTrap03)
        {
            playerHealth.DamagePlayer(damage);
            StartCoroutine(Trap03_Damage());
        }
    }
}
