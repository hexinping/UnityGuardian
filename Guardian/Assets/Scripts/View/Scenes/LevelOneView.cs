
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

public class LevelOneView : BaseView {


    private GameObject _scene;

    private FadeInOut _fadeInOut;

    private GameObject _swordsManObj;

    private GameObject _easyTouchObj;
  
    public void Awake()
    {
        base.Awake();

        GameObject rawImage  = gameObject.transform.Find("Canvas/RawImage").gameObject;
        _fadeInOut = rawImage.GetComponent<FadeInOut>();

    }


	// Use this for initialization
	void Start () {
        base.Start();
        _scene = initScene("Module_02_LevelOne");
     
        _mainCamera.transform.position = new Vector3(76.9f, -8.8f, -41.3f);
        //_mainCamera.transform.eulerAngles = (new Vector3(10.9f, 180.0f, 0.0f));
        
        //_mainCamera.transform.rotation = new Quaternion(10.9f, -180.0f, 0.0f,1.0f);

        _fadeInOut.FadeIn();

        StartCoroutine("initSwordsManPlayersMode");

        _easyTouchObj = GameObject.Find("_Environment").transform.Find("EasyTouch").gameObject;
        _easyTouchObj.SetActive(true);
        
	}
    IEnumerator initSwordsManPlayersMode()
    {

        string path = "Models/SwordsMan/GreateWarrior";
        ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
        yield return rr;
        _swordsManObj = Instantiate(rr.asset) as GameObject;
        _swordsManObj.name = "GreateWarrior";
        _swordsManObj.transform.parent = _sceneRoleNode.transform;
        _swordsManObj.transform.localScale = new Vector3(30.0f, 30.0f, 30.0f);
        _swordsManObj.transform.localPosition = new Vector3(76.9f, -13.02f, -48.27f);
        _swordsManObj.SetActive(true);

        //挂载脚本
        _swordsManObj.AddComponent<HeroMovingByET>();

         //添加摄像机跟谁脚本
        _mainCamera.AddComponent<CameraFollow>();

        CameraFollow _cameFollow = _mainCamera.GetComponent<CameraFollow>();
        _cameFollow.setHeight(5.0f);
        _cameFollow.setDistance(10.0f);

    }
  
    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);


    }
	
}
