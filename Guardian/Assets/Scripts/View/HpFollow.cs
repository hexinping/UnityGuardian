
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

    private GameObject uiCamera;
    private GameObject hpLayer;

    private float delayTime = 0.0f; //延迟显示时间


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
        hpSlider = GetComponentInChildren<Slider>();

        //设置UI摄像机
        uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = uiCamera.GetComponent<Camera>();

        canvas.sortingOrder = GlobalParams.HPOrder;
        
        GameObject onj = hpSlider.gameObject;
        RectTransform rect1 = onj.GetComponent<RectTransform>();
        rect1.localPosition = new Vector3(offset.x, offset.y, 0);
        rectTrains = rect1;
    }

    public void hideSlider()
    {
        hpSlider.gameObject.SetActive(false);
    }

    public void showSlider()
    {
        hpSlider.gameObject.SetActive(true);
    }

    public void updateHpValue(float tvalue, float tmaxValue)
    {
        value = tvalue;
        maxValue = tmaxValue;
    }
	// Update is called once per frame
	void Update () {

        hpSlider.value = value / maxValue;
	}

    void LateUpdate()
    {
        if (target == null) return;
        Canvas canvas = GetComponent<Canvas>();

        ////将世界坐标转化成屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        ////将屏幕坐标转化成UGUI坐标
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out localPos);
        Vector3 newPos = new Vector3(localPos.x + offset.x, localPos.y + offset.y, 0);
        hpSlider.transform.localPosition = newPos;

        if (delayTime >0.0  && GlobalParams.totalTime >= delayTime)
        {
            delayTime = 0.0f;
            showSlider();
        }
    }
}
