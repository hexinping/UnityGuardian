
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

    private Dictionary<int, BaseEnitity> _enitityDic;

    // private float _interval = 0.033f;
    private MessageDispatcher _msgDispatcher;

    private GameObject _scene;

    private FadeInOut _fadeInOut;

    private GameObject _swordsManObj;

    private GameObject _easyTouchObj;

    private PlayerEnitity _playerEnitity;
  
    public void Awake()
    {
        base.Awake();

        _enitityDic = new Dictionary<int, BaseEnitity>();
        _msgDispatcher = MessageDispatcher.getInstance();

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


        PlayerEnitity enitity = new PlayerEnitity();
        _enitityDic.Add(enitity._id, enitity);
        _playerEnitity = enitity;
        enitity.setRootObj(_sceneRoleNode);

        //初始化显示对象
        enitity.initGameObject();

        //挂载脚本
        _swordsManObj = enitity._gameObject;
        HeroMovingByET _moveET = _swordsManObj.AddComponent<HeroMovingByET>();
        _moveET.setPlayerEnitity(enitity);

        HeroMovingByKey moveKey = _swordsManObj.AddComponent<HeroMovingByKey>();
        moveKey.setPlayerEnitity(enitity);

        HeroAttack attack = _swordsManObj.AddComponent<HeroAttack>();
        attack.setPlayerEnitity(enitity);

        HeroAttackByKey attackKey = _swordsManObj.AddComponent<HeroAttackByKey>();
        attackKey.setPlayerEnitity(enitity);

         //添加摄像机跟谁脚本
        _mainCamera.AddComponent<CameraFollow>();

        CameraFollow _cameFollow = _mainCamera.GetComponent<CameraFollow>();
        _cameFollow.setHeight(5.0f);
        _cameFollow.setDistance(10.0f);

        yield return null;

    }


    public PlayerEnitity getPlayer()
    {
        if (_playerEnitity != null)
            return _playerEnitity;
        return null;
    }

    void FixedUpdate()
    {
        //使用逻辑时间
        GlobalParams.frameCount++;
        GlobalParams.totalTime += Time.deltaTime;
        {
            foreach (KeyValuePair<int, BaseEnitity> obj in _enitityDic)
            {

                int id = obj.Key;
                BaseEnitity enitity = obj.Value;
                enitity._stateMachine.update(GlobalParams.interval);
            }

            //消息发送
            _msgDispatcher.dispatchDelayedMessages();

            //事件调用
            GlobalParams.update(GlobalParams.totalTime);

        }

    }
  
    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);
    }
	
}
