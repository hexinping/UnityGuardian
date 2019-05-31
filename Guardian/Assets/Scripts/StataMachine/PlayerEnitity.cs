using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kernal;

public class PlayerEnitity:BaseEnitity  {


    public PlayerEnitityMode _mode;
    
    private List<string> _animationNameList;
    private List<string> _comobAnimationNameList; //组合动作

    private Animation _animation;

    private int _nomarlAttackComobIndex = 0;  //组合动作的序号

    private Transform _playerTransform;

    //预加载prefab, 对象缓冲池使用
    private GameObject _prefabPlayerHp;
    private GameObject _prefabPlayerAttackLeft;
    private GameObject _prefabPlayerAttackMid;
    private GameObject _prefabPlayerAttackRight;
    private GameObject _prefabPlayerMagicA;
    private GameObject _prefabPlayerMagicB;
    private GameObject _prefabPlayerMagicC;
    private GameObject _prefabPlayerMagicD;

    private HpFollow _hpFollow;

    //动画帧事件集合
    private Dictionary<string, List<int>> _animationEventDict;

    //动画状态
    private Dictionary<string, AnimationState> _animationStateDict;

    public GameObject skillGround;
    public GameObject skillLayer;

    public PlayerEnitity()
    {
        _animationNameList = new List<string>();
        _comobAnimationNameList = new List<string>();
        _animationEventDict = new Dictionary<string, List<int>>();
        _animationStateDict = new Dictionary<string, AnimationState>();

        damageLabelOffsetY = 150.0f;

        skillGround = GameObject.Find("_Manager/_ViewManager/_Scene/SkillGround");
        skillLayer = GameObject.Find("_Manager/_ViewManager/_Scene/Skill");
        
    }

    override public void addBaseState()
    {
        BaseState playerIdleState = new PlayerIdleState(this);
        BaseState playerRunState = new PlayerRunState(this);
        BaseState playerAttackState = new PlayerAttackState(this);
        BaseState playerDeadState = new PlayerDeadState(this);
        BaseState playerMagicTrickAState = new PlayerMagicTrickAState(this);
        BaseState playerMagicTrickBState = new PlayerMagicTrickBState(this);
        BaseState playerMagicTrickCState = new PlayerMagicTrickCState(this);
        BaseState playerMagicTrickDState = new PlayerMagicTrickDState(this);

        _stateList.Add(playerIdleState);
        _stateList.Add(playerRunState);
        _stateList.Add(playerDeadState);
        _stateList.Add(playerMagicTrickAState);
        _stateList.Add(playerMagicTrickBState);
        _stateList.Add(playerMagicTrickCState);
        _stateList.Add(playerMagicTrickDState);
        _stateList.Add(playerAttackState);

        //状态机设置
        _stateMachine.setCurrentState(playerIdleState);
    }


    override public void initModeData()
    {
        _mode = new PlayerEnitityMode();
    }

