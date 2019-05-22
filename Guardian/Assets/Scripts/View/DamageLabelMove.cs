
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


    public Canvas canvas;
    public Text labelTxt;
    private GameObject _uiCamera;
    private GameObject _tarObj;

    private bool _isAction = false;

    public void initUI()
    {
        //设置UI摄像机
        _uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _uiCamera.GetComponent<Camera>();
        canvas.sortingOrder = -2;
    }


    public void setTxtPostion(Vector3 wPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(wPos); 
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out localPos);

        labelTxt.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
    
    }
    public void startMove()
    {
        this.Invoke("setActionState",0.1f);
    }

    void setActionState()
    {
        _isAction = true;
        iTween.MoveTo(labelTxt.gameObject, iTween.Hash(
           "y", 100,
          "easetype", iTween.EaseType.easeInSine,
          "time", 0.5,
           "islocal", true,
          "oncomplete", "moveEndCallBack", //必须是目标身上的方法，这里就是transform.gameObject
          "oncompletetarget", gameObject

       ));
    }

    void moveEndCallBack()
    {
        Debug.Log(GetType() + "/moveEndCallBack");
        _isAction = false;
        gameObject.SetActive(false);
    }

    public void setTarget(GameObject obj)
    {
        _tarObj = obj;

    }
	// Update is called once per frame
	void Update () {
        if (_tarObj != null && _isAction == false)
        {
            setTxtPostion(_tarObj.transform.position);
        }
	}

    public void setTextValue(float value)
    {
        labelTxt.text = value.ToString();
    }
}
