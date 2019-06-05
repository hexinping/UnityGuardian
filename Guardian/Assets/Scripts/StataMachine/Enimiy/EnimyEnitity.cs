﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class EnimyEnitity : BaseEnitity {


    public EnimyEnitiyMode _mode;
    public PlayerEnitity playerEnitiy;

    public Transform selfTransform;
    public Transform playerTransform;
    private CharacterController _CC;

    public GameObject skillGround;
    public GameObject skillLayer;


    public GameObject _prefabHurt;
    public GameObject _prefabHp;

    public string mainTexturePath = "warrior/skeleton_warrior__variant5";
    public object[] cObjList = new object[] { "armor", "eyes", "helmet", "Skeletonl_base", "shield", "sword" };

    public HpFollow _hpFollow;

    //动画状态 传统动画方式
    public Dictionary<string, AnimationState> _animationStateDict;
    public Animation _animation;

    public string attackSoundFile = string.Empty;

    public float hpHeight = 100;

    public EnimyEnitity()
    {
        damageLabelOffsetY = 50.0f;
        _animationStateDict = new Dictionary<string, AnimationState>();

        skillGround = GameObject.Find("_Manager/_ViewManager/_Scene/SkillGround");
        skillLayer = GameObject.Find("_Manager/_ViewManager/_Scene/Skill");
    }

    virtual public void intDatas()
    { 
        
    }


    //不同模型数据不一样，需要重载
    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_green";
        _mode.file = _mode.file + objName;
    }

    override public void addBaseState()
    {
        //状态机
        BaseState enimyIdleState = new EnimyIdleState(this);
        BaseState enimyRunState = new EnimyRunState(this);
        BaseState enimyDeadState = new EnimyDeadState(this);
        BaseState enimyNormalAttackState = new EnimyNormalAttackState(this);
        BaseState enimyHurtState = new EnimyHurtState(this);

        _stateList.Add(enimyIdleState);
        _stateList.Add(enimyRunState);
        _stateList.Add(enimyDeadState);
        _stateList.Add(enimyNormalAttackState);
        _stateList.Add(enimyHurtState);

        //状态机设置
        _stateMachine.changeState(enimyIdleState);
    }

    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        playerEnitiy = enitity;
        playerTransform = playerEnitiy._gameObject.transform;
    }

    virtual public void initBufferPoolPrefab()
    {
        _prefabHurt = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_hurtA", rootView._name, true, false);
        _prefabHp = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "Prefabs/View/EnimyHp", rootView._name, true);

    }
    override public void initGameObject()
    {
        initBufferPoolPrefab();
        if (_rootObj)
        {
            GameObject playerGameObject = playerEnitiy._gameObject;
            _gameObject = getGameObject(_mode.file, objName, _rootObj, Vector3.zero);
            _gameObject.transform.localPosition = playerGameObject.transform.position + new Vector3(2.0f, 0.0f, 2.0f);

            _gameObject.AddComponent<EnimyEvent>();
            _animator = _gameObject.GetComponent<Animator>();
            selfTransform = _gameObject.transform;
            _CC = _gameObject.GetComponent<CharacterController>();
            _animation = _gameObject.GetComponent<Animation>();


            //使用缓冲池床创建血条
            GameObject obj = PoolManager.PoolsArray[GlobalParams.HPPool].GetGameObjectByPool(_prefabHp,
                _gameObject.transform.position, Quaternion.identity);
            HpFollow hpFollow = obj.GetComponent<HpFollow>();
            hpFollow.setHpUIDatas(new Vector2(0, hpHeight), _mode.hp, _mode.maxHp);
            hpFollow.target = _gameObject.transform;
            _hpFollow = hpFollow;

        }
    }

    override public void updateHP()
    {
        _hpFollow.updateHpValue(_mode.hp, _mode.maxHp);
    }

    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {
        
        List<int> attack1List = new List<int>();
        attack1List.Add(10);
        _animationEventDict[GlobalParams.anim_ennimy1_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy1_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    { 
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy1_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy1_run);
        _animationNameList.Add(GlobalParams.anim_ennimy1_death);
        _animationNameList.Add(GlobalParams.anim_ennimy1_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy1_hurt);
    }


    public AnimationState getAnimationState(string anmationName)
    {
        AnimationState targetState = null;
        if (_animationStateDict != null && _animationStateDict.ContainsKey(anmationName))
        {
            targetState = _animationStateDict[anmationName];
        }
        return targetState;
    }

    //获取某个动作的播放时长 animation方式
    public float getClipTotalLength(string anmationName, float delayTime = 0.0f)
    {
        AnimationState state = getAnimationState(anmationName);
        if (state != null)
        {
            AnimationClip clip = state.clip;
            float speed = state.speed;
            float time = clip.length / speed + delayTime;
            return time;
        }
        return 0.0f;
    }

    //回到一个动作具体某帧的时间 animation方式
    public float getClipLength(string anmationName, int frameIndex)
    {
        AnimationState state = getAnimationState(anmationName);
        if (state != null)
        {
            AnimationClip clip = state.clip;
            float frameRate = clip.frameRate; //1秒都少帧
            float frameInterval = 1.0f / frameRate;
            float speed = state.speed;
            float time = frameIndex * frameInterval / speed;
            return time;
        }
       
        return 0.0f;
    }


    //返回到一个动作具体某帧的时间 animator方式
    public  float getClipLength(Animator animator, string clip, int frameIndex)
    {
        if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
            return 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0) return 0;
        AnimationClip tAnimationClip;
        for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
        {
            tAnimationClip = ac.animationClips[tCounter];
            if (null != tAnimationClip && tAnimationClip.name == clip)
            {
                float speed = animator.speed;
                float frameRate = tAnimationClip.frameRate; //1秒都少帧
                float frameInterval = 1.0f / frameRate;
                float time = frameIndex * frameInterval / speed;
                return time;

            }
               
        }
        return 0F;
    }

    //返回某个动作总播放时间 animator方式
    public float getClipTotalLength(Animator animator, string clip, float delayTime = 0.0f)
    {
        if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
            return 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0) return 0;
        AnimationClip tAnimationClip;
        for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
        {
            tAnimationClip = ac.animationClips[tCounter];
            if (null != tAnimationClip && tAnimationClip.name == clip)
            {
                float speed = animator.speed;
                float time = tAnimationClip.length / speed + delayTime;
                return time;


            }

        }
        return 0F;
    }


    virtual public string getAnimationName(EnimyStateEnum state, int commbex = 0)
    { 
        int index = (int)state;
        return _animationNameList[index];
    }
    virtual public void addDelayCall(EnimyStateEnum state, float speed = 1.0f, int commbex = 0)
    {
        //拿到对应的动画名称
        int index = (int)state;
        string animationName = getAnimationName(state, commbex);
        if (animationName != null)
        {
            //拿到对应的动画帧事件
            if(_animationEventDict.ContainsKey(animationName))
            {
                List<int> eventList = _animationEventDict[animationName];
                for (int i = 0; i < eventList.Count; i++)
                {
                    int frameIndex = eventList[i];
                    realAddDelayCall(animationName, frameIndex, speed);
                }
            }
            
        }
    }

    virtual public void realAddDelayCall(string animatinName, int frameIndex, float speed = 1.0f)
    {
        _animator.speed = speed;
        float time = getClipLength(_animator, animatinName, frameIndex);
        float p = GlobalParams.totalTime + time;
        //Debug.Log(GetType() + "注册时间：" + GlobalParams.totalTime + " / 预测回调时间：" + p + " 当前帧数：" + GlobalParams.frameCount + " 等待时间:" + time);
        if (attackTarget != null)
        {
            DelayCall delayCall = new DelayCall(time, GlobalParams.frameCount, eventCallBack, this, animatinName, false, attackTarget._gameObject.transform.position);
            GlobalParams.addDelayCall(delayCall);
        }
        else
        {
            DelayCall delayCall = new DelayCall(time, GlobalParams.frameCount, eventCallBack, this, animatinName, false);
            GlobalParams.addDelayCall(delayCall);
        }
       
    }

    virtual public void eventCallBack(BaseEnitity eniity, string animationName, bool isMove = false, Vector3 targetPos = default(Vector3))
    {
        //Debug.Log(GetType() + "testEvent======成功回调========" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);

        if (isHurt)
        {
            //受伤事件
            isHurt = false;
        }
        else if(isAttacking)
        {

            //攻击伤害事件
            if (attackTarget != null)
            {
                //_animator.enabled = false;
                attackTargetHurt(attackTarget);
                attackTarget.updateHP();
               // _animator
            }
          
            
        }
        _animator.speed = 1.0f;
    }

   virtual public void playHitEffect(string animationName)
    {
        Vector3 forwardOffset = Vector3.zero;
        Vector3 targetPos = Vector3.zero;
        if (animationName == GlobalParams.anim_ennimy1_hurt)
        {
            //受伤状态特效
            targetPos = selfTransform.position + forwardOffset;
            createEffect(targetPos, _prefabHurt, GlobalParams.SkillPool);
        }
    }


    public GameObject createEffect(Vector3 pos, GameObject prefab, string poolName)
    {
        GameObject obj = PoolManager.PoolsArray[poolName].GetGameObjectByPool(prefab,
              pos, Quaternion.identity);
        obj.transform.rotation = selfTransform.rotation;

        return obj;
       
    }


    public GameObject createEffectNoPool(string effectName, Vector3 pos, GameObject parentObj = null)
    {
        GameObject prefabObj = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, effectName, rootView._name, true, false);
        GameObject obj = GameObject.Instantiate(prefabObj);
        obj.transform.position = pos;
        if (parentObj != null)
        {
            obj.transform.parent = parentObj.transform;

            obj.transform.rotation = selfTransform.rotation;
        }
        return obj;

    }

    virtual public void changeStateByIndex(EnimyStateEnum enimyStateEm, bool isCheckSameState = true, float tSpeed = 1.0f, bool tIsLoop = false)
    {
        int stateIndex = (int)enimyStateEm;
        BaseState state = _stateList[stateIndex];
        string name = getAnimationName(enimyStateEm);
        changeState(state, isCheckSameState, name, tSpeed, tIsLoop);

    }

    override public void onDestory()
    {

        //血条回收
        PoolManager.PoolsArray[GlobalParams.HPPool].RecoverGameObjectToPools(_hpFollow.gameObject);

        BurnHelper burn = _gameObject.AddComponent<BurnHelper>();
        Texture mainT = (Texture)ResourcesManager.getInstance().getResouce(ResourceType.Texture, "Models/Enemys/Skeleton_Pack/Textures/" + mainTexturePath, rootView._name, true, false);
        burn.setMainTex(mainT);

        if (cObjList.Length > 0)
        {
            burn.setNameList(cObjList);
        }
 
        changeStateByIndex(EnimyStateEnum.DEAD);
        LevelOneView view = (LevelOneView)rootView;
        view.removeFromEnimyList(this);
    }

    override public void faceToTarget()
    {
        if (attackTarget != null || moveTarget !=null)
        {
            if (targetTransform != null)
            {
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, Quaternion.LookRotation(targetTransform.position - selfTransform.position), 1.0f);
            }
            
        }
    }

    public void updateMove()
    {
        if (_CC != null)
        {
            float moveSpeed = _mode.moveSpeed;
            Vector3 v = Vector3.ClampMagnitude(moveTarget._gameObject.transform.position - selfTransform.position, moveSpeed * Time.deltaTime);
            _CC.Move(v);
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

    //选择最近的敌人
    BaseEnitity getNearestPlayer(List<BaseEnitity> list)
    {
        BaseEnitity target = null;
        float minDis = 1000000.0f;
        for (int i = 0; i < list.Count; i++)
        {
            BaseEnitity enitity = list[i];

            if (!enitity.isDead)
            {
                //这里后面可以换成属性计算，todo
                float dis = (enitity._gameObject.transform.position - _gameObject.transform.position).sqrMagnitude;  //距离的平方
                if (dis < minDis)
                {
                    minDis = dis;
                    target = enitity;
                }
            }
           
        }
        return target;
    
    }

    override public void findTarget(List<BaseEnitity> list)
    {
        //不同的敌人可以有不同的索敌方式
        BaseEnitity target = getNearestPlayer(list);

        if (target != null)
        {
            Transform tarTrans = target._gameObject.transform;
            Vector3 playerPos = tarTrans.position;

            //这里考虑下是否需要切换目标，
            if (isAttacking)
            {
                //如果正在攻击 不切换目标
                return;
            }
            attackTarget = null;
            targetTransform = null;

            //移动过程中永远选择离自己最近的敌人
            moveTarget = null;
            float warningDis = getWarningDis();
            float attackDis = getAttackDis();
            Vector3 enimyPos = selfTransform.position;
            float dis = (playerPos - enimyPos).sqrMagnitude;  //距离的平方
            if (dis <= attackDis)  //攻击范围
            {
                isMove = false;
                isAttacking = true;
                attackTarget = target;
                targetTransform = tarTrans;
            }
            else if (dis <= warningDis) //警戒范围
            {

                isMove = true;
                isAttacking = false;
                moveTarget = target;
                targetTransform = tarTrans;
            }
            else
            {
                isMove = false;
                isAttacking = false;
            }

        }
    }
}
