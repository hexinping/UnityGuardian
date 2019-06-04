
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
    private GameObject _sceneLeft;
    private GameObject _sceneRight;

    private FadeInOut _fadeInOut;

    private GameObject _swordsManObj;

    private GameObject _easyTouchObj;

    private PlayerEnitity _playerEnitity;
    private PlayerInfo _playerInfo;
    private Transform _playerTransform;


    //敌人相关
    /*
        容器存放
     *  寻找一定范围内的敌人，寻找最近的敌人，面向敌人
     *  移动时按照条件寻找符合搜索条件的敌人，当停止移动时若存在符合条件的敌人就面向敌人
        
     */

    private List<BaseEnitity> _listEnimy;
    private List<BaseEnitity> _listPlayer;
    private int _updateEnimyIndex = 0;
  
    //音乐
    public AudioClip _backGroundAudioClip;
    
    public void Awake()
    {
        base.Awake();

        _enitityDic = new Dictionary<int, BaseEnitity>();
        _listEnimy = new List<BaseEnitity>();
        _listPlayer = new List<BaseEnitity>();
        _msgDispatcher = MessageDispatcher.getInstance();

        GameObject rawImage  = gameObject.transform.Find("Canvas/RawImage").gameObject;
        _fadeInOut = rawImage.GetComponent<FadeInOut>();



    }


	// Use this for initialization
	void Start () {
        base.Start();
        _scene = initScene("Module_02_LevelOne");
        initScene("Module_03_LevelTwo");
        initScene("Module_01_LevelThree");
        //StartCoroutine("initOtherScene");
        _mainCamera.transform.position = new Vector3(76.9f, -8.8f, -41.3f);
        //_mainCamera.transform.eulerAngles = (new Vector3(10.9f, 180.0f, 0.0f));
        
        //_mainCamera.transform.rotation = new Quaternion(10.9f, -180.0f, 0.0f,1.0f);

        _fadeInOut.FadeIn();


        StartCoroutine("initSwordsManPlayersMode");

        _easyTouchObj = GameObject.Find("_Environment").transform.Find("EasyTouch").gameObject;
        _easyTouchObj.SetActive(true);

        _audioManager.playMusic(_backGroundAudioClip);

  
	}


    IEnumerator initOtherScene()
    {
        //yield return new WaitForSeconds(5.0f);
        string name = "Module_03_LevelTwo";
        string path = "Prefabs/" + name;
        ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
        yield return rr;
        _sceneRight = Instantiate(rr.asset) as GameObject;
        _sceneRight.name = name;
        _sceneRight.transform.parent = _sceneBgNode.transform;
        //_sceneRight.SetActive(false);

        name = "Module_01_LevelThree";
        path = "Prefabs/" + name;
        rr = Resources.LoadAsync<GameObject>(path);
        yield return rr;

        _sceneLeft = Instantiate(rr.asset) as GameObject;
        _sceneLeft.name = name;
        _sceneLeft.transform.parent = _sceneBgNode.transform;
        _sceneLeft.SetActive(false);

        //initScene("Module_03_LevelTwo");
        //initScene("Module_01_LevelThree");


    }

    IEnumerator initSwordsManPlayersMode()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerEnitity enitity = new PlayerEnitity();
        _listPlayer.Add(enitity);
        _enitityDic.Add(enitity._id, enitity);
        _playerEnitity = enitity;
        enitity.setRootObj(_sceneRoleNode);
        enitity.setRootView(this);

        //初始化显示对象
        enitity.initGameObject();

         _playerTransform = enitity._gameObject.transform;

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

        HeroAttackByET attackET = _swordsManObj.AddComponent<HeroAttackByET>();
        attackET.setPlayerEnitity(enitity);

         //添加摄像机跟谁脚本
        _mainCamera.AddComponent<CameraFollow>();

        CameraFollow _cameFollow = _mainCamera.GetComponent<CameraFollow>();
        _cameFollow.setHeight(5.0f);
        _cameFollow.setDistance(10.0f);


        GameObject playerInfoObj = initGameObject("Prefabs/View/PlayerInfo", "PlayerInfo", this.gameObject, new Vector3(0, 0, 0));
        setUICamera(playerInfoObj, "Canvas");

        _playerInfo = playerInfoObj.GetComponent<PlayerInfo>();
        _playerInfo.setPlayerEnitiy(enitity);

        StartCoroutine("createEnimys");

        
    }

    private void setRootObjViwePlayerGame(EnimyEnitity enitity)
    {
        enitity.setRootObj(_sceneRoleNode);
        enitity.setRootView(this);
        enitity.setPlayerEnitity(_playerEnitity);
        enitity.initGameObject();
        _listEnimy.Add(enitity);
        _enitityDic.Add(enitity._id, enitity);
    }
    IEnumerator createEnimys()
    {
        yield return new WaitForSeconds(2.0f);
        //EnimyEnitity warriorEnimy = new EnimyEnitity();
        //setRootObjViwePlayerGame(warriorEnimy);

        //WarriorPurpleEnimyEnitity warriorPurpleEnimy = new WarriorPurpleEnimyEnitity();
        //setRootObjViwePlayerGame(warriorPurpleEnimy);
        //warriorPurpleEnimy._gameObject.transform.localPosition = _playerEnitity._gameObject.transform.position + new Vector3(5.0f, 0.0f, 2.0f);

        //MageGreenEnimyEnitity mageGreenEnimy = new MageGreenEnimyEnitity();
        //setRootObjViwePlayerGame(mageGreenEnimy);
        //mageGreenEnimy._gameObject.transform.localPosition = new Vector3(106f, -13.5f, -42.4f);

        //MagePurpleEnimyEnitity magePurpleEnimy = new MagePurpleEnimyEnitity();
        //setRootObjViwePlayerGame(magePurpleEnimy);
        //magePurpleEnimy._gameObject.transform.localPosition = new Vector3(67f, -13.5f, -42.7f);

        //ArcherEnimyEnitity archerGreenEnimy = new ArcherEnimyEnitity();
        //setRootObjViwePlayerGame(archerGreenEnimy);
        //archerGreenEnimy._gameObject.transform.localPosition = new Vector3(91f, -13.5f, -41.6f);

        //ArcherEnimyEnitity archerGreenEnimy1 = new ArcherEnimyEnitity();
        //setRootObjViwePlayerGame(archerGreenEnimy1);
        //archerGreenEnimy1._gameObject.transform.localPosition = new Vector3(121f, -13.5f, -44f);


        //KingEnimyEnitity kingGreenEnimy = new KingEnimyEnitity();
        //setRootObjViwePlayerGame(kingGreenEnimy);
        //kingGreenEnimy._gameObject.transform.localPosition = new Vector3(126f, -13.5f, -44f);

        //KingEnimyEnitity kingGreenEnimy1 = new KingEnimyEnitity();
        //setRootObjViwePlayerGame(kingGreenEnimy1);
        //kingGreenEnimy1._gameObject.transform.localPosition = new Vector3(33f, -13.5f, -44f);

        //GruntEnimyEnitity gruntGreenEnimy = new GruntEnimyEnitity();
        //setRootObjViwePlayerGame(gruntGreenEnimy);
        //gruntGreenEnimy._gameObject.transform.localPosition = new Vector3(82f, -13.5f, -52f);


        //GruntEnimyEnitity gruntGreenEnimy1 = new GruntEnimyEnitity();
        //setRootObjViwePlayerGame(gruntGreenEnimy1);
        //gruntGreenEnimy1._gameObject.transform.localPosition = new Vector3(84f, -13.5f, -42f);


        BossEnimyEnitity boss = new BossEnimyEnitity();
        setRootObjViwePlayerGame(boss);
        boss._gameObject.transform.localPosition = new Vector3(83f, -13.5f, -45.4f);
        boss._gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    override public void updatePlayerInfo()
    {
        _playerInfo.updateHpSlider();
    }

    public PlayerEnitity getPlayer()
    {
        if (_playerEnitity != null)
            return _playerEnitity;
        return null;
    }

    void FixedUpdate()
    {
        if (_playerEnitity == null) return;
        //使用逻辑时间
        GlobalParams.frameCount++;
        GlobalParams.totalTime += Time.deltaTime;
        {

            
            _playerEnitity.findTarget(_listEnimy);
            enimyFindTarget();

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


    void updateEnimyState(int index, Vector3 playerPos)
    {
        BaseEnitity enimy = _listEnimy[index];
        
        if (!enimy.isDead)
        {

            if (enimy.updateTick <= GlobalParams.totalTime)
            {
                //开始刷新
                enimy.updateTick += enimy.updateTickInterval;
                enimy.findTarget(_listPlayer);
            }
            
        }
    }
    private void enimyFindTarget()
    {

        /*
            敌人的刷新采取每次刷固定个数(2)，间隔刷新
       
         */

        if (_playerEnitity.isDead) return;

        Vector3 playerPos = _playerTransform.position;
        if (_listEnimy != null && _listEnimy.Count > 0)
        {
            if (_updateEnimyIndex > _listEnimy.Count - 1)
            {
                _updateEnimyIndex = 0;
            }
            int index1 = _updateEnimyIndex;
            int index2 = _updateEnimyIndex + 1;
            updateEnimyState(index1, playerPos);
            if (index2 < _listEnimy.Count)
            {
                updateEnimyState(index2, playerPos);
            }
            _updateEnimyIndex += 2;

        }

  
    }

    private void playerFindTarget()
    {
        _playerEnitity.attackTarget = null;
        if (_listEnimy != null && _listEnimy.Count > 0)
        { 
            Vector3 playerPos = _playerTransform.position;
            float minDis = 1000.0f;
            for (int i = 0; i < _listEnimy.Count; i++ )
            {
                BaseEnitity enimy = _listEnimy[i];
                if (!enimy.isDead)
                {
                    float warningDis = _playerEnitity.getWarningDis();
                    GameObject obj = enimy._gameObject;
                    Vector3 enimyPos = obj.transform.position;
                    float dis = (playerPos - enimyPos).sqrMagnitude;  //距离的平方
                    if (dis <= warningDis)  //搜索范围
                    {
                        //找到搜索范围内最近的敌人
                        if (dis < minDis)
                        {
                            minDis = dis;
                            _playerEnitity.attackTarget = enimy;
                        }
                    }
                }
               
            }
        }
    } 
  
    void OnDestroy()
    {
        base.OnDestory();
        if (_scene)
            Destroy(_scene);
    }

    public void removeFromEnimyList(BaseEnitity tarEnimiy)
    {
        if (tarEnimiy != null)
        {
            if (_listEnimy.Contains(tarEnimiy))
            {
                _listEnimy.Remove(tarEnimiy);
            }

        }
    }
	
}
