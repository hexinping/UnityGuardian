
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

public class PlayerInfo : MonoBehaviour {

    public Slider hpSlider;
    public Slider magicSlider;
    public Text   hpTxt;
    public Text   magicTxt;
    public Text levelTxt;
    public Text expTxt;
    public Text coinTxt;
    public Text diamondTxt;

    private PlayerEnitity _playerEnitity;
    private PlayerManager _playerManager;

    private PlayerInfoDetail _playerInfoDetail;
    void Start()
    {
        _playerManager = PlayerManager.getInstance();

        updateLevel();
        updateExp();
        updateCoin();
        updateDaimond();
    }


    public void clickHeadIcon()
    {
        Debug.Log(GetType() + "/clickHeadIcon===");

        string prefabName = "View/PlayerInfoDetail";
        GameObject obj = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, prefabName, "LevelOneView");
        GameObject objClone = GameObject.Instantiate(obj);
        objClone.transform.parent = this.gameObject.transform;
        objClone.name = "PlayerInfoDetail";
        objClone.transform.localPosition = new Vector3(0,0,0);

        _playerInfoDetail = objClone.GetComponent<PlayerInfoDetail>();
        _playerInfoDetail.setPlayerData(levelTxt.text, expTxt.text, coinTxt.text, diamondTxt.text);
        if (_playerEnitity != null)
        {
            _playerInfoDetail.setPlayerEnitiy(_playerEnitity);
        }

    }

    public void clickSettingBtn()
    {
        Debug.Log(GetType() + "/clickSettingBtn===");
    }

    public void clickExitBtn()
    {
        Debug.Log(GetType() + "/clickExitBtn===");
    }


    public void setPlayerEnitiy(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }

    public void updateLevel()
    {
        int level = _playerManager.getPlayerLevel();
        levelTxt.text = level.ToString();
    }

    public void updateExp()
    {
        int exp = _playerManager.getPlayerExp();
        expTxt.text = exp.ToString();
    }


    public void updateCoin()
    {
        int coinNum = _playerManager.getPlayerCoin();
        coinTxt.text = coinNum.ToString();
    }

    public void updateDaimond()
    {
        int diamondNum = _playerManager.getPlayerDiamond();
        diamondTxt.text = diamondNum.ToString();
    }


    public void updateHpSlider()
    { 
       float curHp = _playerEnitity._mode.hp;
       float maxHp = _playerEnitity._mode.maxHp;
       hpSlider.value = curHp / maxHp;

       hpTxt.text = curHp.ToString() + "/" + maxHp.ToString();
    }

    public void updateMagicSlider()
    {
        //todo
    }
	
}
