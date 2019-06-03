﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class KingEnimyEnitity : EnimyEnitity
{

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "king";
        objName = "king_green";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 10;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 100.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 0.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 64;
        _mode.attackDisSquare = 9;

        _mode.moveSpeed = 2.0f;

    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(21);
        _animationEventDict[GlobalParams.anim_ennimy5_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy5_hurt] = hurtList;
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
    }

    override public void onDestory()
    {
        mainTexturePath = "king/SKELETONKING_ARMOR_Variant1";
        cObjList = new object[] { "armor", "cloak", "eyes", "Skeletonl_base","sword"};
        base.onDestory();

    }
   
}
