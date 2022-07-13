using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("平台移动速度")]
    public float speed;
    [Header("到达端点后等待的时间")]
    public float waitTime;
    [Header("移动端点")]
    public Transform[] movePos;

    private int i;//目标端点的序号
    private float nowWaitTime;//当前等待时间
    private Transform playerDefTransform;//玩家Transform的默认parent

    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        nowWaitTime = waitTime;//初始化等待时间
        playerDefTransform = GameObject.FindGameObjectWithTag("playerV").transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //向目标端点移动
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        //到达端点后
        if (Vector2.Distance(transform.position, movePos[i].position) < 0.1f)
        {
            if(nowWaitTime < 0)//等待完成后
            {
                i = (i+1)%2;//更换目标端点
                nowWaitTime = waitTime;//重置等待时间
            }
            else//开始等待的倒计时
            {
                nowWaitTime -= Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与box碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = gameObject.transform;//玩家脚部与平台接触时，将平台作为parent，随平台移动
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与box碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            other.gameObject.transform.parent = playerDefTransform;//离开平台时恢复玩家默认的parent
        }
    }
}
