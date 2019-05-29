using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class WarriorPurpleEnimyEnitity : EnimyEnitity {

    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        objName = "warrior_purple";
        _mode.file = _mode.file + objName;
    }
   
}
