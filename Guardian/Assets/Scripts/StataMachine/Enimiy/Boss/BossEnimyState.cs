
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




public class BossEnimyState : EnimyState
{

    public EnimyStateEnum stateIndex;
    public BossEnimyState(BaseEnitity enity)
        : base(enity)
    {
       
    }

    override public void enter(params object[] values)
    {
        //切换动作
        string animatinName = (string)values[0];
        float speed = (float)values[1];
        bool isLoop = (bool)values[2];
        BossEnimyEnitity enitity = (BossEnimyEnitity)_enitity;
        enitity.changeAniamtion(animatinName, speed, isLoop); //设置了速度

        //注册帧事件
        enitity.addDelayCall(stateIndex);
    }

    override public void exit(params object[] values)
    {

    }

    
}



//Boss敌人Idle状态
public class BossEnimyIdleState : BossEnimyState
{
    private float _endPlayTime = 0.0f;
    private float _playTotalTime = 0.0f;
    public static int commbexIndex = 1;

    private float _speed = 1.0f;
    private bool _isLoop = false;
    

    public BossEnimyIdleState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.IDLE;

    }

    void changeCommbexAnimation(BossEnimyEnitity e, int commbexIndex)
    {
        string animatinName = e.getAnimationName(stateIndex, commbexIndex);
        e.changeAniamtion(animatinName, _speed, _isLoop);
  
        //下一次播放时间
        float delayTime = 0.0f;
        float time = e.getClipTotalLength(animatinName, delayTime);
        float p = GlobalParams.totalTime + time;
        _endPlayTime = p;
        _playTotalTime = time;
        
    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyIdleState enter=============");

        _enitity.isMove = false;
        _enitity.isAttacking = false;
        _enitity.isDead = false;
        _enitity.isHurt = false;

        BossEnimyEnitity enitity = (BossEnimyEnitity)_enitity;
        _speed = (float)values[1];
        _isLoop = (bool)values[2];
        changeCommbexAnimation(enitity, commbexIndex);
        //组合动画下标
        commbexIndex = commbexIndex % 2 + 1;

        AudioManager.getInstance().playSoundEffect(GlobalParams.sound_boss1_born);
    }

    override public void excute(params object[] values)
    {

        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        if (e.isMove)
        {
            e.changeStateByIndex(EnimyStateEnum.RUN, true, 1.0f, true);
        }
        else if(e.isAttacking)
        {
            e.changeStateByIndex(EnimyStateEnum.NORMALATTACK);
        }
        else if(e.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
        }
        else if (e.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }
        else
        {
            if (GlobalParams.totalTime >= _endPlayTime)
            {
                BossEnimyEnitity enitity = (BossEnimyEnitity)_enitity;
                changeCommbexAnimation(enitity, commbexIndex);
                //组合动画下标
                commbexIndex = commbexIndex % 2 + 1;
            }
        }
      
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyIdleState exit=============");
        _endPlayTime = 0.0f;
        _playTotalTime = 0.0f;
        commbexIndex = 1;
        _speed = 1.0f;
        _isLoop = false;
    }

}



//Boss敌人Run状态
public class BossEnimyRunState : BossEnimyState
{

    public BossEnimyRunState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.RUN;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyRunState enter=============");
        base.enter(values);
        _enitity.isMove = true;
    }

    override public void excute(params object[] values)
    {
        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        if (!e.isMove)
        {
            //切换到移动状态
            e.changeStateByIndex(EnimyStateEnum.IDLE, true, 1.0f, true);
        }
        else if (e.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
        }
        else if (e.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }
        else
        {
            if (_enitity.moveTarget != null)
            {
                e.faceToTarget();
                e.updateMove();
            }

        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyRunState exit=============");
        _enitity.isMove = false;
    }

}

//Boss敌人Dead状态
public class BossEnimyDeadState : BossEnimyState
{

    public BossEnimyDeadState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.DEAD;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyDeadState enter=============");
        base.enter(values);
        _enitity.isDead = true;

        AudioManager.getInstance().playSoundEffect(GlobalParams.sound_boss1_dead);
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyDeadState exit=============");
        _enitity.isDead = false;
    }

}

//Boss敌人NormalAttaclk状态
public class BossEnimyNormalAttackState : BossEnimyState
{
    private float _endPlayTime = 0.0f;
    private float _playTotalTime = 0.0f;
    

    private float _speed = 1.0f;
    private bool _isLoop = false;

    private Transform _targetTransform;
    private Transform _selfTransform;

    public static int commbexIndex = 1;
    public static int continuousAttack = 0; //连续攻击次数
    private int maxContinuousAttack = 4;    //连续攻击最大次数
    

