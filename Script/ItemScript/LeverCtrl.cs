using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCtrl : MonoBehaviour
{
    [Header("挡杆的初始打开状态")]
    public bool leverIsOn;         //挡杆初始打开状态

    private Animator leverAnimator; //挡杆的动画控制器
    private bool isAroundLever;     //玩家是否在挡杆附近

    // Start is called before the first frame update
    void Start()
    {
        leverAnimator = GetComponent<Animator>();
        leverAnimator.SetBool("leverON", leverIsOn);        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isAroundLever)
        {
            leverIsOn = !leverIsOn;//状态反转
            AudioManger.Instance.PlayAudio("挡杆开关音效", transform.position);//播放挡杆开关音效
        }
        leverAnimator.SetBool("leverON", leverIsOn);//更新动画
    }

    //当检测到碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundLever = true;//玩家在挡杆附近
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundLever = false;//玩家离开挡杆
        }
    }
}
