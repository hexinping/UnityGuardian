
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

public class AttackCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame


    public void clickAtkBtn()
    {
        Debug.Log(GetType() + "/clickAtkBtn===");
    }

    public void clickMagicTrick1Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick1Btn===");
    }

    public void clickMagicTrick2Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick2Btn===");
    }

    public void clickMagicTrick3Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick3Btn===");
    }

    public void clickMagicTrick4Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick4Btn===");
    }
}
