using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class EnimyEnitity : BaseEnitity {


    public EnimyEnitiyMode _mode;
    public PlayerEnitity playerEnitiy;

    public Transform selfTransform;
    public Transform playerTransform;
    private CharacterController _CC;

    //动画帧事件集合
    public Dictionary<string, List<int>> _animationEventDict;

    //动画剪辑名称
    public List<string> _animationNameList;

    public GameObject skillGround;
    public GameObject skillLayer;


    private GameObject _prefabHurt;

    public string mainTexturePath = "warrior/skeleton_warrior__variant5";
    public object[] cObjList = new object[] { "armor", "eyes", "helmet", "Skeletonl_base", "shield", "sword" };

    public EnimyEnitity()
    {
        damageLabelOffsetY = 50.0f;
        _animationEventDict = new Dictionary<string, List<int>>();
        _animationNameList = new List<string>();

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
        _stateMachine.setCurrentState(enimyIdleState);
    }

    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        playerEnitiy = enitity;
        playerTransform = playerEnitiy._gameObject.transform;
    }

    virtual public void initBufferPoolPrefab()
    {
        _prefabHurt = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/Hero_hurtA", rootView._name, true, false); 

    }
    override public void initGameObject()
    {
        addAinimainEvents();
        addAnimationNames();
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

        }
    }

    //不同模型的动画帧事件不一样 必须重载
    virtual public void addAinimainEvents()
    {
        
        List<int> attack1List = new List<int>();
        attack1List.Add(10);
        _animationEventDict[GlobalParams.anim_ennimy1_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy1_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    virtual public void addAnimationNames()
    { 
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy1_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy1_run);
        _animationNameList.Add(GlobalParams.anim_ennimy1_death);
        _animationNameList.Add(GlobalParams.anim_ennimy1_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy1_hurt);
    }

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
                float time = frameIndex * frameInterval / speed - frameInterval;
                return time;

            }
               
        }
        return 0F;
    }


    public string getAnimationName(EnimyStateEnum state)
    { 
        int index = (int)state;
        return _animationNameList[index];
    }
     public void addDelayCall(EnimyStateEnum state, float speed = 1.0f)
    {
        //拿到对应的动画名称
        int index = (int)state;
        string animationName = _animationNameList[index];
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

    public void realAddDelayCall(string animatinName, int frameIndex, float speed = 1.0f)
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

    public void playHitEffect(string animationName)
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

    public void changeStateByIndex(EnimyStateEnum enimyStateEm, bool isCheckSameState = true)
    {
        int stateIndex = (int)enimyStateEm;
        BaseState state = _stateList[stateIndex];
        //string name = getAnimationName(playerstateEm);

        //changeState(state, true, name, tSpeed, tIsLoop);
        changeState(state, isCheckSameState);

    }

    override public void onDestory()
    {
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
            //_gameObject.transform.LookAt(attackTarget.transform);
            GameObject tarObj = null;
            if (isMove)
            {
                tarObj = moveTarget._gameObject;
            }
            else if (isAttacking)
            {
                tarObj = attackTarget._gameObject;
            }

            if (tarObj != null)
            {
                selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, Quaternion.LookRotation(tarObj.transform.position - selfTransform.position), 1.0f);
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
            Vector3 playerPos = target._gameObject.transform.position;

            //这里考虑下是否需要切换目标，
            if (isAttacking)
            {
                //如果正在攻击 不切换目标
                return;
            }
            attackTarget = null;

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
            }
            else if (dis <= warningDis) //警戒范围
            {

                isMove = true;
                isAttacking = false;
                moveTarget = target;
            }
            else
            {
                isMove = false;
                isAttacking = false;
            }

        }
    }
}
