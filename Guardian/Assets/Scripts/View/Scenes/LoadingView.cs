
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
using System;




//放到外面定义的委托，其他文件也可以使用
public delegate void LoadingEndCallback(); //定义回调函数

/*
    不同c#文件之间传递委托函数有几种方法
 * 1 定义全局的委托类型，public delegate XXXX
 * 2 两个类都用Action 类型定义委托
 
 */

public class LoadingView : BaseView {


	public Slider _slider;

	private float _speed = 0.003f;

	private bool _isEnd = false;


	private LoadingEndCallback _callBack;

	//public Action _callBack;

	public void Awake()
	{
		base.Awake();


	}

	//参数的赋值要放到start里
	public void Start()
	{
		base.Start();
        _callBack = (LoadingEndCallback)paramsValue[0];
        //_callBack = (Action)paramsValue[0];
	}

	// Update is called once per frame
	void Update () {
		
		if(!_isEnd)
		{
			float value = Mathf.Lerp(_slider.value,100,Time.deltaTime * _speed);
			_slider.value = value;
			if (value >= 1.0f)
			{
				_isEnd = true;
				
				if(_callBack!=null)
				{
					_callBack();
				}
			}
			
		}
		
	}

	void OnDestroy()
    {
        base.OnDestory();
    }
}
