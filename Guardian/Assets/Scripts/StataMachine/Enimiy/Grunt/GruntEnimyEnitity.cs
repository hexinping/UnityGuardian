using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class GruntEnimyEnitity : EnimyEnitity
{

    public GruntEnimyEnitity()
    {
        hpHeight = 70;
    }

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "grunt";
        objName = "tom_happy";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 2;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 40.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 0.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 9;
        _mode.attackDisSquare = 5;

        _mode.moveSpeed = 6.0f;

    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(24);
        _animationEventDict[GlobalParams.anim_ennimy5_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy5_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy5_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy5_run);
        _animationNameList.Add(GlobalParams.anim_ennimy5_death);
        _animationNameList.Add(GlobalParams.anim_ennimy5_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy5_hurt);
    }

    override public void onDestory()
    {

        cObjList.Clear();
        cObjList.Add("eyes");
        cObjList.Add("Skeletonl_base");

        texList.Clear();
        texList.Add("grunt/base_skeleton_col");
        texList.Add("grunt/base_skeleton_col");

        base.onDestory();

    }
   
}
