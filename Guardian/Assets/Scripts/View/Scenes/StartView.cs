
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
        GameObject obj = (GameObject)Resources.Load("Prefabs/Module_08_BaseScene");
        _scene = GameObject.Instantiate(obj);
        _scene.name = "Module_08_BaseScene";
        _rootScene = ViewManager.getInstance()._rootScene;
        _scene.transform.parent = _rootScene.transform;
        _scene.transform.localPosition = new Vector3(-1.54f,-13.22f,-107.0f);
       
        _ctrl = StartView_Ctrl._instance;

        _viewManager = ViewManager.getInstance();
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

        _viewManager.swithView("LoginView");
    }

    public void onClickContinueBtn()
    {
        Debug.Log("click the ContinueBtn :" + this.GetType());
        _ctrl.onClickContinueBtn();
    }

    void OnDestroy()
    {
        Destroy(_scene);
    }


}
