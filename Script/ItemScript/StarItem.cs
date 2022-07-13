using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarItem : MonoBehaviour
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
        //玩家的胶囊碰撞体碰到星星
        if (other.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            StarNumCtrl.starNumNow += 1;
            AudioManger.Instance.PlayAudio("捡星星音效", transform.position);//播放捡星星音效
            Destroy(gameObject);
        }
    }
}
