
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

public class PlayerEvent : MonoBehaviour {

    public void PrintEvent(int i)
    {
        Debug.Log("PrintEvent: " + i + " called at: " + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount + " 当前时间:"+ Time.time);
    }
}
