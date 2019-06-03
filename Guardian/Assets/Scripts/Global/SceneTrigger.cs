
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

public class SceneTrigger : MonoBehaviour {

    public List<string> _showNames;
	public List<string> _hideNames;

    private GameObject rootGameObj;
    private Transform rootTransform;


    void Awake()
    {
        rootTransform = GameObject.Find("_Manager/_ViewManager/_Scene/BG").transform;
    }


    //void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag.Equals("Player"))
    //    {
    //        if (_showNames != null && _showNames.Count > 0)
    //        {
    //            for (int i = 0; i < _showNames.Count; i++)
    //            {
    //                string name = _showNames[i];
    //                GameObject sceneObj = rootTransform.Find(name).gameObject;
    //                sceneObj.SetActive(true);
    //            }
    //        }
    //        if (_hideNames != null && _hideNames.Count > 0)
    //        {
    //            for (int i = 0; i < _hideNames.Count; i++)
    //            {
    //                string name = _hideNames[i];
    //                GameObject sceneObj = rootTransform.Find(name).gameObject;
    //                sceneObj.SetActive(false);
    //            }
    //        }
    //    }

    //}

}
