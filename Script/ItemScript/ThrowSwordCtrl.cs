using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordCtrl : MonoBehaviour
{
    [Header("回旋剑的飞行速度")]
    public float speed;
    [Header("回旋剑的旋转速度")]
    public float rotateSpeed;
    [Header("回旋剑的伤害")]
    public int damage;

    private Rigidbody2D tSwordRigb;
    private Transform playerTrans;
    private Vector2 startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        tSwordRigb = GetComponent<Rigidbody2D>();
        tSwordRigb.velocity = transform.right * speed;//给回旋剑加上初始速度
        startSpeed = tSwordRigb.velocity;//记录初始速度
        playerTrans = GameObject.FindGameObjectWithTag("playerV").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed);//每帧旋转rotateSpeed        
        tSwordRigb.velocity = tSwordRigb.velocity - startSpeed * Time.deltaTime * 3;//速度逐渐减少直到与初始速度相反，从而返回
        //当回旋剑的水平速度与初始速度反向时，即回旋剑返回时，让回旋剑的竖直坐标跟随玩家
        if(tSwordRigb.velocity.x * startSpeed.x <= 0)
        {
            float y = Mathf.Lerp(transform.position.y, playerTrans.position.y, 0.1f);
            transform.position = new Vector3(transform.position.x, y, 0);//将回旋见的y坐标设为其到玩家y坐标之间的插值
        }
        //若与玩家的水平距离小于1则销毁回旋剑
        if (Mathf.Abs(transform.position.x - playerTrans.position.x) < 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //剑的碰撞体与普通敌人碰撞时
        if (other.gameObject.CompareTag("normalEnemy"))
        {
            //CameraShake.camShake.Shake();//调用相机震动
            AudioManger.Instance.PlayAudio("sword_hit_snow_2", transform.position);//播放砍到敌人音效
            other.GetComponent<normalEnemy>().TakeDamage(damage);//敌人掉血
            TimePause.Instance.PauseTime(5);//时间停顿
        }
        //剑的碰撞体与batBoss的多边形碰撞体碰撞时
        if (other.gameObject.CompareTag("batBoss") &&
            other.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            //CameraShake.camShake.Shake();//调用相机震动
            AudioManger.Instance.PlayAudio("sword_hit_snow_2", transform.position);//播放砍到敌人音效
            other.GetComponent<BatBossCtrl>().TakeDamage(damage);//敌人掉血
            TimePause.Instance.PauseTime(5);//时间停顿
        }
        //剑的碰撞体与Barrel的碰撞体碰撞时
        if (other.gameObject.CompareTag("barrel"))
        {
            other.GetComponent<BarrelCtrl>().BarrelExplosionAndBack();//barrel爆炸产生伤害并且回到初始位置
            //other.GetComponent<BarrelCtrl>().BarrelExplosionAndBack();//barrel爆炸产生伤害并且销毁
        }
    }
}
