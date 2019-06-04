﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class BossEnimyEnitity : EnimyEnitity
{
    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "Boss_Bruce";
        objName = "bruceObj";
        _mode.filePre = "Models/";
        _mode.file = _mode.filePre + _mode.type + "/" + objName;
        intDatas();
    }
    override public void initGameObject()
    {
        base.initGameObject();
        saveAnimationState();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 20;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 100.0f;
        _mode.hp = _mode.maxHp;

        _mode.warningDisSquare = 81;
        _mode.attackDisSquare = 25;

        _mode.moveSpeed = 5.0f;

    }

    override public void addBaseState()
    {

        //状态机
        BaseState enimyIdleState            = new BossEnimyIdleState(this);
        BaseState enimyRunState             = new BossEnimyRunState(this);
        BaseState enimyDeadState            = new BossEnimyDeadState(this);
        BaseState enimyNormalAttackState    = new BossEnimyNormalAttackState(this);
        BaseState enimyHurtState            = new BossEnimyHurtState(this);
        BaseState enimySkillState           = new BossEnimySkillState(this);

        _stateList.Add(enimyIdleState);
        _stateList.Add(enimyRunState);
        _stateList.Add(enimyDeadState);
        _stateList.Add(enimyNormalAttackState);
        _stateList.Add(enimyHurtState);
        _stateList.Add(enimySkillState);

        //状态机设置
        _stateMachine.changeState(_stateList[0], true, _animationNameList[0], 1.0f, true);
    }


    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(26);
        _animationEventDict[GlobalParams.anim_ennimy6_normalAttack1] = attack1List;

        List<int> attack2List = new List<int>();
        attack2List.Add(21);
        _animationEventDict[GlobalParams.anim_ennimy6_normalAttack2] = attack2List;

        List<int> hurtList = new List<int>();
        hurtList.Add(45);
        _animationEventDict[GlobalParams.anim_ennimy6_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy6_idle1);
        _animationNameList.Add(GlobalParams.anim_ennimy6_run);
        _animationNameList.Add(GlobalParams.anim_ennimy6_death);
        _animationNameList.Add(GlobalParams.anim_ennimy6_normalAttack1);
        _animationNameList.Add(GlobalParams.anim_ennimy6_hurt);
        _animationNameList.Add(GlobalParams.anim_ennimy6_skill);

        //额外攻击动作
        _animationNameList.Add(GlobalParams.anim_ennimy6_normalAttack2);

        //额外休闲动作
        _animationNameList.Add(GlobalParams.anim_ennimy6_idle2);
    }


    override public string getAnimationName(EnimyStateEnum state, int commbex = 0)
    {
        int index = (int)state;
        if (commbex > 0)
        {
            if (state == EnimyStateEnum.IDLE)
            {
                if (commbex == 1)
                {
                    return _animationNameList[0];
                }
                else if (commbex == 2)
                {
                    return _animationNameList[7];
                }
            }
            else if (state == EnimyStateEnum.NORMALATTACK)
            {
                if (commbex == 1)
                {
                    return _animationNameList[3];
                }
                else if (commbex == 2)
                {
                    return _animationNameList[6];
                }
            }
        }
        return _animationNameList[index];
    }

    void saveAnimationState()
    {
        if (_animation != null)
        {
            foreach (AnimationState state in _animation)
            {
                _animationStateDict[state.name] = state;
            }
        }
       
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


    override public void addDelayCall(EnimyStateEnum state, float speed = 1.0f, int commbex = 0)
    {
        //拿到对应的动画名称
        int index = (int)state;
        string animationName = getAnimationName(state, commbex);
        if (animationName != null)
        {
            //拿到对应的动画帧事件
            if (_animationEventDict.ContainsKey(animationName))
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

    override public void realAddDelayCall(string animatinName, int frameIndex, float speed = 1.0f)
    {
       
        float time = getClipLength(animatinName, frameIndex);
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


    override public void eventCallBack(BaseEnitity eniity, string animationName, bool isMove = false, Vector3 targetPos = default(Vector3))
    {
        //Debug.Log(GetType() + "testEvent======成功回调========" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);

        if (isHurt)
        {
            //受伤事件
            isHurt = false;
        }
        else if (isAttacking)
        {

            //攻击伤害事件
            if (attackTarget != null)
            {

                attackTargetHurt(attackTarget);
                attackTarget.updateHP();
            }
        }
 
    }

}
