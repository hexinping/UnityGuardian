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
        intDatas();
    }
    override public void onDestory()
    {
        mainTexturePath = "warrior/skeleton_warrior__variant2";
        base.onDestory();
    }
    override public void intDatas()
    {
        _mode.moveSpeed = 3.0f;
    }
}
