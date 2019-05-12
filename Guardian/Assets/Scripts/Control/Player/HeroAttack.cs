
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
	// Use this for initialization
	void Start () {
		
	}

    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }
	// Update is called once per frame
	void Update () {
		
	}


    public void heroNormalAttack()
    {
        //普通攻击
        //多个攻击动作区分或者整合 todo
        print(GetType() + "/heroNormalAttack Attack");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.NORMALATTACK, 1.0f, false);
        }
    }

    public void heroMagicTrickA()
    {
        //普通技能
        print(GetType() + "/heroMagicTrickA MagicTrickA");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKA, 1.0f, false);
        }
    }

    public void heroMagicTrickB()
    {
        //大招技能
        print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKB, 1.0f, false);
        }
    }
}
