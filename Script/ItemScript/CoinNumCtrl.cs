using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinNumCtrl : MonoBehaviour
{
    [Header("初始金币数量")]
    public int startCoinNum;
    [Header("金币数量UI")]
    public Text coinNumText;

    public static int coinNumNow;


    // Start is called before the first frame update
    void Start()
    {
        coinNumNow = startCoinNum;

    }

    // Update is called once per frame
    void Update()
    {
        coinNumText.text = coinNumNow.ToString();
    }

}
