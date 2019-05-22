
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

/*
 
 
 	itween 练习
	{
		移动
		缩放
		旋转
		颜色
		透明度

		停止
		组合动作
		顺序动作
	
		回调todo
	}
 
 */

public class ItweenDemo : MonoBehaviour {

    public Transform transform;
    private Vector3 initialPosition = new Vector3(0, 0, 0);
    private Vector3 activePosition = new Vector3(5, 0, 0);



    private bool _MoveButtonState = false;
    private bool _ScaleButtonState = false;
    private bool _RotateButtonState = false;
    private bool _ColorButtonState = false;
    private bool _OpacityButtonState = false;


    void Start()
    {
        
    }

    void moveDemo()
    {
         if (_MoveButtonState)
        {
             //简单调用 : 第一个参数是要移动的GameObject
             //iTween.MoveTo(transform.gameObject, initialPosition, 3);

            //利用hash表调用 : 第一个参数是要移动的GameObject
            iTween.MoveTo(transform.gameObject, iTween.Hash(
                    "position", initialPosition,
                    "easetype", iTween.EaseType.easeInSine,
                    "time", 0.5,
                    "oncomplete", "moveEndCallBack" //必须是目标身上的方法，这里就是transform.gameObject
          
                ));

            //利用hash表调用 ValueTo必须设置回调，在回调里设置
            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", activePosition,
            //    "to", initialPosition,
            //    "onupdate", "MoveUpdate",
            //    "easetype", iTween.EaseType.easeInSine,
            //    "time", 0.5
            //  ));

            
        }
        else
        {
            //简单调用 : 第一个参数是要移动的GameObject
            //iTween.MoveTo(transform.gameObject, activePosition, 3);

            //利用hash表调用
            iTween.MoveTo(transform.gameObject, iTween.Hash(
               "position", activePosition,
               "easetype", iTween.EaseType.easeInSine,
               "time", 0.5,
               "oncomplete", "moveEndCallBack"
             ));

             
             //利用hash表调用
            //iTween.ValueTo(gameObject, iTween.Hash(
            //        "from", initialPosition,
            //        "to", activePosition,
            //        "onupdate", "MoveUpdate",
            //        "easetype", iTween.EaseType.easeInSine,
            //        "time", 0.5
            //    ));
        }
        _MoveButtonState = !_MoveButtonState;
    }


    void MoveUpdate(Vector3 newCoordinates)
    {
        transform.position = newCoordinates; 
    }


    void scaleDemo()
    { 
         if (_ScaleButtonState)
        {
             //简单调用 : 第一个参数是要移动的GameObject
             //iTween.ScaleTo(transform.gameObject,new Vector3(1,1,1), 2.0f);


            iTween.ScaleTo(transform.gameObject, iTween.Hash(
                "scale", new Vector3(1, 1, 1),
                "time", 2.0f
                ));


            //利用hash表调用 valueTo
            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", new Vector3(2, 2, 2),
            //    "to", new Vector3(1, 1, 1),
            //    "onupdate", "scaleUpdate",
            //    "time", 0.5
            //  ));

            
        }
        else
        {
            //简单调用 : 第一个参数是要移动的GameObject
            //iTween.ScaleTo(transform.gameObject, new Vector3(2, 2, 2), 2.0f);


            iTween.ScaleTo(transform.gameObject, iTween.Hash(
                "scale", new Vector3(2, 2, 2),
                "time", 2.0f

                ));


            //利用hash表调用 valueTo
            //iTween.ValueTo(gameObject, iTween.Hash(
            //  "from", new Vector3(1, 1, 1),
            //  "to", new Vector3(2, 2, 2),
            //  "onupdate", "scaleUpdate",
            //  "time", 0.5
            //));
        }
        _ScaleButtonState = !_ScaleButtonState;
    }

    void scaleUpdate(Vector3 newCoordinates)
    {
        transform.localScale = newCoordinates;
    }

    void rotateDemo()
    { 
    
    }

    void colorDemo()
    { 
    
    }

    void opacityDemo()
    { 
    
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(75, 20, 150, 55), "Move Click Me!"))
        {
            moveDemo();
        }

        if (GUI.Button(new Rect(75, 100, 150, 55), "Scale Click Me!"))
        {
            scaleDemo();
        }


        if (GUI.Button(new Rect(75, 180, 150, 55), "Rotate Click Me!"))
        {
            rotateDemo();
        }


        if (GUI.Button(new Rect(75, 260, 150, 55), "Color Click Me!"))
        {
            colorDemo();
        }

        if (GUI.Button(new Rect(75, 340, 150, 55), "Opacity Click Me!"))
        {
            opacityDemo();
        }
    }
 

}
