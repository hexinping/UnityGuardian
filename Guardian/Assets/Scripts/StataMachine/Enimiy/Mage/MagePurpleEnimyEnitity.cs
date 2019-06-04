using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class MagePurpleEnimyEnitity : EnimyEnitity
{
    public MagePurpleEnimyEnitity()
    {
        hpHeight = 110;
    }

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "mage";
        objName = "mage_purple";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 5;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 40.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 2.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 64;
        _mode.attackDisSquare = Random.Range(9, 25);

        _mode.moveSpeed = 2.0f;
    
    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(18);
        _animationEventDict[GlobalParams.anim_ennimy2_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy2_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy2_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy2_run);
        _animationNameList.Add(GlobalParams.anim_ennimy2_death);
        _animationNameList.Add(GlobalParams.anim_ennimy2_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy2_hurt);
    }

    override public void onDestory()
    {
        mainTexturePath = "mage/mage_skeleton_col_variant2";
        cObjList = new object[] { "cloak", "eyes", "Skeletonl_base", "staff" };
        base.onDestory();

    }
   
}
