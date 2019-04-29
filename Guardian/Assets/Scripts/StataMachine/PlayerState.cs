using System.Collections;
using System.Collections.Generic;
using UnityEngine;



        //_animationNameList.Add("Idle");
        //_animationNameList.Add("Run");
        //_animationNameList.Add("Attack2");
        //_animationNameList.Add("Death");
public enum PlayerStateEnum
{
    IDLE            = 0,
    RUN             = 1,
    ATTACK          = 2,
    DEAD            = 3,
}


public class PlayerState : BaseState {

    public PlayerStateEnum stateIndex;
    public PlayerState(BaseEnitity enity)
        : base(enity)
    {
       
    }

    override public void enter(params object[] values)
    {
        //切换动作
        string animatinName = (string)values[0];
        float speed = (float)values[1];
        bool isLoop = (bool)values[2];
        PlayerEnitity enitity = (PlayerEnitity)_enitity;
        enitity.changeAniamtion(animatinName, speed, isLoop);

    } 
}



//玩家Idle状态
public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(BaseEnitity enity):base(enity)
    {
        stateIndex = PlayerStateEnum.IDLE;
    }

    override public  void enter(params object[] values)
    {
        //切换动作
        base.enter(values);
        //changeAniamtion(animatinName);
        
    }

    override public void excute(params object[] values)
    {
        //Debug.Log("玩家excute================");
    }


    override public void exit(params object[] values)
    {
        
    }

    override public bool onMessage(BaseEnitity enitity, Message msg)
    {
        if (msg._messageId == MessageCustomType.msg1)
        {

            Dictionary<string, object> extraInfo = msg._extraInfo;

            string valus = (string)extraInfo["msgParams"];
            Debug.Log("接收到消息了" + msg._dispatchTime + " 消息ID：" + msg._messageId + " 消息内容是：" + valus);

            return true;
        }

        return false;
    }

}


//玩家Run状态
public class PlayerRunState : PlayerState
{

    public PlayerRunState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.RUN;
    }

    override public void enter(params object[] values)
    {
        base.enter(values);

    }

    override public void excute(params object[] values)
    {
       
    }


    override public void exit(params object[] values)
    {

    }


}


//玩家Attack状态
public class PlayerAttackState : PlayerState
{

    public PlayerAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.ATTACK;
    }

    override public void enter(params object[] values)
    {
        base.enter(values);

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {

    }


}



//玩家Dead状态
public class PlayerDeadState : PlayerState
{

    public PlayerDeadState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.DEAD;
    }

    override public void enter(params object[] values)
    {
        base.enter(values);

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {

    }


}

