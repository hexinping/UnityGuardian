
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

public class LoginView : BaseView {


    private GameObject _scene;

    private GameObject _rootScene;

	// Use this for initialization
	void Start () {
        GameObject obj = (GameObject)Resources.Load("Prefabs/Module_02_LevelOne");
        _scene = GameObject.Instantiate(obj);
        _scene.name = "Module_02_LevelOne";
        _rootScene = ViewManager.getInstance()._rootScene;
        _scene.transform.parent = _rootScene.transform;
        //_scene.transform.localPosition = new Vector3(-5.3f, -9.0f, -117.0f);

        _viewManager = ViewManager.getInstance();

        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _mainCamera.transform.position = new Vector3(77.3f, -10.7f, -42.6f);
	}

    void OnDestroy()
    {
        Destroy(_scene);
    }
	
}
