
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
        if (stateEnum == PlayerStateEnum.NORMALATTACK)
        {   
            //普通攻击
            //多个攻击动作区分或者整合 todo
            print(GetType() + "/heroAttackInputByKey Attack");
        }
        else if (stateEnum == PlayerStateEnum.MAGICTRICKA)
        {
            //普通技能
            print(GetType() + "/heroAttackInputByKey MagicTrickA");
        }
        else if (stateEnum == PlayerStateEnum.MAGICTRICKB)
        {
            //大招技能
            print(GetType() + "/heroAttackInputByKey MagicTrickB");
        }
    }
	// Update is called once per frame
	void Update () {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

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
