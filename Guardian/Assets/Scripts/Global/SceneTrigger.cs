
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


    private GameObject scene;
	// Use this for initialization
	void Start () {
		
	}

    public void  setTargetScene(GameObject targetScene)
    {
        scene = targetScene;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            scene.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            scene.SetActive(false);
        }

    }

	
	// Update is called once per frame
}
