
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

public class CDCtrl : MonoBehaviour {


    public Image cdImage;
    public GameObject grayObj;
    public Button btn;
    

    private bool _isStartTime = false;
    private float _cdPlayTime;

    private float _cdTotalTime = 2.0f; //todo
   


    void Start()
    {
        btn.interactable = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (_isStartTime)
        {
            _cdPlayTime += Time.deltaTime;
            btn.interactable = false;
            grayObj.SetActive(true);
            cdImage.fillAmount = _cdPlayTime / _cdTotalTime;
            if (_cdPlayTime >= _cdTotalTime)
            {
                _cdPlayTime = 0.0f;
                _isStartTime = false;
                cdImage.fillAmount = 1.0f;
                btn.interactable = true;
                grayObj.SetActive(false);
            }

        }
	}

    public void startCd()
    {
        _isStartTime = true;
    }
}
