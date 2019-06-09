using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum
{
    IDLE                    = 0,       //待机
    RUN                     = 1,       //移动
    DEAD                    = 2,       //死亡
    MAGICTRICKA             = 3,       //普通技能
    MAGICTRICKB             = 4,       //大招技能
    MAGICTRICKC             = 5,       //普通技能
    MAGICTRICKD             = 6,       //大招技能
    NORMALATTACK            = 7,       //普通攻击


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

        //注册帧事件
        enitity.addDelayCall(animatinName);
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
        Debug.Log("idle enter=============");
        //切换动作
        base.enter(values);
        _enitity.faceToTarget();
        
        
    }

    override public void excute(params object[] values)
    {
        //Debug.Log("玩家excute================");
    }


    override public void exit(params object[] values)
    {
        Debug.Log("idle exit=============");
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
        Debug.Log("run enter=============");
        base.enter(values);
        _enitity.isMove = true;

    }

    override public void excute(params object[] values)
    {


    }


    override public void exit(params object[] values)
    {
        Debug.Log("run exit=============");
        _enitity.isMove = false;
    }


}


//玩家Attack状态
public class PlayerAttackState : PlayerState
{

    public PlayerAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.NORMALATTACK;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("attack enter=============");
        _enitity.isAttacking = true;
        base.enter(values);
        _enitity.faceToTarget();

        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.showHpSlider();
        }
    }

    override public void excute(params object[] values)
    {
        //Debug.Log("attack excute=============");
        _enitity.faceToTarget();
    }


    override public void exit(params object[] values)
    {
        Debug.Log("attack exit=============");
        _enitity.isAttacking = false;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.hideHpSlider();
        }
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
        Debug.Log("dead enter=============");
        base.enter(values);
        _enitity.isDead = true;
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("dead exit=============");
        _enitity.isDead = false;
    }

}

//玩家使用普通技能状态
public class PlayerMagicTrickAState : PlayerState
{

    public PlayerMagicTrickAState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.MAGICTRICKA;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("magicTrickA enter=============");
        base.enter(values);
        _enitity.isAttacking = true;
         if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.showHpSlider();
        }

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("magicTrickA exit=============");
        _enitity.isAttacking = false;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.hideHpSlider();
        }
    }

}

//玩家使用大招技能状态
public class PlayerMagicTrickBState : PlayerState
{

    public PlayerMagicTrickBState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.MAGICTRICKB;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("magicTrickB enter=============");
        base.enter(values);
        _enitity.isAttacking = true;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.showHpSlider();
        }
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("magicTrickB exit=============");
        _enitity.isAttacking = false;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.hideHpSlider();
        }
    }

}

public class PlayerMagicTrickCState : PlayerState
{

    public PlayerMagicTrickCState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.MAGICTRICKC;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("magicTrickC enter=============");
        base.enter(values);
        _enitity.isAttacking = true;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.showHpSlider();
        }

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("magicTrickC exit=============");
        _enitity.isAttacking = false;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.hideHpSlider();
        }
    }

}

public class PlayerMagicTrickDState : PlayerState
{

    public PlayerMagicTrickDState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = PlayerStateEnum.MAGICTRICKD;
    }

    override public void enter(params object[] values)
    {
        Debug.Log("magicTrickD enter=============");
        base.enter(values);
        _enitity.isAttacking = true;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.showHpSlider();
        }
    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("magicTrickD exit=============");
        _enitity.isAttacking = false;
        if (_enitity.attackTarget != null)
        {
            _enitity.attackTarget.hideHpSlider();
        }
    }

}
