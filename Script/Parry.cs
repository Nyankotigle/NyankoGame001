using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [Header("格挡伤害值")]
    public int parryDamage;
    [Header("格挡武器成功动画")]
    public GameObject parryArmsSuccess;
    [Header("格挡武器成功火花特效")]
    public GameObject parryArmsSparkEffect;

    private Animator parryArmSucAnim;

    // Start is called before the first frame update
    void Start()
    {
        parryArmSucAnim = parryArmsSuccess.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //格挡的碰撞体与普通敌人碰撞时
        if (other.gameObject.CompareTag("normalEnemy"))
        {
            AudioManger.Instance.PlayAudio("格挡成功2", transform.position);//播放格挡成功音效
            TimePause.Instance.PauseTime(10);//时间停顿
            other.GetComponent<normalEnemy>().BeParried();//敌人受到格挡
        }
        //格挡碰撞体与batBoss的多边形碰撞体碰撞时
        if (other.gameObject.CompareTag("batBoss") &&
            other.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            AudioManger.Instance.PlayAudio("格挡成功2", transform.position);//播放格挡成功音效
            TimePause.Instance.PauseTime(10);//时间停顿
            other.GetComponent<BatBossCtrl>().BeParried();//敌人受到格挡
        }
        //格挡碰撞体与batBoss的武器碰撞体碰撞时
        if (other.gameObject.CompareTag("EnemyArms") &&
            other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            //AudioManger.Instance.PlayAudio("parrySucess", transform.position);//播放格挡音效
            AudioManger.Instance.PlayAudio("格挡成功3", transform.position);//播放格挡武器成功音效
            parryArmsSuccess.SetActive(true);//显示格挡武器成功动画组件
            parryArmSucAnim.SetTrigger("parrySuccess");//开始动画
            Invoke("HideParrySuccessAnim", 0.2f);
            Instantiate(parryArmsSparkEffect, parryArmsSuccess.transform.position, Quaternion.Euler(0, 0, 0));//显示格挡武器成功火花特效
            other.GetComponentInParent<BatBossCtrl>().ArmsBeParried();//武器受到格挡            
            //GameObject.FindGameObjectWithTag("playerV").transform.position += new Vector3(PlayerController.playerFaceRight ? -2 : 2, 0, 0);
            //调用玩家的完美格挡后退函数
            GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerController>().ParriedBack();
        }
    }

    void HideParrySuccessAnim()
    {
        parryArmsSuccess.SetActive(false);//隐藏格挡武器成功动画组件
    }
}
