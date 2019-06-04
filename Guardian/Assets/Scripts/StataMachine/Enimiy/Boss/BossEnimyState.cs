﻿
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

public enum BossEnimyStateEnum
{
    IDLE = 0,               //待机
    RUN = 1,                //移动
    DEAD = 2,               //死亡
    NORMALATTACK = 3,       //普通攻击
    HURT = 4,               //受伤
    SKILL = 5,              //技能
}


public class BossEnimyState : EnimyState
{

    public BossEnimyStateEnum stateIndex;
    public BossEnimyState(BaseEnitity enity)
        : base(enity)
    {
       
    }

    override public void enter(params object[] values)
    {
        //切换动作
        //string animatinName = (string)values[0];
        //float speed = (float)values[1];
        //bool isLoop = (bool)values[2];
        //PlayerEnitity enitity = (PlayerEnitity)_enitity;
        //enitity.changeAniamtion(animatinName, speed, isLoop);

        ////注册帧事件
        //enitity.addDelayCall(animatinName);
    }

    override public void exit(params object[] values)
    {

    } 
}



//Boss敌人Idle状态
public class BossEnimyIdleState : BossEnimyState
{

    public BossEnimyIdleState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = BossEnimyStateEnum.IDLE;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyIdleState enter=============");

    }

    override public void excute(params object[] values)
    {
       
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
        stateIndex = BossEnimyStateEnum.RUN;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyRunState enter=============");

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
        stateIndex = BossEnimyStateEnum.DEAD;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyDeadState enter=============");

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
        stateIndex = BossEnimyStateEnum.NORMALATTACK;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyNormalAttackState enter=============");

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
        stateIndex = BossEnimyStateEnum.HURT;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimyHurtState enter=============");

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
        stateIndex = BossEnimyStateEnum.SKILL;

    }

    override public void enter(params object[] values)
    {
        Debug.Log("BossEnimySkillState enter=============");

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("BossEnimySkillState exit=============");
    }

}