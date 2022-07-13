using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueBar : MonoBehaviour
{
    public static int blueMax;//最大蓝
    public static int blueNow;//当前蓝

    public Text BlueText;

    private Image blueBar;

    // Start is called before the first frame update
    void Start()
    {
        blueBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        blueBar.fillAmount = (float)blueNow / (float)blueMax;//填充蓝条
        BlueText.text = blueNow.ToString();//显示蓝值
    }
}
