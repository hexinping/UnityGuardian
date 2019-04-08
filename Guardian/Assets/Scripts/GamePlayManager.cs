
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

    public void Awake()
    {
       
    }
	void Start () 
    {
        _viewManager = ViewManager.getInstance();
        _viewManager.showView("Prefabs/StartView", "StartView");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
