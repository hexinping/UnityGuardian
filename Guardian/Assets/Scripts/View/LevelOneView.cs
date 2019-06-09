
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
using kernal;

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

    //npc 生成点
    SpawnPos spawnPosA;
    SpawnPos spawnPosB;
    SpawnPos spawnPosC;

    //分区域存储npc 动态手工剔除用
    public List<EnimyEnitity> listA;
    public List<EnimyEnitity> listB;
    public List<EnimyEnitity> listC;

    private Vector3 _bossPos;


    new public void Awake()
    {
        base.Awake();

        _enitityDic = new Dictionary<int, BaseEnitity>();
        _listEnimy = new List<BaseEnitity>();
        _listPlayer = new List<BaseEnitity>();
        listA = new List<EnimyEnitity>();
        listB = new List<EnimyEnitity>();
        listC = new List<EnimyEnitity>();
        _msgDispatcher = MessageDispatcher.getInstance();

        GameObject rawImage  = gameObject.transform.Find("Canvas/RawImage").gameObject;
        _fadeInOut = rawImage.GetComponent<FadeInOut>();



    }


	// Use this for initialization
    new void Start()
    {
        base.Start();
        _scene = _sceneBgNode.transform.Find("Module_02_LevelOne").gameObject;
        _scene.SetActive(true);
        spawnPosB = _scene.transform.Find("SpawnArea").gameObject.GetComponent<SpawnPos>();


        _scene = _sceneBgNode.transform.Find("Module_03_LevelTwo").gameObject;
        _scene.SetActive(true);
        spawnPosC = _scene.transform.Find("SpawnArea").gameObject.GetComponent<SpawnPos>();

        _scene = _sceneBgNode.transform.Find("Module_01_LevelThree").gameObject;
        _scene.SetActive(true);
        spawnPosA = _scene.transform.Find("SpawnArea").gameObject.GetComponent<SpawnPos>();
        
        _mainCamera.transform.position = new Vector3(76.9f, -8.8f, -41.3f);
        //_mainCamera.transform.eulerAngles = (new Vector3(10.9f, 180.0f, 0.0f));
        
        //_mainCamera.transform.rotation = new Quaternion(10.9f, -180.0f, 0.0f,1.0f);

        _fadeInOut.FadeIn();


        StartCoroutine("initSwordsManPlayersMode");

        _easyTouchObj = GameObject.Find("_Environment").transform.Find("EasyTouch").gameObject;
        enableEasyTouch();

        _audioManager.playMusic(_backGroundAudioClip);

	}

    public void enableEasyTouch()
    {
        _easyTouchObj.SetActive(true);
    }

    public void disableEasyTouch()
    {
        _easyTouchObj.SetActive(false);
    }

    IEnumerator initSwordsManPlayersMode()
    {
        yield return new WaitForEndOfFrame();

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

        GameObject playerInfoObj = initGameObject("Prefabs/View/PlayerInfo", "PlayerInfo", this.gameObject, new Vector3(0, 0, 0));
        setUICamera(playerInfoObj, "Canvas");

        _playerInfo = playerInfoObj.GetComponent<PlayerInfo>();
        _playerInfo.setPlayerEnitiy(enitity);

        StartCoroutine("createEnimys");

        //默认开启后期
        enableBloom();
        
    }

    public void onPlayerDead()
    {
        disableEasyTouch();
        hidePlayInfo();
        _playerEnitity = null;

        for (int i = 0; i < _listEnimy.Count; i++)
        {
            EnimyEnitity e = (EnimyEnitity)_listEnimy[i];
            if (!e.isDead)
            {
                e.changeStateByIndex(EnimyStateEnum.IDLE, true, 1.0f, true);
            }
        }
    }

    public void hidePlayInfo()
    {
        _playerInfo.gameObject.SetActive(false);
    }

    public void enableRadialBlur()
    {
        RadialBlurEffect radialBlurEffect = _mainCamera.GetComponent<RadialBlurEffect>();
        radialBlurEffect.enabled = true;
    }

    public void disableRadialBlur()
    {
        RadialBlurEffect radialBlurEffect = _mainCamera.GetComponent<RadialBlurEffect>();
        radialBlurEffect.enabled = false;
    }
    public void enableBloom()
    {
        PostEffectBloom bloom = _mainCamera.GetComponent<PostEffectBloom>();
        bloom.enabled = true;
    }

    public void disableBloom()
    {
        PostEffectBloom bloom = _mainCamera.GetComponent<PostEffectBloom>();
        bloom.enabled = false;
    }

    private void setRootObjViwePlayerGame(EnimyEnitity enitity, int AreaIndex = 0)
    {
        enitity.setRootObj(_sceneRoleNode);
        enitity.setRootView(this);
        enitity.setPlayerEnitity(_playerEnitity);
        enitity.initGameObject();
        _listEnimy.Add(enitity);
        _enitityDic.Add(enitity._id, enitity);
        if (AreaIndex == 1)
        {
            listA.Add(enitity);
        }
        else if (AreaIndex == 2)
        {
            listB.Add(enitity);
        }
        else if (AreaIndex == 3)
        {
            listC.Add(enitity);
        }
    }
    IEnumerator createEnimys()
    {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine("createAreaBEnimy");

    }
    IEnumerator createAreaBEnimy()
    {
        //B区域NPC
        Transform[] warriorPosArry = spawnPosB.warrorSpawnPos;
        Transform[] magePosArry = spawnPosB.mageSpawnPos;
        Transform[] gruntPosArry = spawnPosB.gruntSpawnPos;
        Transform[] archerPosArry = spawnPosB.archerSpawnPos;
        Transform[] kingPosArry = spawnPosB.kingSpawnPos;
        Transform[] bossPosArry = spawnPosB.bossSpawnPos;

        int areaIndex = 2;
        EnimyEnitity warriorEnimy = new EnimyEnitity();
        setRootObjViwePlayerGame(warriorEnimy, areaIndex);
        warriorEnimy._gameObject.transform.position = warriorPosArry[0].position;

        WarriorPurpleEnimyEnitity warriorPurpleEnimy = new WarriorPurpleEnimyEnitity();
        setRootObjViwePlayerGame(warriorPurpleEnimy, areaIndex);
        warriorPurpleEnimy._gameObject.transform.position = warriorPosArry[1].position;

        MageGreenEnimyEnitity mageGreenEnimy = new MageGreenEnimyEnitity();
        setRootObjViwePlayerGame(mageGreenEnimy, areaIndex);
        mageGreenEnimy._gameObject.transform.position = magePosArry[0].position;

        MagePurpleEnimyEnitity magePurpleEnimy = new MagePurpleEnimyEnitity();
        setRootObjViwePlayerGame(magePurpleEnimy, areaIndex);
        magePurpleEnimy._gameObject.transform.position = magePosArry[1].position;

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < gruntPosArry.Length; i++)
        {
            GruntEnimyEnitity gruntGreenEnimy = new GruntEnimyEnitity();
            setRootObjViwePlayerGame(gruntGreenEnimy, areaIndex);
            gruntGreenEnimy._gameObject.transform.position = gruntPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < archerPosArry.Length; i++)
        {
            ArcherEnimyEnitity archerGreenEnimy = new ArcherEnimyEnitity();
            setRootObjViwePlayerGame(archerGreenEnimy, areaIndex);
            archerGreenEnimy._gameObject.transform.position = archerPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < kingPosArry.Length; i++)
        {
            KingEnimyEnitity kingGreenEnimy = new KingEnimyEnitity();
            setRootObjViwePlayerGame(kingGreenEnimy, areaIndex);
            kingGreenEnimy._gameObject.transform.position = kingPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine("createAreaAEnimy");
       

    }
    IEnumerator createAreaAEnimy()
    {
        //A区域
        Transform[] warriorPosArry = spawnPosA.warrorSpawnPos;
        Transform[] magePosArry = spawnPosA.mageSpawnPos;
        Transform[] gruntPosArry = spawnPosA.gruntSpawnPos;
        Transform[] archerPosArry = spawnPosA.archerSpawnPos;
        Transform[] kingPosArry = spawnPosA.kingSpawnPos;
        Transform[] bossPosArry = spawnPosA.bossSpawnPos;

        int areaIndex = 1;
        WarriorPurpleEnimyEnitity warriorPurpleEnimyEnitity = new WarriorPurpleEnimyEnitity();
        setRootObjViwePlayerGame(warriorPurpleEnimyEnitity, areaIndex);
        warriorPurpleEnimyEnitity._gameObject.transform.position = warriorPosArry[0].position;

        WarriorTealEnimyEnitity warriorTealEnimyEnitity = new WarriorTealEnimyEnitity();
        setRootObjViwePlayerGame(warriorTealEnimyEnitity, areaIndex);
        warriorTealEnimyEnitity._gameObject.transform.position = warriorPosArry[1].position;

        WarriorRedEnimyEnitity warriorRedEnimyEnitity2 = new WarriorRedEnimyEnitity();
        setRootObjViwePlayerGame(warriorRedEnimyEnitity2, areaIndex);
        warriorRedEnimyEnitity2._gameObject.transform.position = warriorPosArry[2].position;

        WarriorYellowEnimyEnitity warriorYellowEnimyEnitity = new WarriorYellowEnimyEnitity();
        setRootObjViwePlayerGame(warriorYellowEnimyEnitity, areaIndex);
        warriorYellowEnimyEnitity._gameObject.transform.position = warriorPosArry[3].position;

        yield return new WaitForSeconds(1.0f);

        MageGreenEnimyEnitity mageGreenEnimyA = new MageGreenEnimyEnitity();
        setRootObjViwePlayerGame(mageGreenEnimyA, areaIndex);
        mageGreenEnimyA._gameObject.transform.position = magePosArry[0].position;

        MagePurpleEnimyEnitity magePurpleEnimyA = new MagePurpleEnimyEnitity();
        setRootObjViwePlayerGame(magePurpleEnimyA, areaIndex);
        magePurpleEnimyA._gameObject.transform.position = magePosArry[1].position;

        MageGreenEnimyEnitity mageGreenEnimyA1 = new MageGreenEnimyEnitity();
        setRootObjViwePlayerGame(mageGreenEnimyA1, areaIndex);
        mageGreenEnimyA1._gameObject.transform.position = magePosArry[2].position;

        MagePurpleEnimyEnitity magePurpleEnimyA2 = new MagePurpleEnimyEnitity();
        setRootObjViwePlayerGame(magePurpleEnimyA2, areaIndex);
        magePurpleEnimyA2._gameObject.transform.position = magePosArry[3].position;

        MagePurpleEnimyEnitity magePurpleEnimyA3 = new MagePurpleEnimyEnitity();
        setRootObjViwePlayerGame(magePurpleEnimyA3, areaIndex);
        magePurpleEnimyA3._gameObject.transform.position = magePosArry[4].position;

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < gruntPosArry.Length; i++)
        {
            GruntEnimyEnitity gruntGreenEnimyA = new GruntEnimyEnitity();
            setRootObjViwePlayerGame(gruntGreenEnimyA, areaIndex);
            gruntGreenEnimyA._gameObject.transform.position = gruntPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < archerPosArry.Length; i++)
        {
            ArcherEnimyEnitity archerGreenEnimyA = new ArcherEnimyEnitity();
            setRootObjViwePlayerGame(archerGreenEnimyA, areaIndex);
            archerGreenEnimyA._gameObject.transform.position = archerPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < kingPosArry.Length; i++)
        {
            KingEnimyEnitity kingGreenEnimyA = new KingEnimyEnitity();
            setRootObjViwePlayerGame(kingGreenEnimyA, areaIndex);
            kingGreenEnimyA._gameObject.transform.position = kingPosArry[i].position;
        }
     
        //boss
        _bossPos = bossPosArry[0].position;
        createBoss();

        yield return new WaitForSeconds(1.0f);
        StartCoroutine("createAreaCEnimy");
        
    }

    private void createBoss()
    {
        BossEnimyEnitity boss = new BossEnimyEnitity();
        StartCoroutine(initBoss(boss));
    }

    IEnumerator initBoss(BossEnimyEnitity boss)
    {
        yield return null;
        string path = boss._mode.file;
        ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
        yield return rr;
        setRootObjViwePlayerGame(boss, 1);
        GameObject obj = GameObject.Instantiate(rr.asset) as GameObject;
        obj.name = boss.objName;
        obj.transform.parent = boss._rootObj.transform;
        obj.transform.position = _bossPos;

        boss._gameObject = obj;
        boss.setBattleDatas();
    }

     IEnumerator createAreaCEnimy()
    {
        //C区域
        Transform[] warriorPosArry = spawnPosC.warrorSpawnPos;
        Transform[] magePosArry = spawnPosC.mageSpawnPos;
        Transform[] gruntPosArry = spawnPosC.gruntSpawnPos;
        Transform[] archerPosArry = spawnPosC.archerSpawnPos;
        Transform[] kingPosArry = spawnPosC.kingSpawnPos;
        Transform[] bossPosArry = spawnPosC.bossSpawnPos;

        int areaIndex = 3;
        WarriorRedEnimyEnitity warriorRedEnimyEnitity = new WarriorRedEnimyEnitity();
        setRootObjViwePlayerGame(warriorRedEnimyEnitity, areaIndex);
        warriorRedEnimyEnitity._gameObject.transform.position = warriorPosArry[0].position;

        WarriorTealEnimyEnitity warriorTealEnimyEnitity = new WarriorTealEnimyEnitity();
        setRootObjViwePlayerGame(warriorTealEnimyEnitity, areaIndex);
        warriorTealEnimyEnitity._gameObject.transform.position = warriorPosArry[1].position;

        MageGreenEnimyEnitity mageGreenEnimyC = new MageGreenEnimyEnitity();
        setRootObjViwePlayerGame(mageGreenEnimyC, areaIndex);
        mageGreenEnimyC._gameObject.transform.position = magePosArry[0].position;

        MagePurpleEnimyEnitity magePurpleEnimyC = new MagePurpleEnimyEnitity();
        setRootObjViwePlayerGame(magePurpleEnimyC, areaIndex);
        magePurpleEnimyC._gameObject.transform.position = magePosArry[1].position;

        GruntEnimyEnitity gruntGreenEnimyC = new GruntEnimyEnitity();
        setRootObjViwePlayerGame(gruntGreenEnimyC, areaIndex);
        gruntGreenEnimyC._gameObject.transform.position = gruntPosArry[0].position;

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < archerPosArry.Length; i++)
        {
            ArcherEnimyEnitity archerGreenEnimyC = new ArcherEnimyEnitity();
            setRootObjViwePlayerGame(archerGreenEnimyC, areaIndex);
            archerGreenEnimyC._gameObject.transform.position = archerPosArry[i].position;
        }

        yield return new WaitForSeconds(1.0f);

        KingEnimyEnitity kingGreenEnimyC = new KingEnimyEnitity();
        setRootObjViwePlayerGame(kingGreenEnimyC, areaIndex);
        kingGreenEnimyC._gameObject.transform.position = kingPosArry[0].position;

        //默认隐藏 todo
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

            if (!_playerEnitity.isDead)
            {
                _playerEnitity.findTarget(_listEnimy);
            }
            
            enimyFindTarget();

            foreach (KeyValuePair<int, BaseEnitity> obj in _enitityDic)
            {
                int id = obj.Key;
                BaseEnitity enitity = obj.Value;
                if (!enitity.isDead)
                {
                    enitity._stateMachine.update(GlobalParams.interval);
                }
                
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
        _playerEnitity.targetTransform = null;
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
                            _playerEnitity.targetTransform = obj.transform;
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
