﻿
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

public class StartView_Ctrl : MonoBehaviour
{
	// Use this for initialization
    public void onClickNewBtn()
    {
        Debug.Log("click the NewBtn :" + this.GetType());
    }

    public void onClickContinueBtn()
    {
        Debug.Log("click the ContinueBtn :" + this.GetType());
    }
}
