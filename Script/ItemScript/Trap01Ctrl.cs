using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap01Ctrl : MonoBehaviour
{
    [Header("Trap01伤害")]
    public int damage;
    [Header("Trap01每周期开启时间")]
    public float onTime;
    [Header("Trap01每周期关闭时间")]
    public float offTime;

    private float time;
    private bool trap01IsOn;
    private Animator trap01Animator;
    private BoxCollider2D trap01Collider;

    private PlayerHealth playerHealth;
    private bool isOnTrap01;//是否停留在Trap01上

    // Start is called before the first frame update
    void Start()
    {
        trap01Animator = GetComponent<Animator>();
        trap01Collider = GetComponent<BoxCollider2D>();
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();
        time = 0f;
        trap01IsOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > offTime && !trap01IsOn)
        {
            trap01IsOn = true;
            trap01Animator.SetBool("Trap01_On", true);
            trap01Collider.enabled = true;
            time = 0;
        }
        if(time > onTime && trap01IsOn)
        {
            trap01IsOn = false;
            trap01Animator.SetBool("Trap01_On", false);
            trap01Collider.enabled = false;
            time = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家的胶囊碰撞体碰到Trap01
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap01 = true;                  //玩家在Trap01上
            playerHealth.DamagePlayer(damage);  //产生伤害
            StartCoroutine(Trap01Damage());      //启动连续伤害协程
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //玩家的胶囊碰撞体离开Trap01
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isOnTrap01 = false;//玩家不在Trap01上
        }
    }
    //Trap01连续伤害协程
    IEnumerator Trap01Damage()
    {
        //当玩家接触到Trap01后每隔0.5秒检查玩家是否仍在Trap01上，若是继续造成伤害，并开启下一个0.5秒计时
        yield return new WaitForSeconds(0.5f);
        if (isOnTrap01)
        {
            playerHealth.DamagePlayer(damage);
            StartCoroutine(Trap01Damage());
        }
    }
}
