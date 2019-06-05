using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class WarriorPurpleEnimyEnitity : EnimyEnitity
{

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_purple";
        _mode.file = _mode.file + objName;
        attackSoundFile = GlobalParams.sound_ennimy_attack;
        intDatas();
    }
    override public void onDestory()
    {

        texList[0] = "warrior/skeleton_warrior__variant2";
        base.onDestory();
    }
    override public void intDatas()
    {
        _mode.moveSpeed = 3.0f;
    }
}
