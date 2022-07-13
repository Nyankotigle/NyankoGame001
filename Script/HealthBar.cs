using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static int healthMax;//最大血量
    public static int healthNow;//当前血量

    public Text HealthText;

    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)healthNow / (float)healthMax;//填充血条
        HealthText.text = healthNow.ToString();//显示血量值
    }
}
