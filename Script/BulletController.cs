using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("子弹速度")]
    public float speed;
    [Header("子弹伤害")]
    public int damage;
    [Header("子弹射程")]
    public float destroyDistance;

    private Rigidbody2D rb2D;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = transform.right * speed;//起始速度
        startPos = transform.position;//起始位置
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - startPos).sqrMagnitude;//当前飞行距离
        if (distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    //子弹碰到敌人时造成伤害
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("normalEnemy"))
        {
            other.GetComponent<normalEnemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
