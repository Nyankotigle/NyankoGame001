using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterUICtrl : MonoBehaviour
{
    [Header("teleporterBar")]
    public RectTransform UI_Element;
    [Header("Canvas")]
    public RectTransform CanvasRect;
    [Header("Teleporter")]
    public Transform teleporterTransform;
    [Header("mainCamera")]
    public Camera mainCamera;
    public float xOffset;
    public float yOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //将Teleporter的世界坐标转换为camera的视口坐标 Viewport Point（相机左下角为（0，0），右上角为（1，1））       
        Vector2 viewPortPos = mainCamera.WorldToViewportPoint(teleporterTransform.position);       
        Vector2 worldObjScreenPos = new Vector2((viewPortPos.x * CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x *0.5f)+xOffset,
            (viewPortPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f) + yOffset);
        UI_Element.anchoredPosition = worldObjScreenPos;
    }
}
