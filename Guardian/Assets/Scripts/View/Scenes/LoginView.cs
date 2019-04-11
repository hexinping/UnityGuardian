
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

    //private GameObject _rootScene;

	// Use this for initialization
	void Start () {
        _scene = initScene("Module_02_LevelOne");
        _mainCamera.transform.position = new Vector3(77.3f, -10.7f, -42.6f);

        setUICamera(gameObject, "Canvas");
	}

    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);
    }
	
}
