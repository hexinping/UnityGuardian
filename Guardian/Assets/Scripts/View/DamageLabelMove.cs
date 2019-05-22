
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

public class DamageLabelMove : MonoBehaviour {


    private Vector3 _originPos;
    private Text _labelTxt;
    private GameObject _uiCamera;

    //public GameObject txtObj;
	// Use this for initialization
	void Start () 
    {
        initUI();
        //gameObject.transform.localPosition = Vector3.zero;

       
	}

    void initUI()
    {
        //设置UI摄像机
        _uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        GameObject parentObj = gameObject.transform.parent.gameObject;
        Canvas canvas = parentObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _uiCamera.GetComponent<Camera>();

        canvas.sortingOrder = -1;

        _labelTxt = GetComponent<Text>();

    }


    public void setTxtPostion(Vector3 pos)
    {
        if (_labelTxt == null)
        { 
            _labelTxt = GetComponent<Text>();
        }
        _labelTxt.rectTransform.position = new Vector3(pos.x, pos.y, 0);
    
    }
    public void startMove(Vector3 tarPostion)
    {
        //RectTransform rectTrans = GetComponent<RectTransform>();
        //Vector3 oldPos = rectTrans.position;
        //float targetY = 1;                                           //为什么这个是米的概念
        //Vector3 targetPos = new Vector3(oldPos.x, oldPos.y + 1, oldPos.z);
        iTween.MoveTo(gameObject, iTween.Hash(
                   //"y", targetY,
                  "position", tarPostion,
                  "easetype", iTween.EaseType.easeInSine,
                  "time", 0.5,
                   "islocal",true,
                  "oncomplete", "moveEndCallBack" //必须是目标身上的方法，这里就是transform.gameObject

              ));
    }

    void moveEndCallBack()
    {
        Debug.Log(GetType() + "/moveEndCallBack");
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void setTextValue(float value)
    {
        _labelTxt.text = value.ToString();
    }
}