    void initBufferPoolPrefab()
    {
        _prefabPlayerHp = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "Prefabs/View/Hp", rootView._name, true);
        _prefabPlayerAttackLeft = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_attack01", rootView._name, true);
        _prefabPlayerAttackMid = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_attack02", rootView._name, true);
        _prefabPlayerAttackRight = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_attack03", rootView._name, true);
        _prefabPlayerMagicA = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/bruceSkill", rootView._name, true);
        _prefabPlayerMagicB = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_Skill03", rootView._name, true);
        _prefabPlayerMagicC = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab,  "ParticleProps/Hero_Skill03", rootView._name, true);
        _prefabPlayerMagicD = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/groundBrokeRed", rootView._name, true);

    }

    public GameObject getAttackEffeectObj(Vector3 targetPos)
    {
        GameObject preObj = null;
        int comIndex = getCurrComIndex() + 1;
        if (comIndex == 1)
        { 
            preObj = _prefabPlayerAttackLeft;
        }
        else if(comIndex == 2)
        {
            preObj = _prefabPlayerAttackMid;
        }
        else if (comIndex == 3)
        {
            preObj = _prefabPlayerAttackRight;
        }
        GameObject obj = PoolManager.PoolsArray[GlobalParams.SkillPool].GetGameObjectByPool(_prefabPlayerAttackLeft,
               targetPos, Quaternion.identity);
        obj.transform.rotation = _playerTransform.rotation;
        return obj;
    }
    override public void initGameObject()
    {
        initBufferPoolPrefab();
        if (_rootObj)
        {
            _gameObject = getGameObject(_mode.file, "GreateWarrior", _rootObj, Vector3.zero);
            _gameObject.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);
            _gameObject.transform.localPosition = new Vector3(76.9f, -13.9f, -48.27f);

            _playerTransform = _gameObject.transform;

            //使用缓冲池床创建血条
            GameObject obj = PoolManager.PoolsArray[GlobalParams.HPPool].GetGameObjectByPool(_prefabPlayerHp,
                _gameObject.transform.position, Quaternion.identity);

            obj.name = "PlayerHp";
            HpFollow hpFollow = obj.GetComponent<HpFollow>();
            hpFollow.setHpUIDatas(new Vector2(0, 170), _mode.hp, _mode.maxHp);
            _hpFollow = hpFollow;

            //添加主角出场特效
            createEffectNoPool("ParticleProps/EnemySpawnEff", _gameObject.transform.position, skillGround);
        }

        addAinimainEvents();
        //动作添加
        if (_gameObject != null)
        {
            addAinimainClips();
            Animation animation = _gameObject.GetComponent<Animation>();
            if(animation == null)
            {
                _gameObject.AddComponent<Animation>();
                animation = _gameObject.GetComponent<Animation>();
            }

            _animation = animation;
            foreach(string clipName in _animationNameList)
            {
                string path = "Models/SwordsMan/SwordsManResources/Animations/StoneKing@" + clipName;
                AnimationClip clip = Resources.Load<AnimationClip>(path);
                _animation.AddClip(clip, clipName);
            }

            //组合动作
            foreach (string clipName in _comobAnimationNameList)
            {
                string path = "Models/SwordsMan/SwordsManResources/Animations/StoneKing@" + clipName;
                AnimationClip clip = Resources.Load<AnimationClip>(path);
                _animation.AddClip(clip, clipName);
            }
            saveAnimationState();
            _gameObject.AddComponent<PlayerEvent>(); //动画帧事件要放到一个与Animation同级的脚本里
            changeAniamtion(_animationNameList[0], 1.0f, true);

        }
    }


    void saveAnimationState()
    {
        foreach (AnimationState state in _animation)
        {
            _animationStateDict[state.name] = state;
        }
    }

    void addAinimainClips()
    {
        _animationNameList.Add("Idle");
        _animationNameList.Add("Run");
        _animationNameList.Add("Death");

        //技能动作
        _animationNameList.Add("Attack1");
        _animationNameList.Add("Attack2");
        _animationNameList.Add("Attack3"); 
        _animationNameList.Add("Attack4");

        //普通攻击组合动作
        _comobAnimationNameList.Add("Attack3-1");
        _comobAnimationNameList.Add("Attack3-2");
        _comobAnimationNameList.Add("Attack3-3");
    }

    void addAinimainEvents()
    {
        //技能帧事件
        List<int> attack1List = new List<int>();
        attack1List.Add(21);
        _animationEventDict["Attack1"] = attack1List;

        List<int> attack2List = new List<int>();
        attack2List.Add(27);
        _animationEventDict["Attack2"] = attack2List;

        List<int> attack3List = new List<int>();
        attack3List.Add(27);
        _animationEventDict["Attack3"] = attack3List;

        List<int> attack4List = new List<int>();
        attack4List.Add(40);
        _animationEventDict["Attack4"] = attack4List;

        //普通攻击帧事件
        List<int> Attack3_1List = new List<int>();
        Attack3_1List.Add(21);
        _animationEventDict["Attack3-1"] = Attack3_1List;

        List<int> Attack3_2List = new List<int>();
        Attack3_2List.Add(21);
        _animationEventDict["Attack3-2"] = Attack3_2List;

        List<int> Attack3_3List = new List<int>();
        Attack3_3List.Add(21);
        _animationEventDict["Attack3-3"] = Attack3_3List;
    }


    public void changeStateByIndex(PlayerStateEnum playerstateEm, float tSpeed = 1.0f, bool tIsLoop = false)
    {
        int stateIndex = (int)playerstateEm;
        BaseState state = _stateList[stateIndex];
        string name = getAnimationName(playerstateEm);

        bool isCheckSameState = true;
        if (playerstateEm == PlayerStateEnum.NORMALATTACK)
        {
            isCheckSameState = false;
            //Debug.Log("_nomarlAttackComobIndex===========" + _nomarlAttackComobIndex);
        }
        
        changeState(state, isCheckSameState, name, tSpeed, tIsLoop);

    }


    private string getAnimationName(PlayerStateEnum playerstateEm)
    {
        string resultName = string.Empty;
       
        if (playerstateEm != PlayerStateEnum.NORMALATTACK)
        {
            int stateIndex = (int)playerstateEm;
            resultName = _animationNameList[stateIndex];
            //return resultName;
        }
        else
        { 
            //给普通攻击添加多个动作
            resultName = _comobAnimationNameList[_nomarlAttackComobIndex];
            //Debug.Log("resultName=======" + resultName);
        }
        return resultName;
    
    }

    override public void updateHP()
    {
        _hpFollow.updateHpValue(_mode.hp, _mode.maxHp);
        rootView.updatePlayerInfo();
    }

    public void addDelayCall(string animatinName)
    {
        if (_animationEventDict.ContainsKey(animatinName))
        {
            AnimationState state = getAnimationState(animatinName);
            AnimationClip clip = state.clip;
            float frameRate = clip.frameRate; //1秒都少帧
            float frameInterval = 1.0f / frameRate;
            float speed = state.speed;

            List<int> eventList = _animationEventDict[animatinName];

            for (int i = 0; i < eventList.Count; i++)
            {
                int frameIndex = eventList[i];
                float time = frameIndex * frameInterval / speed;
                float p = GlobalParams.totalTime + time;
                //Debug.Log("注册时间：" + GlobalParams.totalTime + " / 预测回调时间：" + p + " 当前帧数：" + GlobalParams.frameCount + " 等待时间:" + time);
                DelayCall delayCall = new DelayCall(time, GlobalParams.frameCount, eventCallBack, this);
                GlobalParams.addDelayCall(delayCall);
            }
        }
       
    }

    //attackTargetHurt
    public void eventCallBack(BaseEnitity eniity)
    {
        //Debug.Log("testEvent======成功回调========" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);
        if (attackTarget != null && !attackTarget.isDead)
        {
            attackTargetHurt(attackTarget);
        }

        playHitSound();
        playHitEffect();

    }

    //播放击打音效
    private void playHitSound()
    {
        //根据状态区分
        BaseState curState = _stateMachine._curState;
        if (curState == _stateList[7])
        {
            //攻击状态
            int comIndex = getCurrComIndex();
            if (comIndex == 0) comIndex = 3;
            string audioName = "BeiJi_DaoJian_" + comIndex;
            AudioManager.getInstance().playSoundEffect(audioName);
        }
        else if (curState == _stateList[6])
        {
            //技能D
            AudioManager.getInstance().playSoundEffect("Hero_MagicC");
        }
        else if (curState == _stateList[5])
        {
            //技能C
            AudioManager.getInstance().playSoundEffect("Hero_MagicB");
        }
        else if (curState == _stateList[4])
        {
            //技能B
            AudioManager.getInstance().playSoundEffect("Hero_MagicB");
        }
        else if (curState == _stateList[3])
        {
            //技能A
            AudioManager.getInstance().playSoundEffect("Hero_MagicA");
        }
    }

    //播放击打特效
    private void playHitEffect()
    {
      
        Vector3 forwardOffset = Vector3.zero;
        Vector3 targetPos = Vector3.zero;
        //根据状态区分
        BaseState curState = _stateMachine._curState;
        if (curState == _stateList[6])
        {
            //技能D
            forwardOffset = _playerTransform.forward * 3;
            targetPos = _playerTransform.position + forwardOffset;
            createEffect(targetPos, _prefabPlayerMagicD, GlobalParams.SkillGroundPool);
        }
        else if (curState == _stateList[5])
        {
            //技能C
            forwardOffset = -_playerTransform.forward * 1;
            targetPos = _playerTransform.position + forwardOffset;
            targetPos.y = targetPos.y - 0.5f;

            GameObject effObj = createEffect(targetPos, _prefabPlayerMagicC, GlobalParams.SkillPool);
            iTween.MoveTo(effObj, iTween.Hash(
               "position", targetPos + _playerTransform.forward * 3 ,
              "easetype", iTween.EaseType.easeInSine,
              "time", 0.8

            ));
           
        }
        else if (curState == _stateList[4])
        {
            //技能B
            forwardOffset = -_playerTransform.forward * 1;
            targetPos = _playerTransform.position + forwardOffset;
            targetPos.y = targetPos.y - 0.5f;
            GameObject effObj = createEffect(targetPos, _prefabPlayerMagicB, GlobalParams.SkillPool);
            iTween.MoveTo(effObj, iTween.Hash(
               "position", targetPos + _playerTransform.forward * 3,
              "easetype", iTween.EaseType.easeInSine,
              "time", 0.8

            ));
            
        }
        else if (curState == _stateList[3])
        {
            //技能A
            forwardOffset = _playerTransform.forward * 3;
            targetPos = _playerTransform.position + forwardOffset;
            createEffect(targetPos, _prefabPlayerMagicA, GlobalParams.SkillGroundPool);
        }
    }

     public GameObject createEffectNoPool(string effectName, Vector3 pos, GameObject parentObj = null)
    {
        //特效上都绑定了自我销毁脚本，就不加入引用管理
        GameObject prefabObj = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, effectName, rootView._name, true, false);
        GameObject obj = GameObject.Instantiate(prefabObj);
        obj.transform.position = pos;
        if (parentObj != null)
        {
            obj.transform.parent = parentObj.transform;
            obj.transform.rotation = _playerTransform.rotation;
        }
        return obj;
       
    }

    public GameObject createEffect(Vector3 pos, GameObject prefab, string poolName)
    {
        GameObject obj = PoolManager.PoolsArray[poolName].GetGameObjectByPool(prefab,
              pos, Quaternion.identity);
        obj.transform.rotation = _playerTransform.rotation;

        return obj;
       
    }

    public AnimationState getAnimationState(string animatinName)
    {
        AnimationState targetState = null;
        if (_animationStateDict != null && _animationStateDict.ContainsKey(animatinName))
        {
            targetState = _animationStateDict[animatinName];
        }
        return targetState;
    }

    public void changeAniamtion(string animatinName, float speed = 1.0f, bool isLoop = false)
    {

        /*
         * AnimationClip:
                frameRate  帧率:1秒多少帧
         *      length     长度（秒）
         *      AddEvent 添加帧事件
         *      
         * 
         * AnimationState：
         *      clip
         *      length 长度（秒）
         *      speed  播放倍速
         *      time  好像可以回放 。。。不确定
         
         */
        if (_animation != null)
        {
            //_animation.CrossFade(animatinName);
            _animation.Stop();
            AnimationState state = getAnimationState(animatinName);
            if (state != null)
            {
                state.speed = speed;
                AnimationClip clip = state.clip;

                if (isLoop)
                {
                    //循环播放
                    _animation.wrapMode = WrapMode.Loop;
                }
                else
                {
                    _animation.wrapMode = WrapMode.Once;
                }
                _animation.CrossFade(animatinName);
            }
        }  
    }

    public float getAnimaitionPlayTime(PlayerStateEnum playerstateEm)
    {
        float time = 0.0f;
        string name = string.Empty;
        float delayTime = 0.0f;
        if (playerstateEm == PlayerStateEnum.NORMALATTACK)
        {
            if (_nomarlAttackComobIndex == _comobAnimationNameList.Count - 1)
            {
                delayTime = 0.15f;
            }
            name = _comobAnimationNameList[_nomarlAttackComobIndex];
        }
        else
        {
            name = getAnimationName(playerstateEm);
        }

        AnimationState state = getAnimationState(name);
        time = state.clip.length / state.speed;
        return time + delayTime;
    }

    public int getCurrComIndex()
    {
        return _nomarlAttackComobIndex;
      
    }

    public void AddComobIndex()
    {
        //组合动作序号+1
        _nomarlAttackComobIndex++;
        int count = _comobAnimationNameList.Count;
        if (_nomarlAttackComobIndex == count) _nomarlAttackComobIndex = 0;
    }

    public void reduceComobIndex()
    {
        _nomarlAttackComobIndex--;
        int count = _comobAnimationNameList.Count;
        if (_nomarlAttackComobIndex <= 0) _nomarlAttackComobIndex = count -1;
    }

    override public  void faceToTarget()
    {
        if (attackTarget != null)
        {
          //_gameObject.transform.LookAt(attackTarget.transform);
            GameObject tarObj = attackTarget._gameObject;
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, Quaternion.LookRotation(tarObj.transform.position - _playerTransform.position), 1.0f);
        }
    }

    override public float countDamage(BaseEnitity target)
    {
        return _mode.countDamage(target);
        
    }


    override public float getAtkValue()
    {
        return _mode.getAtkValue();
    }

    override public float getDefenceValue()
    {
        return _mode.getDefenceValue();
    }

    override public float getDexterityValue()
    {
        return _mode.getDexterityValue();
    }

    override public float getHpValue()
    {
        return _mode.getHpValue();
    }


    override public void updateMode()
    {
        _mode.update(_params);
        _params.Clear();
    }

    override public float getWarningDis()
    {
        return _mode.warningDisSquare;
    }


    override public float getAttackDis()
    {
        return _mode.attackDisSquare;
    }


    override public void onDestory()
    {
        changeStateByIndex(PlayerStateEnum.DEAD);
    }

}
