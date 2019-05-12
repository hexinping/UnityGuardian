
/***
 *
 *  Title: "Guardian" 项目
 *         描述：
 *
 *  Description:
 *        功能：
 *       
 *
 *  Date: 2019
 * 
 *  Version: 1.0
 *
 *  Modify Recorder:
 *     
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAttack : MonoBehaviour {

    private PlayerEnitity _playerEnitity;
    private Coroutine _resetIdleCor;
	// Use this for initialization
	
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }


    public void heroNormalAttack()
    {
        //普通攻击
        //多个攻击动作区分或者整合 todo
        print(GetType() + "/heroNormalAttack Attack");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.NORMALATTACK, 1.0f, false);
            startResetIdle(PlayerStateEnum.NORMALATTACK);
        }
    }

    public void heroMagicTrickA()
    {
        //普通技能
        print(GetType() + "/heroMagicTrickA MagicTrickA");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKA, 1.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKA);
        }
    }

    public void heroMagicTrickB()
    {
        //大招技能
        print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKB, 1.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKB);
        }
    }

    IEnumerator resetIdleState(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if(_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
        }
    }

    void startResetIdle(PlayerStateEnum playstateEM)
    {
        if (_resetIdleCor != null)
        {
            StopCoroutine(_resetIdleCor);
        }
        float time = _playerEnitity.getAnimaitionPlayTime(playstateEM);
        _resetIdleCor = StartCoroutine(resetIdleState(time));
    }
}
