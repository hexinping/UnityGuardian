
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

public class PlayerInfoDetail : MonoBehaviour {


    private GameObject _easyTouchObj;


    public Text levelTxt;
    public Text hpTxt;
    public Text magicTxt;
    public Text expTxt;
    public Text atkTxt;
    public Text defenceTxt;
    public Text dexterityTxt;
    public Text killEnimiyTxt;
    public Text diamondTxt;
    public Text coinTxt;

    private PlayerEnitity _playerEnitity;

    void Start()
    {
        _easyTouchObj = GameObject.Find("_Environment").transform.Find("EasyTouch").gameObject;
        _easyTouchObj.SetActive(false);


        updateAtk();
        updateDefence();
        updateDexterity();
        updateHP();
        updateMagic();

    }

   public void clickCloseBtn()
   {
        Destroy(this.gameObject);
        _easyTouchObj.SetActive(true);
   }

   public void setPlayerData(string level, string exp, string coin, string diamond)
   {
       levelTxt.text = level;
       expTxt.text = exp;
       coinTxt.text = coin;
       diamondTxt.text = diamond;
    
   }

   public void setPlayerEnitiy(PlayerEnitity enitity)
   {
       _playerEnitity = enitity;
   }

   public void updateAtk()
   {
       float atk = _playerEnitity.getAtkValue();
       atkTxt.text = atk.ToString();
   }

   public void updateDefence()
   {
       float defence = _playerEnitity.getDefenceValue();
       defenceTxt.text = defence.ToString();
   }

   public void updateDexterity()
   {
       float dexterity = _playerEnitity.getDexterityValue();
       dexterityTxt.text = dexterity.ToString();
   }

   public void updateHP()
   {
       float curHp = _playerEnitity.getHpValue();
       float maxHp = _playerEnitity._mode.maxHp;
       hpTxt.text = curHp.ToString() + "/" + maxHp.ToString();
   }


   public void updateMagic()
   {
       float curMagic = _playerEnitity._mode.magic;
       float maxMagic = _playerEnitity._mode.maxMagic;
       magicTxt.text = curMagic.ToString() + "/" + maxMagic.ToString();
   }

   public void updatekillEnimiy()
   {

       killEnimiyTxt.text = "0";
   }
}
