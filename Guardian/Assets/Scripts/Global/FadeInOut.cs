
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

public class FadeInOut : MonoBehaviour {

    private RawImage _rawImage; //RawImage组件

    public float speed = 0.2f;

    public bool fadeoutEnd = false;
    public bool fadeinEnd = false;

    public delegate void FadeInEndCallback(); //定义回调函数
    public delegate void FadeOutEndCallback(); //定义回调函数

    private FadeInEndCallback _inCallBack;
    private FadeOutEndCallback _outCallBack;

    private Coroutine _fadeInCoroutine;
    private Coroutine _fadeOutCoroutine;
    public void Awake()
    {
        _rawImage = this.GetComponent<RawImage>();
       
    }
	// Use this for initialization

    public void FadeIn(FadeInEndCallback callBack = null)
    {
        fadeinEnd = false;
        //StopCoroutine("fadeInAction");
        //StartCoroutine("fadeInAction");
        if (_fadeInCoroutine != null)
        {
            StopCoroutine(_fadeInCoroutine);
        }
        _fadeInCoroutine = StartCoroutine(fadeInAction(callBack));
    }

    public void FadeOut(FadeOutEndCallback callBack = null)
    {
        fadeoutEnd = false;
        //StopCoroutine("fadeOutAction");
        //StartCoroutine("fadeOutAction");

        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
        }
        _fadeOutCoroutine = StartCoroutine(fadeOutAction(callBack));
    }

    IEnumerator fadeInAction(FadeInEndCallback callBack)
    {
        _inCallBack = callBack;
        while (true)
        {
            _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, Time.deltaTime * speed);
            float s = speed;
            float a = _rawImage.color.a;
            if (_rawImage.color.a <= 0.01)
            {
                _rawImage.color = Color.clear;
                setRawImageEnable(false);
                fadeinEnd = true;

                if (_inCallBack != null)
                {
                    _inCallBack();
                    _inCallBack = null;
                }
               
                break;
            }
            yield return null;
        }
       
    }

    IEnumerator fadeOutAction(FadeOutEndCallback callBack)
    {
        setRawImageEnable(true);
        _outCallBack = callBack;
        while (true)
        {
            _rawImage.color = Color.Lerp(_rawImage.color, Color.black, Time.deltaTime * speed);
            if (_rawImage.color.a >=0.99)
            {
                _rawImage.color = Color.black;
                fadeoutEnd = true;
                if (_outCallBack != null)
                {
                    _outCallBack();
                    _outCallBack = null;
                }
                break;
            }
            yield return null;
        }

    }

    public void setRawImageEnable(bool isEnable)
    {

        _rawImage.enabled = isEnable;
    }

}
