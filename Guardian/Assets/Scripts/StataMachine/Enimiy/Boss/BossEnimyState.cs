
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
        enitity.changeAniamtion(animatinName, speed, isLoop);

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
        BossEnimyEnitity enitity = (BossEnimyEnitity)_enitity;
        _speed = (float)values[1];
        _isLoop = (bool)values[2];
        changeCommbexAnimation(enitity, commbexIndex);
        commbexIndex = commbexIndex % 2 + 1;
    }

    override public void excute(params object[] values)
    {
        if (GlobalParams.totalTime >= _endPlayTime)
        {
            BossEnimyEnitity enitity = (BossEnimyEnitity)_enitity;
            changeCommbexAnimation(enitity, commbexIndex);
            commbexIndex = commbexIndex % 2 + 1;
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyIdleState exit=============");
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
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyRunState exit=============");
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
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyDeadState exit=============");
    }

}

//Boss敌人NormalAttaclk状态
public class BossEnimyNormalAttackState : BossEnimyState
{

    public BossEnimyNormalAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.NORMALATTACK;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyNormalAttackState enter=============");
        base.enter(values);
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyNormalAttackState exit=============");
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
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimyHurtState exit=============");
    }

}

//Boss敌人Skill状态
public class BossEnimySkillState : BossEnimyState
{

    public BossEnimySkillState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.SKILL;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimySkillState enter=============");
        base.enter(values);
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimySkillState exit=============");
    }

}