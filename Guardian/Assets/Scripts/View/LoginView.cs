
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


    //动作相关
    private Animation _curAnimation;
    private int _curIndex = 0;
    private int _maxIndex = 0;
    private float _passTime = 0.0f;
    private float _totalTime = 2.0f;

    private List<string> _nameList;

    private PlayerManager _playerManager;

    public AudioClip _backGroundAudioClip;

    private LoadingEndCallback _loadEndCallBack;
    public void Awake()
    {
        base.Awake();

        GameObject rawImage  = gameObject.transform.Find("Canvas/RawImage").gameObject;
        _fadeInOut = rawImage.GetComponent<FadeInOut>();

        _nameList = new List<string>();

        _playerManager = PlayerManager.getInstance();

    }


	// Use this for initialization
	void Start () {
        base.Start();
        _scene = initScene("Module_02_LevelOne");
     
        _mainCamera.transform.position = new Vector3(77.3f, -11.46f, -42.96f);
        _fadeInOut.FadeIn();

        StartCoroutine("initSwordsManPlayersMode");
        addBtnListener();

        _audioManager.playMusic(_backGroundAudioClip,true,0.5f);

        _loadEndCallBack = gotoNextView;
	}

    void gotoNextView()
    {
        _viewManager.swithView("LevelOneView");
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

        //设置动作相关信息
        setCurAnimationState(_swordsManObj);
    }

    public void setCurAnimationState(GameObject obj)
    {
        _curIndex = 0;
        _passTime = 0;
        _curAnimation = null;
        _nameList.Clear();
        Animation animation = obj.GetComponent<Animation>();
        _curAnimation = animation;
        _maxIndex = _curAnimation.GetClipCount();
        foreach (AnimationState state in _curAnimation)
        {
            _nameList.Add(state.name);
        }
    }

    override public void onClick(Button btn)
    {
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
        
        if (_magicianObj == null)
        {
            _magicianObj = initGameObject("Models/Magician/CreateMage", "CreateMage", _sceneRoleNode, new Vector3(76.9f, -13.62f, -50.4f));
        }
        if (_curAnimation == _magicianObj.GetComponent<Animation>()) return;
        _magicianObj.SetActive(true);
        magicIntroduce.enabled = true;
        _swordsManObj.SetActive(false);
        swordIntroduce.enabled = false;
        setCurAnimationState(_magicianObj);
        _audioManager.playSoundEffect("2_Roar_MagicHero",false,0.5f);

    }

    public void clickSwordBtn()
    {
        if (_curAnimation == _swordsManObj.GetComponent<Animation>()) return;
        if (_magicianObj!=null)
        {
            _magicianObj.SetActive(false);
            magicIntroduce.enabled = false;
        }
       
        _swordsManObj.SetActive(true);
        swordIntroduce.enabled = true;
        setCurAnimationState(_swordsManObj);
        _audioManager.playSoundEffect("1_LightSword_SwordHero");

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
        string name = inputField.text;
        _playerManager.setPlayerName(name);

        _viewManager.swithView("LoadingView", _loadEndCallBack);
    }


    void Update()
    {
        if (_curAnimation)
        {
            _passTime += Time.deltaTime;
            if (_passTime >= _totalTime)
            {

                _passTime = 0;
                _curIndex++;
                if (_curIndex >= _maxIndex)
                {
                    _curIndex = 0;
                }
                Debug.Log("_curIndex========" + _curIndex + "/" + _nameList[_curIndex]);
                _curAnimation.CrossFade(_nameList[_curIndex]);

            }
        }
       
    }


    void OnDestroy()
    {
        base.OnDestory();
        if (_scene != null)
        {
            Destroy(_scene);
            _scene = null;
        }


        if (_magicianObj != null)
        {
            Destroy(_magicianObj);
            _magicianObj = null;
        }

        if (_swordsManObj != null)
        {
            Destroy(_swordsManObj);
            _swordsManObj = null;
        }
            
        _audioManager.destoryClip(_backGroundAudioClip);
    }
	
}
