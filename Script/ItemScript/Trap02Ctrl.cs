using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap02Ctrl : MonoBehaviour
{
    [Header("Trap02伤害")]
    public int damage;

    private PlayerHealth playerHealth;
    private bool isOnTrap02;//是否停留在Trap01上

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
        //玩家的胶囊碰撞体碰到Trap02
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap02 = true;                  //玩家在Trap02上
            playerHealth.DamagePlayer(damage);  //产生伤害
            StartCoroutine(Trap02Damage());      //启动连续伤害协程
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //玩家的胶囊碰撞体离开Trap02
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap02 = false;//玩家不在Trap02上
        }
    }
    //Trap02连续伤害协程
    IEnumerator Trap02Damage()
    {
        //当玩家接触到Trap02后每隔0.5秒检查玩家是否仍在Trap02上，若是继续造成伤害，并开启下一个0.5秒计时
        yield return new WaitForSeconds(0.5f);
        if (isOnTrap02)
        {
            playerHealth.DamagePlayer(damage);
            StartCoroutine(Trap02Damage());
        }
    }
}
