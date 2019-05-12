
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
        _attack = this.GetComponent<HeroAttack>();
        if (_attack == null) return;
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

        if (Input.GetButtonDown(GlobalParams.NormalAttack))
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
	}
}
