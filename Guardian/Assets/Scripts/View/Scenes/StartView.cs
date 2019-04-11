
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

    private GameObject _rootScene;

    
    void Start()
    {
        _scene = initScene("Module_08_BaseScene");
        _mainCamera.transform.position = new Vector3(10.8f, -8.5f, -85.0f);
        _ctrl = StartView_Ctrl._instance;
        setUICamera(gameObject, "_BaseView/Canvas");

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
        //_ctrl.onClickNewBtn();
        DestroyImmediate(_scene);
        _scene = null;
        _viewManager.swithView("LoginView");
    }

    public void onClickContinueBtn()
    {
        Debug.Log("click the ContinueBtn :" + this.GetType());
        _ctrl.onClickContinueBtn();
    }

    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);
    }

}
