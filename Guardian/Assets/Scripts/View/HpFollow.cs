
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

    private Transform target;
    private float value;
    private float maxValue;
    private Vector2 offset;

    private GameObject uiCamera;
    private GameObject hpLayer;



    
	// Use this for initialization
	void Start () {
        hpSlider = GetComponentInChildren<Slider>();
        rectTrains = GetComponent<RectTransform>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initUI(); 
	}

    public void setHpUIDatas(Vector2 tOffset, float tValue, float tMaxValue)
    {
        offset = tOffset;
        value = tValue;
        maxValue = tMaxValue;
    }

    void initUI()
    {
        //设置UI摄像机
        uiCamera = GameObject.FindGameObjectWithTag("UICamera");
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = uiCamera.GetComponent<Camera>();

        canvas.sortingOrder = GlobalParams.HPOrder;
    
        GameObject onj = hpSlider.gameObject;
        RectTransform rect1 = onj.GetComponent<RectTransform>();
        rect1.localPosition = new Vector3(offset.x, offset.y, 0);
    }

    public void updateHpValue(float tvalue, float tmaxValue)
    {
        value = tvalue;
        maxValue = tmaxValue;
    }
	// Update is called once per frame
	void Update () {

        if (target == null) return;

        //获取跟随目标的世界坐标
        Vector3 tarPos = target.transform.position;

        //将世界坐标转化成屏幕坐标
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tarPos);
        rectTrains.position = pos;

        hpSlider.value = value / maxValue;
	}
}
