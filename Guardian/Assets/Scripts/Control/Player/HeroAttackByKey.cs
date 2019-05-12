
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

public class HeroAttackByKey : MonoBehaviour {

    private PlayerEnitity _playerEnitity;
    private HeroAttack _attack;

    //利用event 进行多播委托
    private static event HeroAttackInputHandle _heroAttackInputHandle;

    void Awake()
    {
        _heroAttackInputHandle += heroAttackInputByKey;
    }
	// Use this for initialization
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }

    void heroAttackInputByKey(PlayerStateEnum stateEnum)
    {
        
        if (_attack == null)
        {
            _attack = this.GetComponent<HeroAttack>();
        }
        switch (stateEnum)
        {
            case PlayerStateEnum.NORMALATTACK:
                _attack.heroNormalAttack();
                break;
            case PlayerStateEnum.MAGICTRICKA:
                _attack.heroMagicTrickA();
                break;
            case PlayerStateEnum.MAGICTRICKB:
                _attack.heroMagicTrickB();
                break;
            default:
                break;
        }
    }


	// Update is called once per frame
	void Update () {
        if (_playerEnitity != null && _playerEnitity.isMove)
        {
            return;
        }
        if (Input.GetButton(GlobalParams.NormalAttack))
        {
            _heroAttackInputHandle(PlayerStateEnum.NORMALATTACK);
        }
        else if (Input.GetButtonDown(GlobalParams.MagicTrickA))
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKA);
        }
        else if (Input.GetButtonDown(GlobalParams.MagicTrickB))
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKB);
        }
        else
        {
            if (_playerEnitity.isAttacking)
            {
                if (_attack != null)
                {
                    _attack._lastPressTime = 0.0f;
                    if (_attack._isLongPrees)
                    {
                        _attack._isLongPrees = false;

                        Debug.Log("攻击长按 退出===========");
                        _playerEnitity.reduceComobIndex();
                        _attack.startResetIdle(PlayerStateEnum.NORMALATTACK);
                        _playerEnitity.AddComobIndex();
                       
                    }
                }
            }

        }
	}
}
