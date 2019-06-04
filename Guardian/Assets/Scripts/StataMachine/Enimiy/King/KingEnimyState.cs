using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人KingNormalAttack状态
public class KingEnimyNormalAttackState : EnimyState
{
    public float endPlayTime = 0.0f;
    public float playTotalTime = 0.0f;
    public Transform targetTransform;
    public Transform selfTransform;


    //attack_slice  attack_cleave attack_stab
    public static int commbexIndex = 1;
    public KingEnimyNormalAttackState(BaseEnitity enity)
        : base(enity)
    {
        stateIndex = EnimyStateEnum.NORMALATTACK;
       
    }

    void addAttackEvent(EnimyEnitity e, int commbexIndex)
    {
        e.addDelayCall(stateIndex, 1.0f, commbexIndex);
        if (commbexIndex == 1)
        {
            e._animator.Play(GlobalParams.state_enimyAttack, 0, 0.0f); //播放动画
        }
        else if (commbexIndex == 2)
        {
            e._animator.Play(GlobalParams.state_enimyAttackEx1, 0, 0.0f); //播放动画
        }
        else if (commbexIndex == 3)
        {
            e._animator.Play(GlobalParams.state_enimyAttackEx2, 0, 0.0f); //播放动画
        }

        //下一次播放时间
        int intevalFrameCount = 0; //攻击时间间隔 2帧时间
        string animName = e.getAnimationName(stateIndex, commbexIndex);
        float time = e.getClipTotalLength(e._animator, animName, intevalFrameCount);
        float p = GlobalParams.totalTime + time;
        endPlayTime = p;
        playTotalTime = time;
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
        targetTransform = _enitity.attackTarget._gameObject.transform;
        selfTransform = _enitity._gameObject.transform;



        AudioManager.getInstance().playSoundEffect("1_LightSword_SwordHero");
        EnimyEnitity e = (EnimyEnitity)_enitity;
        addAttackEvent(e, commbexIndex);
        if (commbexIndex == 1)
        {
            commbexIndex = 2;
        }
        else if(commbexIndex == 2)
        {
            commbexIndex = 3;
        }
        else if (commbexIndex == 3)
        {
            commbexIndex = 1;
        }

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
            //如果攻击目标脱离了我的攻击范围，结束攻击重新寻敌
            BaseEnitity target = _enitity.attackTarget;
            if (target != null)
            {
                float dis = (targetTransform.position - selfTransform.position).sqrMagnitude;  //距离的平方
                float attDis = _enitity.getAttackDis();
                if (dis > attDis)
                {
                    _enitity.isAttacking = false;
                    e.changeStateByIndex(EnimyStateEnum.IDLE);
                    return;
                }
            }

            //面向目标
            _enitity.faceToTarget();
            if (GlobalParams.totalTime >= endPlayTime)
            {
                addAttackEvent(e, commbexIndex);
                if (commbexIndex == 1)
                {
                    commbexIndex = 2;
                }
                else if (commbexIndex == 2)
                {
                    commbexIndex = 3;
                }
                else if (commbexIndex == 3)
                {
                    commbexIndex = 1;
                }

                AudioManager.getInstance().playSoundEffect("1_LightSword_SwordHero");
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

        commbexIndex = 1;
        endPlayTime = 0.0f;
        playTotalTime = 0.0f;
    }

}


