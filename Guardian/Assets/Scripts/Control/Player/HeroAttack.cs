
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


/*
    持续按 ，普通按
 */
public class HeroAttack : MonoBehaviour {

    private PlayerEnitity _playerEnitity;
    private Coroutine _resetIdleCor;


    private float delayTime = 2.0f;
    public float _lastPressTime = 0.0f;
    public bool _isLongPrees = false;

    private bool isSingle = false;
	
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }


    public void heroNormalAttack()
    {
        if (_lastPressTime == 0.0f)
        {
            _lastPressTime = Time.time;
        }
        if (Time.time - _lastPressTime >= delayTime)
        {
            _isLongPrees = true;
        }
        if (isSingle) return;
        if (!isSingle)
        {
            isSingle = true;
        }
        //普通攻击
        //多个攻击动作区分或者整合 todo
        //print(GetType() + "/heroNormalAttack Attack");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.NORMALATTACK, 2.0f, false);
            startResetIdle(PlayerStateEnum.NORMALATTACK);
            _playerEnitity.AddComobIndex();
        }
    }

    public void heroMagicTrickA()
    {
        if (isSingle) return;
        if (!isSingle)
        {
            isSingle = true;
        }
        //普通技能
        //print(GetType() + "/heroMagicTrickA MagicTrickA");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKA, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKA);
        }
    }

    public void heroMagicTrickB()
    {
        if (isSingle) return;
        if (!isSingle)
        {
            isSingle = true;
        }
        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKB, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKB);
           
        }
    }

    public void heroMagicTrickC()
    {
        if (isSingle) return;
        if (!isSingle)
        {
            isSingle = true;
        }
        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKC, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKC);

        }
    }

    public void heroMagicTrickD()
    {
        if (isSingle) return;
        if (!isSingle)
        {
            isSingle = true;
        }
        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKD, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKD);

        }
    }

    IEnumerator resetIdleState(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if(_playerEnitity != null)
        {
            if (!_isLongPrees)
            {
                _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
              
            }
            isSingle = false;
           
            
        }
    }

    public void startResetIdle(PlayerStateEnum playstateEM)
    {
        if (_resetIdleCor != null)
        {
            StopCoroutine(_resetIdleCor);
        }
        
        float time = _playerEnitity.getAnimaitionPlayTime(playstateEM);
        _resetIdleCor = StartCoroutine(resetIdleState(time));
    }
}
