using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("宝箱中的物体")]
    public GameObject treasureInBox;

    public bool isOpened;//宝箱已打开

    private bool isAroundTreasureBox;    
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        //接近宝箱并且宝箱未打开的情况下按E
        if (Input.GetKeyDown(KeyCode.E) && isAroundTreasureBox && !isOpened)
        {
            anim.SetTrigger("openBox");
            isOpened = true;
            AudioManger.Instance.PlayAudio("开箱子音效_加快", transform.position);//播放开箱子音效
            Instantiate(treasureInBox, transform.position + Vector3.up * 1, Quaternion.identity);            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundTreasureBox = true;//可以打开宝箱
        }
    }
    //当结束碰撞时
    void OnTriggerExit2D(Collider2D other)
    {
        //如果碰撞对象的标签为playerV，并且是与胶囊碰撞体发生的碰撞时
        if (other.gameObject.CompareTag("playerV")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isAroundTreasureBox = true;//不能打开宝箱
        }
    }
}
