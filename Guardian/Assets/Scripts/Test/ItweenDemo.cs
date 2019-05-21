
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

public class ItweenDemo : MonoBehaviour {


    public bool buttonState = false;
    public Transform transform;
    private Vector3 initialPosition = new Vector3(0, 0, 0);
    private Vector3 activePosition = new Vector3(5, 0, 0);


    void Start()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", initialPosition, "to", activePosition, "onupdate", "MoveButton", "easetype", iTween.EaseType.elastic, "time", 5.0));
    }
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(75, 20, 100, 55), "Click Me!"))
    //    {
    //        if (buttonState)
    //        {
    //            iTween.ValueTo(gameObject, iTween.Hash("from", activePosition, "to", initialPosition, "onupdate", "MoveButton", "easetype", iTween.EaseType.elastic, "time",3.0));
    //        }
    //        else
    //        {
    //            iTween.ValueTo(gameObject, iTween.Hash("from", initialPosition, "to", activePosition, "onupdate", "MoveButton", "easetype", iTween.EaseType.elastic,"time",3.0));
    //        }
    //        buttonState = !buttonState;
    //    }
    //}
 
    void MoveButton(Vector3 newCoordinates)
    {
        transform.position = newCoordinates;
    }

}
