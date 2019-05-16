
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

public class HeroAttackByET : MonoBehaviour {


    private PlayerEnitity _playerEnitity;
    private HeroAttack _attack;


    //利用event 进行多播委托
    private static event HeroAttackInputHandle _heroAttackInputHandle;
    public static HeroAttackByET instance;

    void Awake()
    {
        instance = this;
        _heroAttackInputHandle += heroAttackInputByET;
    }


    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }

    void heroAttackInputByET(PlayerStateEnum stateEnum)
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
            case PlayerStateEnum.MAGICTRICKC:
                _attack.heroMagicTrickC();
                break;
            case PlayerStateEnum.MAGICTRICKD:
                _attack.heroMagicTrickD();
                break;
            default:
                break;
        }
    }

    public void responseNormalAttack()
    { 
        if (_heroAttackInputHandle!=null)
        {
            _heroAttackInputHandle(PlayerStateEnum.NORMALATTACK);
        }
    }

    public void responseMagicTrickA()
    {
        if (_heroAttackInputHandle != null)
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKA);
        }
    }

    public void responseMagicTrickB()
    {
        if (_heroAttackInputHandle != null)
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKB);
        }
    }

    public void responseMagicTrickC()
    {
        if (_heroAttackInputHandle != null)
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKC);
        }
    }

    public void responseMagicTrickD()
    {
        if (_heroAttackInputHandle != null)
        {
            _heroAttackInputHandle(PlayerStateEnum.MAGICTRICKD);
        }
    }
	
}
