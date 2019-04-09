
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

public class StartView : BaseView
{


    private StartView_Ctrl _ctrl;
    private GameObject _scene;

    void Start()
    {
        _scene = GameObject.Find("_Manager/_ViewManager/_Scene/Module_08_BaseScene");
        _ctrl = StartView_Ctrl._instance;
        this.Invoke("setTimeOut", 0.5f);
    }

    public void setTimeOut()
    {
        _scene.SetActive(true);
        FadeInOut._instance.FadeIn();
    }

    public void onClickNewBtn()
    {
        Debug.Log("click the NewBtn :"+ this.GetType());
        _ctrl.onClickNewBtn();
    }

    public void onClickContinueBtn()
    {
        Debug.Log("click the ContinueBtn :" + this.GetType());
        _ctrl.onClickContinueBtn();
    }


}
