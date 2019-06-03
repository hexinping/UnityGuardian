
/***
 *
 *  Title: "Guardian" 项目
 *         描述：层消隐技术
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

public class SmallObjectLayerCtrl : MonoBehaviour {


    public int intDisappearDistance = 10; //消隐距离

    private float[] distanceArray = new float[32];

	// Use this for initialization
	void Start () {
        distanceArray[8] = intDisappearDistance;
        Camera.main.layerCullDistances = distanceArray;
	}
	
}
