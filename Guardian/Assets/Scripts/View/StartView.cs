
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
using System;


public class StartView : BaseView
{


    private GameObject _scene;

    private GameObject _rootScene;

    private bool _isFadeOutEnd = false;

    private FadeInOut _fadeInOut;

    //private Action _loadEndCallBack;

    public AudioClip _backGroundAudioClip;

    private LoadingEndCallback _loadEndCallBack;

    public void Awake()
    {
        base.Awake();

        GameObject rawImage  = gameObject.transform.Find("Canvas/RawImage").gameObject;
        _fadeInOut = rawImage.GetComponent<FadeInOut>();

    }
    
    void Start()
    {
        base.Start();
        _scene = initScene("Module_08_BaseScene");
        _mainCamera.transform.position = new Vector3(10.8f, -8.5f, -85.0f);

        //粒子系统
        GameObject obj = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/ParticlePiao", _name, true);
        GameObject particleObj = GameObject.Instantiate(obj); 
        particleObj.transform.parent = _scene.transform;
        particleObj.name = name;
        particleObj.transform.localPosition = new Vector3(-13.0f,-5.0f,-43.0f);
        
        //监听场景中其他的声音 调整音频监听器的位置
        _audioManager.transform.localPosition = new Vector3(10.8f,-11.2f,-104.0f);
        _audioManager.playMusic(_backGroundAudioClip);

        this.Invoke("setTimeOut", 0.5f);

        _loadEndCallBack = gotoNextView;

    }

    void gotoNextView()
    {
        _viewManager.swithView("LoginView");  
    }

    public void setTimeOut()
    {
        _scene.SetActive(true);
        _fadeInOut.FadeIn();
    }

    void onFadeInEnd()
    {
       
    }

    void onFadeOutEnd()
    {
        DestroyImmediate(_scene);
        _scene = null;
        _viewManager.swithView("LoadingView", _loadEndCallBack);
    }

    public void onClickNewBtn()
    {
        Debug.Log("click the NewBtn :" + this.GetType());
        _fadeInOut.FadeOut(onFadeOutEnd);
    }

    public void onClickContinueBtn()
    {
        Debug.Log("click the ContinueBtn :" + this.GetType());
    }

    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
        {
            Destroy(_scene);
            _scene = null;
        }
            

        //清除委托
        _loadEndCallBack = null;
        _audioManager.destoryClip(_backGroundAudioClip);
    }


}
