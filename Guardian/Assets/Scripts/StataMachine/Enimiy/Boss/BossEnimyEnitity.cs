using System.Collections;
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
        _stateMachine.setCurrentState(enimyIdleState);
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
}
