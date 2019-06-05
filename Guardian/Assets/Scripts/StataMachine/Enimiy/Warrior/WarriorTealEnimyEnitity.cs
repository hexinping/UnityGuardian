using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class WarriorTealEnimyEnitity : EnimyEnitity{

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_teal";
        _mode.file = _mode.file + objName;
        attackSoundFile = GlobalParams.sound_ennimy_attack;
    }

    override public void onDestory()
    {
        texList[0] = "warrior/skeleton_warrior_variant1";
        base.onDestory();
    }
   

}
