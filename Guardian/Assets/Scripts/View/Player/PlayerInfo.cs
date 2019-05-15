
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

    private GameObject _easyTouchObj;
	// Use this for initialization
	void Start () {
        _easyTouchObj = GameObject.Find("_Environment").transform.Find("EasyTouch").gameObject;
        
	}


    public void clickHeadIcon()
    {
        Debug.Log(GetType() + "/clickHeadIcon===");

        string prefabName = "Prefabs/View/PlayerInfoDetail";
        GameObject obj = (GameObject)Resources.Load(prefabName);
        GameObject objClone = GameObject.Instantiate(obj);
        objClone.transform.parent = this.gameObject.transform;
        objClone.name = "PlayerInfoDetail";
        objClone.transform.localPosition = new Vector3(0,0,0);
        _easyTouchObj.SetActive(false);
    }

    public void clickSettingBtn()
    {
        Debug.Log(GetType() + "/clickSettingBtn===");
    }

    public void clickExitBtn()
    {
        Debug.Log(GetType() + "/clickExitBtn===");
    }
	
}
