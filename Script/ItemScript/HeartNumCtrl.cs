using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartNumCtrl : MonoBehaviour
{
    [Header("初始爱心数量")]
    public int startHeartNum;
    [Header("爱心数量UI")]
    public Text heartNumText;

    public static int heartNumNow;


    // Start is called before the first frame update
    void Start()
    {
        heartNumNow = startHeartNum;

    }

    // Update is called once per frame
    void Update()
    {
        heartNumText.text = heartNumNow.ToString();
    }

}
