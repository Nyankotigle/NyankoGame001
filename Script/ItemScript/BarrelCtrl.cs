using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    [Header("barrel爆炸伤害")]
    public int barrelDamage;

    private CapsuleCollider2D explosionCollider;//爆炸碰撞体
    private Vector2 originPos;//barrel的初始位置
    private Animator barrelAnim;
    private PlayerHealth playerHealth;//玩家血量管理类

    // Start is called before the first frame update
    void Start()
    {
        explosionCollider = GetComponent<CapsuleCollider2D>();
        explosionCollider.enabled = false;
        playerHealth = GameObject.FindGameObjectWithTag("playerV").GetComponent<PlayerHealth>();
        barrelAnim = GetComponent<Animator>();
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BarrelExplosionAndDestroy()
    {
        explosionCollider.enabled = true;
        barrelAnim.SetBool("Explosion", true);
        CameraShake.camShake.Shake();//调用相机震动
        AudioManger.Instance.PlayAudio("爆炸音效", transform.position);//播放爆炸音效
        Invoke("BarrelDestory", 0.5f);
    }
    void BarrelDestory()
    {
        explosionCollider.enabled = false;
        Destroy(gameObject);
    }


    public void BarrelExplosionAndBack()
    {
        explosionCollider.enabled = true;
        barrelAnim.SetBool("Explosion", true);
        CameraShake.camShake.Shake();//调用相机震动
        AudioManger.Instance.PlayAudio("爆炸音效", transform.position);//播放爆炸音效
        Invoke("BarrelBack", 0.5f);
    }
    void BarrelBack()
    {
        barrelAnim.SetBool("Explosion", false);
        explosionCollider.enabled = false;
        transform.position = originPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //当碰撞的对象为player的胶囊碰撞体时
        if (other.gameObject.CompareTag("playerV") &&
            other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (playerHealth != null)//避免玩家死亡时出错
            {
                playerHealth.DamagePlayer(barrelDamage);//调用玩家伤害函数
            }
        }
    }
}
