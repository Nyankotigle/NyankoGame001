using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap03Ctrl : MonoBehaviour
{
  
    [Header("Trap03每个状态持续时间")]
    public float keepTime;
    [Header("Trap03各状态子物体")]
    public GameObject length1Obj;
    public GameObject length2Obj;
    public GameObject length3Obj;

    private int length;//Trap03长度状态
    private Animator trap03Animator;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        trap03Animator = GetComponent<Animator>();

        length1Obj.SetActive(true);
        length2Obj.SetActive(false);
        length3Obj.SetActive(false);
        length = 1;
        trap03Animator.SetInteger("length", length);
              
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > keepTime && length == 1)
        {
            length1Obj.SetActive(false);
            length2Obj.SetActive(true);
            length3Obj.SetActive(false);
            length = 2;
            trap03Animator.SetInteger("length", length);
            time = 0;
        }
        if (time > keepTime && length == 2)
        {
            length1Obj.SetActive(false);
            length2Obj.SetActive(false);
            length3Obj.SetActive(true);
            length = 3;
            trap03Animator.SetInteger("length", length);
            time = 0;
        }
        if (time > keepTime && length == 3)
        {
            length1Obj.SetActive(true);
            length2Obj.SetActive(false);
            length3Obj.SetActive(false);
            length = 1;
            trap03Animator.SetInteger("length", length);
            time = 0;
        }
    }

}
