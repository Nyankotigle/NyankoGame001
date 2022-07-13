using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 mousePos;//鼠标位置
    private float flashAngle;//手电筒的角度，0度水平正方向，90度垂直向上

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.HasFlashLight)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);

            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);//将鼠标位置转换为世界坐标
            Vector3 flashDir = (mousePos - transform.position).normalized;//求手电筒到鼠标方向的单位向量
            flashAngle = Mathf.Atan2(flashDir.y, flashDir.x) * Mathf.Rad2Deg;//转换为与X轴正向的夹角
                                                                             //transform.localRotation = Quaternion.Euler(0, 0, flashAngle);
            transform.eulerAngles = new Vector3(0, 0, flashAngle);//这种方式可以不受player左右翻转的影响
        }
        
    }
}
