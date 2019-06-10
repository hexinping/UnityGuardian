
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
using kernal;

public class DamageLabelMove : MonoBehaviour {


    public Canvas canvas;
    public Text labelTxt;
    private GameObject _uiCamera;
    private GameObject _tarObj;
    private float _offsetY = 0.0f;    //起始位置偏移量

    public void initUI()
    {
        //设置UI摄像机
        _uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _uiCamera.GetComponent<Camera>();
        canvas.sortingOrder = GlobalParams.DamageLabelOrder;
    }


    public void setTxtPostion(Vector3 wPos)
    {

        Vector3 screenPos = Camera.main.WorldToScreenPoint(wPos); 
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out localPos);

        labelTxt.transform.localPosition = new Vector3(localPos.x + Random.Range(-80.0f, 80.0f), localPos.y + _offsetY, 0);
    
    }
    public void startMove(GameObject targetObj, float offsetY)
    {
        _offsetY = offsetY;
        initUI();
        setTarget(targetObj);
        setTxtPostion(_tarObj.transform.position);
        setActionState();
    }

    void setActionState()
    {
        Vector3 targetPos = labelTxt.transform.localPosition + new Vector3(0, 150, 0);
        iTween.MoveTo(labelTxt.gameObject, iTween.Hash(
           //"y", 200,
           "position",targetPos,
          "easetype", iTween.EaseType.easeOutQuad,
          "time", 1.0,
           "islocal", true,
          "oncomplete", "moveEndCallBack",
          "oncompletetarget", gameObject

       ));
    }

    void moveEndCallBack()
    {
        //Debug.Log(GetType() + "/moveEndCallBack");
        //加入到缓冲池的非活动集合
        PoolManager.PoolsArray[GlobalParams.DamageLabelPool].RecoverGameObjectToPools(gameObject);
    }


    public void setTarget(GameObject obj)
    {
        _tarObj = obj;

    }

    public void setTextValue(float value)
    {
        labelTxt.text = value.ToString();
    }
}
