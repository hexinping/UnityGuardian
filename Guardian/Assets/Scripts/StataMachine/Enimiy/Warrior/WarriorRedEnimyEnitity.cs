using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class WarriorRedEnimyEnitity : EnimyEnitity {

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_red";
        _mode.file = _mode.file + objName;
        attackSoundFile = GlobalParams.sound_ennimy_attack;
    }
   override public void onDestory()
    {
        mainTexturePath = "warrior/skeleton_warrior_col_01";
        base.onDestory();
    }
   
}
