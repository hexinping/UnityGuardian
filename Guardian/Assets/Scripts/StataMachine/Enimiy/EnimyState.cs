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

        _enitity.isMove = false;
        _enitity.isAttacking = false;
        _enitity.isDead = false;
        _enitity.isHurt = false;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isMove", _enitity.isMove);
            _enitity._animator.SetBool("isAttack", _enitity.isAttacking);
            _enitity._animator.SetBool("isHurt", _enitity.isHurt);
            _enitity._animator.SetBool("isDead", _enitity.isDead);
        }


    }

    override public void excute(params object[] values)
    {
        EnimyEnitity e = (EnimyEnitity)_enitity;
        if (_enitity.isMove)
        {
            //切换到移动状态
          
           e.changeStateByIndex(EnimyStateEnum.RUN);
        }
        else if (_enitity.isAttacking)
        {
            e.changeStateByIndex(EnimyStateEnum.NORMALATTACK);
        }
        else if (_enitity.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
        }
        else if (_enitity.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }

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

        _enitity.isMove = true;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isMove", _enitity.isMove);
        }

        _enitity._animator.Play("Run", 0, 0.0f); //播放动画

    }

    override public void excute(params object[] values)
    {
        EnimyEnitity e = (EnimyEnitity)_enitity;
        if (!_enitity.isMove)
        {
            //切换到移动状态
            e.changeStateByIndex(EnimyStateEnum.IDLE);
        }
        else if (_enitity.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
        }
        else if (_enitity.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }
        else
        {
            if (_enitity.moveTarget != null)
            { 
                 _enitity.faceToTarget();
                 e.updateMove();
            }
               
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyRunState exit=============");
        _enitity.isMove = false;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isMove", _enitity.isMove);
        }

        _enitity.moveTarget = null;
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

        _enitity.isDead = true;
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isDead", _enitity.isDead);
        }

    }

    override public void excute(params object[] values)
    {

    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyDeadState exit=============");
        _enitity.isDead = false;
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isDead", _enitity.isDead);
        }
    }

}


//敌人NormalAttack状态
public class EnimyNormalAttackState : EnimyState
{
    public float endPlayTime = 0.0f;
    public float playTotalTime = 0.0f;
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

        _enitity.isAttacking = true;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isAttack", _enitity.isAttacking);
        }

        //面向目标
        _enitity.faceToTarget();

        
        EnimyEnitity e = (EnimyEnitity)_enitity;
        e.addDelayCall(stateIndex);  //伤害事件 9-0+1

        AudioManager.getInstance().playSoundEffect("1_LightSword_SwordHero");
        e._animator.Play(GlobalParams.state_enimyAttack, 0, 0.0f); //播放动画
        
        //下一次播放时间
        int intevalFrameCount = 2; //攻击时间间隔 2帧时间
        string animName = e.getAnimationName(stateIndex);
        float time = e.getClipLength(e._animator, animName, 31 + intevalFrameCount); //30-0+1
        float p = GlobalParams.totalTime + time;
        endPlayTime = p;
        playTotalTime = time;
    }

    override public void excute(params object[] values)
    {

        EnimyEnitity e = (EnimyEnitity)_enitity;
        if (!_enitity.isAttacking)
        {
            //切换到移动状态
            e.changeStateByIndex(EnimyStateEnum.IDLE);
        }
        else if (_enitity.isHurt)
        {
            e.changeStateByIndex(EnimyStateEnum.HURT);
  
        }
        else if (_enitity.isDead)
        {
            e.changeStateByIndex(EnimyStateEnum.DEAD);
        }
        else
        {
            //面向目标
            _enitity.faceToTarget();
            if (GlobalParams.totalTime >= endPlayTime)
            {

                e.addDelayCall(stateIndex);
                AudioManager.getInstance().playSoundEffect("1_LightSword_SwordHero");
                e._animator.Play(GlobalParams.state_enimyAttack, 0, 0.0f); //播放动画
                endPlayTime = GlobalParams.totalTime + playTotalTime;
            }
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyNormalAttackState exit=============");
        _enitity.isAttacking = false;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isAttack", _enitity.isAttacking);
        }
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


        
        _enitity.isHurt = true;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isHurt", _enitity.isHurt);
        }
        EnimyEnitity e = (EnimyEnitity)_enitity;
        e.addDelayCall(stateIndex);

        //播放动画
        e._animator.Play(GlobalParams.state_enimyHurt, 0, 0.0f); 
        //播放特效
        e.playHitEffect(GlobalParams.anim_ennimy1_hurt);
    }

    override public void excute(params object[] values)
    {
        EnimyEnitity e = (EnimyEnitity)_enitity;
        if (!_enitity.isHurt)
        {
            //切换到移动状态
            e.changeStateByIndex(EnimyStateEnum.IDLE);
        }
    }


    override public void exit(params object[] values)
    {
        Debug.Log("EnimyHurtState exit=============");
        _enitity.isHurt = false;

        //设置animatorctrl 条件
        if (_enitity._animator != null)
        {
            _enitity._animator.SetBool("isHurt", _enitity.isHurt);
        }
    }

}
