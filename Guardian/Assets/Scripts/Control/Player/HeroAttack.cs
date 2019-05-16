
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
    private bool isSingleA = false;
    private bool isSingleB = false;
    private bool isSingleC = false;
    private bool isSingleD = false;
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }


    public void heroNormalAttack(bool isCheckLongPrees = true)
    {
        if (isCheckLongPrees)
        {
            if (_lastPressTime == 0.0f)
            {
                _lastPressTime = Time.time;
            }
            if (Time.time - _lastPressTime >= delayTime)
            {
                _isLongPrees = true;
            }
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
        if (isSingleA) return;
        if (!isSingleA)
        {
            isSingleA = true;
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
        if (isSingleB) return;
        if (!isSingleB)
        {
            isSingleB = true;
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
        if (isSingleC) return;
        if (!isSingleC)
        {
            isSingleC = true;
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
        if (isSingleD) return;
        if (!isSingleD)
        {
            isSingleD = true;
        }
        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKD, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKD);

        }
    }

    IEnumerator resetIdleState(float delayTime, PlayerStateEnum preEnum)
    {
        yield return new WaitForSeconds(delayTime);
        if(_playerEnitity != null)
        {
            if (!_isLongPrees)
            {
                _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
              
            }

            switch (preEnum)
            {
                case PlayerStateEnum.NORMALATTACK:
                    isSingle = false;
                    break;
                case PlayerStateEnum.MAGICTRICKA:
                    isSingleA = false;
                    break;
                case PlayerStateEnum.MAGICTRICKB:
                    isSingleB = false;
                    break;
                case PlayerStateEnum.MAGICTRICKC:
                    isSingleC = false;
                    break;
                case PlayerStateEnum.MAGICTRICKD:
                    isSingleD = false;
                    break;
                default:
                    break;
            }    
        }
    }

    public void startResetIdle(PlayerStateEnum playstateEM)
    {
        if (_resetIdleCor != null)
        {
            StopCoroutine(_resetIdleCor);
        }
        
        float time = _playerEnitity.getAnimaitionPlayTime(playstateEM);
        _resetIdleCor = StartCoroutine(resetIdleState(time, playstateEM));
    }
}
