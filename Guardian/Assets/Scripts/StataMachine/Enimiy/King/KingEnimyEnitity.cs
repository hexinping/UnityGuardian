using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class KingEnimyEnitity : EnimyEnitity
{

    public KingEnimyEnitity()
    {
        hpHeight = 160;
    }

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "king";
        objName = "king_green";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        attackSoundFile = GlobalParams.sound_ennimy_attack;
        intDatas();
    }

    override public void addBaseState()
    {
        //状态机
        BaseState enimyIdleState = new EnimyIdleState(this);
        BaseState enimyRunState = new EnimyRunState(this);
        BaseState enimyDeadState = new EnimyDeadState(this);
        BaseState enimyNormalAttackState = new KingEnimyNormalAttackState(this);  //king的攻击状态单独写
        BaseState enimyHurtState = new EnimyHurtState(this);

        _stateList.Add(enimyIdleState);
        _stateList.Add(enimyRunState);
        _stateList.Add(enimyDeadState);
        _stateList.Add(enimyNormalAttackState);
        _stateList.Add(enimyHurtState);

        //状态机设置
        _stateMachine.setCurrentState(enimyIdleState);
    }


    override public string getAnimationName(EnimyStateEnum state, int commbex = 0)
    {
        int index = (int)state;
        if (commbex > 0)
        {
            if (commbex == 1)
            {
               return  _animationNameList[3];
            }
            else if (commbex == 2)
            {
                return _animationNameList[5];
            }
            else if (commbex == 3)
            { 
                return _animationNameList[6];
            }

        }
        return _animationNameList[index];
    }

 
    override public void intDatas()
    {
        _mode.maxAtk = 10;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 100.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 0.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 100;
        _mode.attackDisSquare = 9;

        _mode.moveSpeed = 2.0f;

    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(21);
        _animationEventDict[GlobalParams.anim_ennimy4_normalAttack] = attack1List;

        List<int> attack2List = new List<int>();
        attack2List.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy4_normalAttack1] = attack2List;

        List<int> attack3List = new List<int>();
        attack3List.Add(16);
        _animationEventDict[GlobalParams.anim_ennimy4_normalAttack2] = attack3List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy4_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy4_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy4_run);
        _animationNameList.Add(GlobalParams.anim_ennimy4_death);
        _animationNameList.Add(GlobalParams.anim_ennimy4_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy4_hurt);

        //额外攻击动作
        _animationNameList.Add(GlobalParams.anim_ennimy4_normalAttack1);
        _animationNameList.Add(GlobalParams.anim_ennimy4_normalAttack2);
    }

    override public void onDestory()
    {

        cObjList.Clear();
        cObjList.Add("armor");
        cObjList.Add("cloak");
        cObjList.Add("eyes");
        cObjList.Add("Skeletonl_base");
        cObjList.Add("sword");

        texList.Clear();
        texList.Add("king/SKELETONKING_ARMOR_Variant1");
        texList.Add("king/WEAPON_Variant5");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("king/WEAPON_Variant5");
        base.onDestory();

    }
   
}
