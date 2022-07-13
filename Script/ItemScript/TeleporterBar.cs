using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleporterBar : MonoBehaviour
{
    public static int powerMax;//最大能量
    public static int powerNow;//当前能量

    private Image teleporterBar;

    // Start is called before the first frame update
    void Start()
    {
        teleporterBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        teleporterBar.fillAmount = (float)powerNow / (float)powerMax;//填充能量条
    }
}
