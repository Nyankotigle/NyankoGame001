using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door01Ctrl : MonoBehaviour
{
    [Header("是否可以打开")]
    public bool canEnter;    
    [Header("玩家位置")]
    public Transform playerPos;
    
    public bool playerCrossDoor;    //玩家穿过门

    private bool openOnRight;       //从右边开门
    private bool door01IsOpen;      //打开状态

    private Animator door01Animator; //door01的动画控制器    
    private BoxCollider2D doorCollider;//碰撞体

    // Start is called before the first frame update
    void Start()
    {
        door01IsOpen = false;
        playerCrossDoor = false;
        door01Animator = GetComponent<Animator>();
        doorCollider = GetComponents<BoxCollider2D>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //当检测到碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (canEnter && !door01IsOpen)
            {
                door01Animator.SetBool("IsOpen", true);//开门动画
                AudioManger.Instance.PlayAudio("Door01开门声", transform.position);//播放Door01开门声

                doorCollider.enabled = false;//停用碰撞体
                door01IsOpen = true;//门已开
                //记录开门的位置
                if(playerPos.position.x > transform.position.x)
                {
                    openOnRight = true;//从右边开门
                }
                else
                {
                    openOnRight = false;//从左边开门
                }
            }
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {           
            //若离开时门为开启状态，则自动关闭
            if (door01IsOpen)
            {
                door01Animator.SetBool("IsOpen", false);//关门动画
                AudioManger.Instance.PlayAudio("Door01开门声", transform.position);//播放Door01开关门声

                doorCollider.enabled = true;
                door01IsOpen = false;
                //若离开的位置和开门的位置分别在门的两边，则玩家穿过了门
                if(playerPos.position.x > transform.position.x && !openOnRight ||
                    playerPos.position.x < transform.position.x && openOnRight)
                {
                    playerCrossDoor = true;
                    //Debug.Log("玩家穿过了门");
                }
            }
        }
    }
}
