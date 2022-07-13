using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImgCtrl : MonoBehaviour
{
    /*
    [Header("拼接图片1左边")]
    public Transform image1;
    [Header("拼接图片2下边")]
    public Transform image2;
    [Header("拼接图片3左下角")]
    public Transform image3;
    [Header("单张图片的宽度")]
    public float imgWidth;
    [Header("单张图片的高度")]
    public float imgHeight;
    */
    [Header("玩家Transform")]
    public Transform playerTrans;
    [Header("该层背景移动的速度")]
    public float moveSpeed;
    [Header("场景1的中心点")]
    public Transform scene1CenterPoint;

    private Vector3 lastPlayerPos;//玩家上一帧的位置
    //private Vector2 imgStartPos;//背景图片的起始位置
    private Vector3 backgroundImgOffset;//该层背景的偏移量，transfo中心与形心的偏移量

    // Start is called before the first frame update
    void Start()
    {
        lastPlayerPos = playerTrans.position;
        //imgStartPos = transform.position;
        backgroundImgOffset = new Vector3(12, -7, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraFollow.cameraRotate)
        {
            transform.position -= (playerTrans.position - lastPlayerPos) * moveSpeed;
            lastPlayerPos = playerTrans.position;//记录玩家该帧的位置            
        }
        else
        {
            //当镜头旋转时，该层背景随着镜头反向旋转，这样飞船窗户背景就随着飞船一起旋转了
            //同时根据玩家离场景中心的相对位置，设置背景离场景中心的相对位置，同时考虑背景的transform中心与形心不重合，所以加上偏移量
            //这样该层背景不会在相机视野露出边界的同时，也能根据玩家的移动做相应的视差偏移
            transform.position = scene1CenterPoint.position + 0.5f * (playerTrans.position - scene1CenterPoint.position) + backgroundImgOffset;
        }
        
        /*
        if(transform.position.x - imgStartPos.x >= 0)
        {
            image1.position = new Vector2(transform.position.x + imgWidth, image1.position.y);
            image3.position = new Vector2(transform.position.x + imgWidth, image3.position.y);
            imgStartPos.x = image1.position.x;
        }
        if (transform.position.x - imgStartPos.x <= -imgWidth)
        {
            image1.position = new Vector2(transform.position.x - imgWidth, image1.position.y);
            image3.position = new Vector2(transform.position.x - imgWidth, image3.position.y);
            imgStartPos.x = image1.position.x;
        }
        if (transform.position.y - imgStartPos.y >= 0)
        {
            image2.position = new Vector2(image2.position.x, transform.position.y + imgHeight);
            image3.position = new Vector2(image3.position.x, transform.position.y + imgHeight);
            imgStartPos.y = image2.position.y;
        }
        if (transform.position.y - imgStartPos.y <= -imgHeight)
        {
            image2.position = new Vector2(image2.position.x, transform.position.y - imgHeight);
            image3.position = new Vector2(image3.position.x, transform.position.y - imgHeight);
            imgStartPos.y = image2.position.y;
        }
        */
    }
}
