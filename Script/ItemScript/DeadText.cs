using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadText : MonoBehaviour
{
    public Text deadText;

    // Start is called before the first frame update
    void Start()
    {
        deadText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        deadText.enabled = PlayerHealth.playerIsDead;//玩家为死亡状态时显示死字
    }
}
