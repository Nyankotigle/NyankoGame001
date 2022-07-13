using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackDir : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 mousePos;//鼠标位置
    private float swordAngle;//剑攻击的角度，0度水平正方向，90度垂直向上
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);//将鼠标位置转换为世界坐标
        Vector3 swordDir = (mousePos - transform.position).normalized;//求剑到鼠标方向的单位向量
        swordAngle = Mathf.Atan2(swordDir.y, Mathf.Abs(swordDir.x)) * Mathf.Rad2Deg;//转换为与X轴(不分正负)的夹角，范围[-90°,90°]
        transform.localRotation = Quaternion.Euler(0, 0, swordAngle);//这样剑不会攻击到背后
    }
}
