
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

public class HeroMovingByET : MonoBehaviour {


    private const string MoveJoystickName = "Herojoystick";

    private Animation _animation;

    private float _speed = 10.0f;

    void Awake()
    {
        _animation = gameObject.GetComponent<Animation>();
    }

	 void OnEnable()  
     {  
        EasyJoystick.On_JoystickMove += OnJoystickMove;  
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;  
     }  

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;  
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;  
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;  
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;  
    }


     //移动摇杆结束  
     void OnJoystickMoveEnd(MovingJoystick move)  
     {  
            //停止时，角色恢复idle  
         if (move.joystickName == MoveJoystickName)  
        {
            _animation.CrossFade("Idle");  
        }  
     }  
 

    //移动摇杆中  
     void OnJoystickMove(MovingJoystick move) 
     {
         if (move.joystickName != MoveJoystickName)
         {
             return;
         }

         //获取摇杆中心偏移的坐标  
         float joyPositionX = move.joystickAxis.x;
         float joyPositionY = move.joystickAxis.y;

         if (joyPositionY != 0 || joyPositionX != 0)
         {
             //设置角色的朝向（朝向当前坐标+摇杆偏移量）  
             transform.LookAt(new Vector3(transform.position.x - joyPositionX, transform.position.y, transform.position.z - joyPositionY));
             //移动玩家的位置（按朝向位置移动）  
             transform.Translate(Vector3.forward * Time.deltaTime * _speed);
             //播放奔跑动画  
             _animation.CrossFade("Run");
         }  
     }  
   
}
