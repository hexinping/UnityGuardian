
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

    private FadeInOut _fadeInOut;

    //组件
    public RawImage magicIntroduce;
    public RawImage swordIntroduce;
    public InputField inputField;

    private GameObject _magicianObj;
    private GameObject _swordsManObj;

    

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
     
        _mainCamera.transform.position = new Vector3(77.3f, -11.46f, -42.96f);
        _fadeInOut.FadeIn();

        this.Invoke("setTimeOut", 0.5f);
        
	}

    public void setTimeOut()
    {
        addBtnListener();
        initPlayersModes(); ;
    }



    public void initPlayersModes()
    {

        _magicianObj = initGameObject("Models/Magician/CreateMage", "CreateMage", _sceneRoleNode, new Vector3(76.9f, -13.62f, -50.4f));
        _swordsManObj = initGameObject("Models/SwordsMan/GreateWarrior", "GreateWarrior", _sceneRoleNode, new Vector3(76.9f, -13.02f, -48.27f));
        _magicianObj.SetActive(false);
        _swordsManObj.SetActive(true);
        _swordsManObj.transform.localScale = new Vector3(30.0f, 30.0f,30.0f);
 
    }
    override public void onClick(Button btn)
    {
        Debug.Log("btn.name========" + btn.name);
        switch (btn.name)
        { 
            case "BtnMagic":
                clickMaginBtn();
                break;
            case "BtnSword":
                clickSwordBtn();
                break;
            case "BtnRandom":
                clickRandomBtn();
                break;
            case "BtnSubmit":
                clickSubmitBtn();
                break;

        }
    }

    public void clickMaginBtn()
    {
        _magicianObj.SetActive(true);
        magicIntroduce.enabled = true;
        _swordsManObj.SetActive(false);
        swordIntroduce.enabled = false;
       
    }

    public void clickSwordBtn()
    {
        _magicianObj.SetActive(false);
        magicIntroduce.enabled = false;
        _swordsManObj.SetActive(true);
        swordIntroduce.enabled = true;
    }


    public void clickRandomBtn()
    {
        bool isMagicShow = _magicianObj.activeSelf;
        bool isSwordsShow = _swordsManObj.activeSelf;
        if (isMagicShow)
        {
            clickSwordBtn();
        }
        else if(isSwordsShow)
        {
            clickMaginBtn();
        }
    }

    public void clickSubmitBtn()
    {

    }





    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);
    }
	
}
