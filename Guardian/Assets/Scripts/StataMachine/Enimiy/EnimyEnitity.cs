using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class EnimyEnitity : BaseEnitity {


    public EnimyEnitiyMode _mode;
    private PlayerEnitity _playerEnitiy;

    private Transform _selfTransform;
    private CharacterController _CC;
    public EnimyEnitity()
    {
        damageLabelOffsetY = 50.0f;
    }

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
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
        _playerEnitiy = enitity;
    }
   

    override public void initGameObject()
    {

        if (_rootObj)
        {
            GameObject playerGameObject = _playerEnitiy._gameObject;
            _gameObject = getGameObject(_mode.file, "warrior_green", _rootObj, Vector3.zero);
            _gameObject.transform.localPosition = playerGameObject.transform.position + new Vector3(2.0f, 0.0f, 2.0f);

            _gameObject.AddComponent<EnimyEvent>();
            _animator = _gameObject.GetComponent<Animator>();
            _selfTransform = _gameObject.transform;
            _CC = _gameObject.GetComponent<CharacterController>();

        }
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

    public void addDelayCall(string animatinName, int frameIndex, float speed = 1.0f)
    {
        _animator.speed = speed;
        float time = getClipLength(_animator, animatinName, frameIndex);
        float p = GlobalParams.totalTime + time;
        //Debug.Log(GetType() + "注册时间：" + GlobalParams.totalTime + " / 预测回调时间：" + p + " 当前帧数：" + GlobalParams.frameCount + " 等待时间:" + time);
        DelayCall delayCall = new DelayCall(time, GlobalParams.frameCount, eventCallBack, this);
        GlobalParams.addDelayCall(delayCall);
    }

    public void eventCallBack(BaseEnitity eniity)
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
        Texture mainT = Resources.Load<Texture>("Models/Enemys/Skeleton_Pack/Textures/warrior/skeleton_warrior__variant5");
        burn.setMainTex(mainT);
        burn.setNameList("armor", "eyes", "helmet", "Skeletonl_base", "shield", "sword");

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
                _selfTransform.rotation = Quaternion.Slerp(_selfTransform.rotation, Quaternion.LookRotation(tarObj.transform.position - _selfTransform.position), 1.0f);
            }
            
        }
    }

    public void updateMove()
    {
        if (_CC != null)
        {
            float moveSpeed = 5.0f;
            Vector3 v = Vector3.ClampMagnitude(moveTarget._gameObject.transform.position - _selfTransform.position, moveSpeed * Time.deltaTime);
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


}
