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
    }

    override public void onDestory()
    {
        base.onDestory();
    }
   

}
