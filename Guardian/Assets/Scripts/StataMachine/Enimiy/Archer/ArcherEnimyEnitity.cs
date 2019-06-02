using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class ArcherEnimyEnitity : EnimyEnitity
{


    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "archer";
        objName = "archer_green";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 1;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 30.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 0.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 100;
        _mode.attackDisSquare = 81;

        _mode.moveSpeed = 3.0f;

    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(35);
        _animationEventDict[GlobalParams.anim_ennimy3_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy3_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy3_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy3_run);
        _animationNameList.Add(GlobalParams.anim_ennimy3_death);
        _animationNameList.Add(GlobalParams.anim_ennimy3_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy3_hurt);
    }

    override public void onDestory()
    {
        mainTexturePath = "archer/skel_archer_col_green";
        cObjList = new object[] { "armor", "eyes", "Skeletonl_base", "bow" };
        base.onDestory();

    }
}
