using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //玩家的胶囊碰撞体碰到硬币
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            CoinNumCtrl.coinNumNow += 1;
            AudioManger.Instance.PlayAudio("捡金币音效2", transform.position);//播放捡金币音效2
            Destroy(gameObject);
        }
    }
}
