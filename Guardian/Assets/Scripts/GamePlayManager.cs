
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

public class GamePlayManager : MonoBehaviour {

	// Use this for initialization
    private ViewManager _viewManager;

    private static GamePlayManager _instance = null;

    public void Awake()
    {
        _instance = this;
    }
	void Start () 
    {
        _viewManager = ViewManager.getInstance();
        _viewManager.showView("StartView");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //test code

    public void showView1()
    {
        _viewManager.showView("View1");
    }

    public void showView2()
    {
        _viewManager.showView("View2");
    }

    public void showView3()
    {
        _viewManager.showView("View3");
    }


    public void popView()
    {
        _viewManager.popView();
    }

    public void switchView1()
    {
        _viewManager.swithView("View1");
    }

    public void switchView2()
    {
        _viewManager.swithView("View2");
    }

    public void switchView3()
    {
        _viewManager.swithView("View3");
    }


}
