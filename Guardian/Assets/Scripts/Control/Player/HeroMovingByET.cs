
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

    //private float _speed = 10.0f;

    private CharacterController _CC;

    private float _gravity = 1.0f;

    private PlayerEnitity _playerEnitity;

    public bool isRunning = false;

    void Awake()
    {
        _animation = this.GetComponent<Animation>();
        _CC = this.GetComponent<CharacterController>();
    }

    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }


	 void OnEnable()  
     {
        isRunning = false;
        EasyJoystick.On_JoystickMove += OnJoystickMove;  
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;  
     }  

    void OnDisable()
    {
        isRunning = false;
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
           // _animation.CrossFade("Idle");
            if (_playerEnitity != null)
            {
                isRunning = false;
                //切换成IDLE状态
                _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
            }
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
             isRunning = true;

             //设置角色的朝向（朝向当前坐标+摇杆偏移量）  
             transform.LookAt(new Vector3(transform.position.x - joyPositionX, transform.position.y, transform.position.z - joyPositionY));
             //移动玩家的位置（按朝向位置移动）  
             //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
             Vector3 movement = transform.forward * Time.deltaTime * _playerEnitity.moveSpeed;

             //添加模拟重力
             movement.y -= _gravity;
             _CC.Move(movement); //transform.forward * Time.deltaTime * _speed

             if (_playerEnitity != null)
             {
                  //切换成移动状态
                 _playerEnitity.changeStateByIndex(PlayerStateEnum.RUN, 1.0f, true);
             }
             //播放奔跑动画  
             //_animation.CrossFade("Run");
             //BaseState state = _playerEnitity._stateList[1];
            
         }  
     }  
   
}
