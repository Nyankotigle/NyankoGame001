using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCutDamage : MonoBehaviour
{
    [Header("蓄力攻击伤害值")]
    public int chargeCutDamage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //ChargeCut的碰撞体与普通敌人碰撞时
        if (other.gameObject.CompareTag("normalEnemy"))
        {
            AudioManger.Instance.PlayAudio("sword_hit_snow_2", transform.position);//播放砍到敌人音效
            other.GetComponent<normalEnemy>().TakeDamage(chargeCutDamage);//敌人掉血
        }
        //ChargeCut的碰撞体与batBoss的多边形碰撞体碰撞时
        if (other.gameObject.CompareTag("batBoss") &&
            other.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            AudioManger.Instance.PlayAudio("sword_hit_snow_2", transform.position);//播放砍到敌人音效
            other.GetComponent<BatBossCtrl>().TakeDamage(chargeCutDamage);//敌人掉血
        }
        //ChargeCut的碰撞体与Barrel的碰撞体碰撞时
        if (other.gameObject.CompareTag("barrel"))
        {
            other.GetComponent<BarrelCtrl>().BarrelExplosionAndBack();//barrel爆炸产生伤害并且回到初始位置
            //other.GetComponent<BarrelCtrl>().BarrelExplosionAndBack();//barrel爆炸产生伤害并且销毁
        }
    }
}
