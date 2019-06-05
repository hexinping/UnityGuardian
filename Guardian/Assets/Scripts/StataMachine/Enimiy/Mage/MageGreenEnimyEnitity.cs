using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class MageGreenEnimyEnitity : EnimyEnitity
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
      
        cObjList.Clear();
        cObjList.Add("cloak");
        cObjList.Add("eyes");
        cObjList.Add("Skeletonl_base");
        cObjList.Add("staff");

        texList.Clear();
        texList.Add("mage/mage_skeleton_col_variant5");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("mage/mage_skeleton_col_variant5");
     
        base.onDestory();

    }
   
}
