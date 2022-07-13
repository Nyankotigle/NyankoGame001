using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxCtrl : MonoBehaviour
{
    [Header("玩家位置")]
    public Transform playerPos;

    private bool isAroundBox;//是否在箱子附近
    private Vector2 boxSize = new Vector2(1.9f, 1.9f);//box碰撞检测射线的大小
    //private Vector2 boxSize = new Vector2(2f, 2f);//刚好设置为2会误检测到碰撞
    private RaycastHit2D raycastHit2D;
    private int groundLayerMask;//Ground层的layermask值, 仅在该层上检测碰撞体

    // Start is called before the first frame update
    void Start()
    {
        isAroundBox = false;
        groundLayerMask = LayerMask.GetMask("Ground");//Ground层的layermask值, 仅在该层上检测碰撞体
    }

    // Update is called once per frame
    void Update()
    {
        //在箱子附近时，先根据玩家相对于箱子的位置判断箱子的预期移动方向，检测到对应的按键信号后向该方向发射box射线检测该方向是否有障碍物，没有则移动箱子
        if (isAroundBox)
        {
            if((playerPos.position.x > transform.position.x - 1) && (playerPos.position.x < transform.position.x + 1))
            {
                if(playerPos.position.y < transform.position.y)//从下方推
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        //从箱子的位置向当前移动方向发射长度为2.0的box射线
                        raycastHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.up, 2.0f, groundLayerMask);
                        //如果当前方向上没有碰撞体，移动箱子
                        if (raycastHit2D.collider == null)
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y + 2);
                            AudioManger.Instance.PlayAudio("推箱子声音", transform.position);//播放推箱子声音
                        }
                        else//否则在scene视图中画出碰撞位置
                        {
                            DrawBoxLine(raycastHit2D.point, boxSize, 3.0f);
                        }
                    }                    
                }
                else//从上方推
                {
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        raycastHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, 2.0f, groundLayerMask);
                        if (raycastHit2D.collider == null)
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y - 2);
                            AudioManger.Instance.PlayAudio("推箱子声音", transform.position);//播放推箱子声音
                        }
                        else
                        {
                            DrawBoxLine(raycastHit2D.point, boxSize, 3.0f);
                        }
                    }                   
                }
            }
            if ((playerPos.position.y > transform.position.y - 1) && (playerPos.position.y < transform.position.y + 1))
            {
                if (playerPos.position.x < transform.position.x)//从左边推
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        raycastHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.right, 2.0f, groundLayerMask);
                        if (raycastHit2D.collider == null)
                        {
                            transform.position = new Vector2(transform.position.x + 2, transform.position.y);
                            AudioManger.Instance.PlayAudio("推箱子声音", transform.position);//播放推箱子声音
                        }
                        else
                        {
                            DrawBoxLine(raycastHit2D.point, boxSize, 3.0f);
                        }
                    }                    
                }
                else//从右边推
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        raycastHit2D = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.left, 2.0f, groundLayerMask);
                        if (raycastHit2D.collider == null)
                        {
                            transform.position = new Vector2(transform.position.x - 2, transform.position.y);
                            AudioManger.Instance.PlayAudio("推箱子声音", transform.position);//播放推箱子声音
                        }
                        else
                        {
                            DrawBoxLine(raycastHit2D.point, boxSize, 3.0f);
                        }
                    }                    
                }
            }
        }
    }

    //当检测到碰撞时
    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundBox = true;//接触到告示牌
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundBox = false;
        }
    }


    /// <summary>
    /// 在盒子射线的碰撞点处显示盒子
    /// </summary>
    public void DrawBoxLine(Vector3 point, Vector2 size, float time = 0)
    {
        float x, y;
        x = point.x;
        y = point.y;
        float m, n;
        m = size.x;
        n = size.y;

        Vector3 point1, point2, point3, point4;
        point1 = new Vector3(x - m * 0.5f, y + n * 0.5f, 0);
        point2 = new Vector3(x + m * 0.5f, y + n * 0.5f, 0);
        point3 = new Vector3(x + m * 0.5f, y - n * 0.5f, 0);
        point4 = new Vector3(x - m * 0.5f, y - n * 0.5f, 0);

        Debug.DrawLine(point1, point2, Color.yellow, time);
        Debug.DrawLine(point2, point3, Color.yellow, time);
        Debug.DrawLine(point3, point4, Color.yellow, time);
        Debug.DrawLine(point4, point1, Color.yellow, time);
    }
}
