using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class MageGreenEnimyEnitity : MagePurpleEnimyEnitity
{

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "mage";
        objName = "mage_green";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void intDatas()
    {
        _mode.maxAtk = 3;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 40.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 3.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 49;
        _mode.attackDisSquare = Random.Range(9, 25);

        _mode.moveSpeed = 3.0f;

    }

    override public void onDestory()
    {
        mainTexturePath = "mage/mage_skeleton_col_variant1";
        cObjList = new object[] { "cloak", "eyes", "Skeletonl_base", "staff" };
        base.onDestory();

    }
   
}
