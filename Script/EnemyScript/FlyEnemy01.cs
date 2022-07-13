using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承自普通敌人normalEnemy
public class FlyEnemy01 : normalEnemy
{
    [Header("移动速度")]
    public float speed;
    [Header("到达一个位置后停留时间")]
    public float waitTime;    
    [Header("下一个位置")]
    public Transform movePos;
    [Header("移动范围左下角")]
    public Transform leftDownPos;
    [Header("移动范围右上角")]
    public Transform rightUpPos;
    
    private Transform playerPos;//玩家位置

    private float nowWaitTime;//当前剩余等待时间

    // Start is called before the first frame update
    public void Start()
    {
        //调用父类的Start方法
        base.Start();

        nowWaitTime = waitTime;//初始化等待时间
        movePos.position = GetRandomPos();//随机获取下一个目标位置
        playerPos = GameObject.FindGameObjectWithTag("playerV").GetComponent<Transform>();//获取玩家位置
    }

    // Update is called once per frame
    public void Update()
    {
        //调用父类的Update方法
        base.Update();
        EnemyMove();
        EnemyFlip();
    }

    /// <summary>
    /// 敌人移动
    /// </summary>
    void EnemyMove()
    {
        //从当前位置向目标位置移动
        transform.position = Vector2.MoveTowards(transform.position, movePos.position, speed * Time.deltaTime);
        //到达目标位置后开始等待
        if (Vector2.Distance(transform.position, movePos.position) < 0.1f)
        {
            if (nowWaitTime <= 0)//等待时间结束后
            {
                movePos.position = GetRandomPos();//随机获取下一个目标位置
                nowWaitTime = waitTime;//重置等待时间
            }
            else
            {
                nowWaitTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 根据玩家左右位置翻转
    /// </summary>
    void EnemyFlip()
    {
        if(playerPos == null)//玩家死亡时跳过
        {
            return;
        }
        if (playerPos.position.x >= transform.position.x)//玩家在敌人右边
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else//玩家在敌人左边
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    /// <summary>
    /// 在移动范围内随机获取一个位置
    /// </summary>
    Vector2 GetRandomPos()
    {
        Vector2 randPos = new Vector2(Random.Range(leftDownPos.position.x, rightUpPos.position.x), 
            Random.Range(leftDownPos.position.y, rightUpPos.position.y));
        return randPos;
    }

    /// <summary>
    /// 受到格挡后退
    /// </summary>
    public override void BeParried() 
    {
        float newX;
        if (PlayerController.playerFaceRight)//向玩家面向的方向后退3.0
        {
            newX = transform.position.x + 3.0f;
        }
        else
        {
            newX = transform.position.x - 3.0f;
        }
        //如果后退后水平坐标越界了，则修正
        if (newX < leftDownPos.position.x)
        {
            newX = leftDownPos.position.x;
        }
        if (newX > rightUpPos.position.x)
        {
            newX = rightUpPos.position.x;
        }
        transform.position = new Vector2(newX, transform.position.y);
    }
}
