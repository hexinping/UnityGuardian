
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

    private HeroAttackByET _instanceET;
	// Use this for initialization
	void Start () {
        _instanceET = HeroAttackByET.instance;
	}
	
	// Update is called once per frame


    public void clickAtkBtn()
    {
        Debug.Log(GetType() + "/clickAtkBtn===");
        _instanceET.responseNormalAttack();
    }

    public void clickMagicTrick1Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick1Btn===");
        _instanceET.responseMagicTrickA();
    }

    public void clickMagicTrick2Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick2Btn===");
        _instanceET.responseMagicTrickB();
    }

    public void clickMagicTrick3Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick3Btn===");
        _instanceET.responseMagicTrickC();
    }

    public void clickMagicTrick4Btn()
    {
        Debug.Log(GetType() + "/clickMagicTrick4Btn===");
        _instanceET.responseMagicTrickD();
    }
}
