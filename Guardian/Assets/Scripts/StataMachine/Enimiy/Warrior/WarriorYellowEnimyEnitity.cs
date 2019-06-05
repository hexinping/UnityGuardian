using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class WarriorYellowEnimyEnitity : EnimyEnitity {


    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_yellow";
        _mode.file = _mode.file + objName;
        attackSoundFile = GlobalParams.sound_ennimy_attack;
    }

    override public void onDestory()
    {
        texList[0] = "warrior/skeleton_warrior__variant3";
        base.onDestory();
    }
   

}
