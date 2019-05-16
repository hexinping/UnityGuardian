
/***
 *
 *  Title: "Guardian" 项目
 *         描述：
 *
 *  Description:
 *        功能：
 *       
 *
 *  Date: 2019
 * 
 *  Version: 1.0
 *
 *  Modify Recorder:
 *     
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.jianshu.com/p/9f0f90acc84f
public class ChangAn : MonoBehaviour {

    public float Ping;
    private bool IsStart = false;
    private float LastTime = 0;

    void Update()
    {
        if (IsStart && Ping > 0 && LastTime > 0 && Time.time - LastTime > Ping)
        {
            Debug.Log("ET长按触发=======");
            IsStart = false;
            LastTime = 0;
        }
    }
    public void LongPress(bool bStart)
    {
        IsStart = bStart;
        if (IsStart)
        {
            LastTime = Time.time;
        }
        else if (LastTime != 0)
        {
            LastTime = 0;
            Debug.Log("ET长按取消==========");
        }
    }
}
