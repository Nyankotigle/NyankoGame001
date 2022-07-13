using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarNumCtrl : MonoBehaviour
{
    [Header("初始星星数量")]
    public int startStarNum;
    [Header("星星数量UI")]
    public Text starNumText;

    public static int starNumNow;


    // Start is called before the first frame update
    void Start()
    {
        starNumNow = startStarNum;

    }

    // Update is called once per frame
    void Update()
    {
        starNumText.text = starNumNow.ToString();
    }

}
