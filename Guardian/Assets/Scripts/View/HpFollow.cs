
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

public class HpFollow : MonoBehaviour {

    private Slider hpSlider;
    private RectTransform rectTrains;

    public Transform target;
    private float value;
    private float maxValue;
    private Vector2 offset;

    private GameObject uiCameraObj;
    private GameObject hpLayer;
    private Camera uiCamera;

    private float delayTime = 0.0f; //延迟显示时间
    private bool isShowHp = true;


    public void setDelayTime(float time)
    {
        delayTime = time;
    }
    
	// Use this for initialization

    public void setHpUIDatas(Vector2 tOffset, float tValue, float tMaxValue)
    {
        offset = tOffset;
        value = tValue;
        maxValue = tMaxValue;
        initUI(); 
    }

    void initUI()
    {
        GameObject hpObj = gameObject.transform.Find("Slider").gameObject;
        hpSlider = hpObj.GetComponent<Slider>();
        //设置UI摄像机
        uiCameraObj = GameObject.FindGameObjectWithTag("UICamera");
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        uiCamera = uiCameraObj.GetComponent<Camera>();
        canvas.worldCamera = uiCamera;

        canvas.sortingOrder = GlobalParams.HPOrder;
  
    }

    public void hideSlider()
    {
        hpSlider.gameObject.SetActive(false);
        isShowHp = false;
    }

    public void showSlider()
    {
        hpSlider.gameObject.SetActive(true);
        isShowHp = true;
    }

    public void updateHpValue(float tvalue, float tmaxValue)
    {
        value = tvalue;
        maxValue = tmaxValue;
    }

	// Update is called once per frame
	void Update () {

        if (!isShowHp) return;
        if (hpSlider == null) hpSlider = GetComponentInChildren<Slider>();
        hpSlider.value = value / maxValue;
        updateHpPos();

        if (delayTime > 0.0 && GlobalParams.totalTime >= delayTime)
        {
            delayTime = 0.0f;
            showSlider();
        }

	}

    void LateUpdate()
    {
        if (!isShowHp) return;
        //updateHpPos();
    }

    void updateHpPos()
    {
        if (target == null) return;
        Canvas canvas = GetComponent<Canvas>();


        //方法1
        ////将世界坐标转化成屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        ////将屏幕坐标转化成UGUI坐标
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, uiCamera, out localPos);
        Vector3 newPos = new Vector3(localPos.x + offset.x, localPos.y + offset.y, 0);
        hpSlider.transform.localPosition = newPos;

        //方法2
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        //Vector3 pos = uiCamera.ScreenToWorldPoint(screenPos);
        //Vector3 newPos = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z);
        //hpSlider.gameObject.transform.position = newPos;


        //方法3
        //获取跟随目标的世界坐标  这个手机上不会抖动
        //Vector3 tarPos = target.transform.position;
        //RectTransform rectTrains1 = GetComponent<RectTransform>();
        ////将世界坐标转化成屏幕坐标
        //Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tarPos);
        //rectTrains1.position = pos;

        

    }
}
