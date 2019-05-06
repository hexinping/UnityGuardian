
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
        switch (stateEnum)
        {
            case PlayerStateEnum.NORMALATTACK:
                heroNormalAttack();
                break;
            case PlayerStateEnum.MAGICTRICKA:
                heroMagicTrickA();
                break;
            case PlayerStateEnum.MAGICTRICKB:
                heroMagicTrickB();
                break;
            default:
                break;
        }
    }

    void heroNormalAttack()
    {
        //普通攻击
        //多个攻击动作区分或者整合 todo
        print(GetType() + "/heroNormalAttack Attack");
    }

    void heroMagicTrickA()
    {
        //普通技能
        print(GetType() + "/heroMagicTrickA MagicTrickA");
    }

    void heroMagicTrickB()
    {
        //大招技能
        print(GetType() + "/heroMagicTrickB MagicTrickB");
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
