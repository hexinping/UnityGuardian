using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnimyStateEnum
{
    IDLE            = 0,               //待机
    RUN             = 1,               //移动
    DEAD            = 2,               //死亡
    NORMALATTACK    = 3,               //普通攻击
    HURT            = 4,               //受伤
}




public class EnimyState : BaseState
{
    public EnimyStateEnum stateIndex;
    public EnimyState(BaseEnitity enity)
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

        //注册帧事件
        //enitity.addDelayCall(animatinName);
    } 
}


//敌人Idle状态
public class EnimyIdleState : EnimyState
{

    public EnimyIdleState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.IDLE;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("EnimyIdleState enter=============");
        //切换动作
        base.enter(values);
        


    }

    override public void excute(params object[] values)
    {
       
    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyIdleState exit=============");
    }

}

//敌人Run状态
public class EnimyRunState : EnimyState
{

    public EnimyRunState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.RUN;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("EnimyRunState enter=============");
        //切换动作
        base.enter(values);



    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyRunState exit=============");
    }

}

//敌人Dead状态
public class EnimyDeadState : EnimyState
{

    public EnimyDeadState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.DEAD;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("EnimyDeadState enter=============");
        //切换动作
        base.enter(values);



    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyDeadState exit=============");
    }

}


//敌人NormalAttack状态
public class EnimyNormalAttackState : EnimyState
{

    public EnimyNormalAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.NORMALATTACK;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("EnimyNormalAttackState enter=============");
        //切换动作
        base.enter(values);



    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyNormalAttackState exit=============");
    }

}


//敌人受伤状态
public class EnimyHurtState : EnimyState
{

    public EnimyHurtState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.HURT;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("EnimyHurtState enter=============");
        //切换动作
        base.enter(values);


    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyHurtState exit=============");
    }

}
