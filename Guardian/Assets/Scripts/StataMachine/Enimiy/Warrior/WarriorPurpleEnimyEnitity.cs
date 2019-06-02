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
    }
    override public void onDestory()
    {
        BurnHelper burn = _gameObject.AddComponent<BurnHelper>();
        Texture mainT = (Texture)ResourcesManager.getInstance().getResouce(ResourceType.Texture, "Models/Enemys/Skeleton_Pack/Textures/mage/mage_skeleton_col_variant2", rootView._name, true, false);
        burn.setMainTex(mainT);
        burn.setNameList("cloak", "eyes", "Skeletonl_base", "staff");

        changeStateByIndex(EnimyStateEnum.DEAD);
        LevelOneView view = (LevelOneView)rootView;
        view.removeFromEnimyList(this);
    }
   
}
