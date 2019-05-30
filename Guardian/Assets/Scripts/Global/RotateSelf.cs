
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

public class RotateSelf : MonoBehaviour {

	// Use this for initialization
    public float aroundSpeed = 1F;
    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up, aroundSpeed);
    }
}
