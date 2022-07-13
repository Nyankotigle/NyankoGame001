using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePause : MonoBehaviour
{
    //声明一个私有的静态实例成员instance
    private static TimePause instance;
    //声明一个公有的静态属性来封装这个实例
    public static TimePause Instance
    {
        //get访问器读取字段
        get
        {
            if (instance == null)
            {
                //如果instance为空，从场景中找到TimePause的实例并赋值
                instance = Transform.FindObjectOfType<TimePause>();
            }
            return instance;
        }
        //set访问器给字段赋值
        //不需要从外部给instance赋值，所以省略set访问器
    }

    public void PauseTime(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);//真实时间不会受时间缩放的影响
        Time.timeScale = 1;
    }
}