    public BossEnimyNormalAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.NORMALATTACK;

    }

    void changeCommbexAnimation(BossEnimyEnitity e, int commbexIndex)
    {
        string animatinName = e.getAnimationName(stateIndex, commbexIndex);
        e.changeAniamtion(animatinName, _speed, _isLoop); //设置了速度

        //下一次播放时间
        float delayTime = 0.0f;
        float time = e.getClipTotalLength(animatinName, delayTime);
        float p = GlobalParams.totalTime + time;
        _endPlayTime = p;
        _playTotalTime = time;

        //注册事件
        e.addDelayCall(stateIndex, _speed, commbexIndex);
    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyNormalAttackState enter=============");

        _enitity.isAttacking = true;
        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;

        //面向目标
        e.faceToTarget();
        _targetTransform = e.attackTarget._gameObject.transform;
        _selfTransform = e._gameObject.transform;

        _speed = (float)values[1];
        _isLoop = (bool)values[2];
        changeCommbexAnimation(e, commbexIndex);

        //组合动画下标
        commbexIndex = commbexIndex % 2 + 1;

        continuousAttack++;
    }

    override public void excute(params object[] values)
    {

        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        if (!e.isAttacking)
        {
            //切换到移动状态
            e.changeStateByIndex(EnimyStateEnum.IDLE);
        }
        else if (e.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
        }
        else if (e.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }
        else
        {
            //如果攻击目标脱离了我的攻击范围，结束攻击重新寻敌
            BaseEnitity target = e.attackTarget;
            if (target != null)
            {
                float dis = (_targetTransform.position - _selfTransform.position).sqrMagnitude;  //距离的平方
                float attDis = _enitity.getAttackDis();
                if (dis > attDis)
                {
                    _enitity.isAttacking = false;
                    e.changeStateByIndex(EnimyStateEnum.IDLE);
                    return;
                }
            }

            //面向目标
            e.faceToTarget();

            if (GlobalParams.totalTime >= _endPlayTime)
            {
                continuousAttack++;
                if (continuousAttack > maxContinuousAttack)
                {
                    continuousAttack = 0;
                    e.changeStateByIndex(EnimyStateEnum.SKILL);
                    return;
                }
                changeCommbexAnimation(e, commbexIndex);
                //组合动画下标
                commbexIndex = commbexIndex % 2 + 1;
               
            }
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyNormalAttackState exit=============");
        _enitity.isAttacking = false;

        _endPlayTime = 0.0f;
        _playTotalTime = 0.0f;
         commbexIndex = 1;
         continuousAttack = 0;
        _speed = 1.0f;
        _isLoop = false;
    }

}

//Boss敌人Hurt状态
public class BossEnimyHurtState : BossEnimyState
{

    public BossEnimyHurtState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.HURT;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyHurtState enter=============");
        base.enter(values);

        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        e.isHurt = true;

        //播放特效
        e.playHitEffect(GlobalParams.anim_ennimy6_hurt);
    }

    override public void excute(params object[] values)
    {
        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        if (!e.isHurt)
        {
            //切换到Idle状态
            e.changeStateByIndex(EnimyStateEnum.IDLE, true, 1.0f, true);
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyHurtState exit=============");
        _enitity.isHurt = false;
    }

}

//Boss敌人Skill状态
public class BossEnimySkillState : BossEnimyState
{
    private float _endPlayTime = 0.0f;
    private float _playTotalTime = 0.0f;

    public BossEnimySkillState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.SKILL;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimySkillState enter=============");
        base.enter(values);
        _enitity.isPlaySkill = true;
        _enitity.isAttacking = true;

        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        string animatinName = e.getAnimationName(stateIndex);

        //下一次播放时间
        float delayTime = 0.0f;
        float time = e.getClipTotalLength(animatinName, delayTime);
        float p = GlobalParams.totalTime + time;
        _endPlayTime = p;
        _playTotalTime = time;

        //注册事件
        e.addDelayCall(stateIndex);

    }

    override public void excute(params object[] values)
    {
        BossEnimyEnitity e = (BossEnimyEnitity)_enitity;
        if (GlobalParams.totalTime >= _endPlayTime)
        {
            e.isPlaySkill = false;
            e.playOnlyOnceSkillEffect = true;
            //切换到Idle状态
            e.changeStateByIndex(EnimyStateEnum.IDLE, true, 1.0f, true);
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimySkillState exit=============");
        _enitity.isPlaySkill = false;
        _enitity.isAttacking = false;
        _endPlayTime = 0.0f;
        _playTotalTime = 0.0f;
    }

}