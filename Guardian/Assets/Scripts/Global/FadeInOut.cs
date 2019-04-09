
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

    public static FadeInOut  _instance;

    public float speed = 0.2f;

    public void Awake()
    {
        _rawImage = this.GetComponent<RawImage>();
        _instance = this;
    }
	// Use this for initialization

    public void FadeIn()
    {
        StartCoroutine("fadeInAction");
    }

    public void FadeOut()
    {
        StartCoroutine("fadeOutAction");
    }

    IEnumerator fadeInAction()
    {
        while (true)
        {
            _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, Time.deltaTime * speed);
            if (_rawImage.color.a <= 0.05)
            {
                _rawImage.color = Color.clear;
                _rawImage.enabled = false;
                break;
            }
            yield return null;
        }
       
    }

    IEnumerator fadeOutAction()
    {
        _rawImage.enabled = true;
        while (true)
        {
            _rawImage.color = Color.Lerp(_rawImage.color, Color.black, Time.deltaTime * speed);
            if (_rawImage.color.a >=0.95)
            {
                _rawImage.color = Color.black;
                break;
            }
            yield return null;
        }

    }

}
