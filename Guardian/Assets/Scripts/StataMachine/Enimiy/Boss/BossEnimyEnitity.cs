using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class BossEnimyEnitity : EnimyEnitity
{
    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "Boss_Bruce";
        objName = "bruceObj";
        _mode.filePre = "Models/";
        _mode.file = _mode.filePre + _mode.type + "/" + objName;
        intDatas();
    }


    override public void addBaseState()
    {

        //状态机
        BaseState enimyIdleState            = new BossEnimyIdleState(this);
        BaseState enimyRunState             = new BossEnimyRunState(this);
        BaseState enimyDeadState            = new BossEnimyDeadState(this);
        BaseState enimyNormalAttackState    = new BossEnimyNormalAttackState(this);
        BaseState enimyHurtState            = new BossEnimyHurtState(this);
        BaseState enimySkillState           = new BossEnimySkillState(this);

        _stateList.Add(enimyIdleState);
        _stateList.Add(enimyRunState);
        _stateList.Add(enimyDeadState);
        _stateList.Add(enimyNormalAttackState);
        _stateList.Add(enimyHurtState);
        _stateList.Add(enimySkillState);

        //状态机设置
        _stateMachine.setCurrentState(enimyIdleState);
    }
}
