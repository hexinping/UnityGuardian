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

public class PlayerInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


    public void clickHeadIcon()
    {
        Debug.Log(GetType() + "/clickHeadIcon===");
    }

    public void clickSettingBtn()
    {
        Debug.Log(GetType() + "/clickSettingBtn===");
    }

    public void clickExitBtn()
    {
        Debug.Log(GetType() + "/clickExitBtn===");
    }
	
}
