
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

public class HeroMovingByKey : MonoBehaviour {
    public float speed = 8.0f;

    private CharacterController _CC;
    private float _gravity = 1.0f;
    private PlayerEnitity _playerEnitity;
    private HeroMovingByET _moveEt;

	// Use this for initialization
	void Start () {
        _CC = GetComponent<CharacterController>();
        _moveEt = GetComponent<HeroMovingByET>();

	}

    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }
	// Update is called once per frame
	void Update () {

        if (_moveEt.isRunning) return;
        /*
            speed 是控制人物移动的速度
            float h 获取的是操纵杆输入和键盘输入，值为（-1到1）的值，x轴正方向为1，负方向为-1，也就是说A键为-1，D键为1
            float v获取的是操纵杆输入和键盘输入，值为（-1到1）的值，y轴正方向为1，负方向为-1，也就是说W键为1，S键为01
            targetDir 是键盘输入之后获取到的方向，将目标用SimpleMove方法向获取到方向移动
            transform.lookat 是让目标旋转到获取到的方向
            transform.forward 是让目标向正前方移动

         */

        
        float h = Input.GetAxis(GlobalParams.Horizontal);
        float v = Input.GetAxis(GlobalParams.Vertical);
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1)
        {
            Vector3 targetDir = new Vector3(h, 0, v);
            transform.LookAt(-targetDir + transform.position);

            Vector3 movement = transform.forward * Time.deltaTime * speed;

            //添加模拟重力
            movement.y -= _gravity;
            _CC.Move(movement);

            if (_playerEnitity != null)
            {
                //切换成移动状态 因为每一帧都会切换成run状态，所有设置成不循环
                _playerEnitity.changeStateByIndex(PlayerStateEnum.RUN, 1.0f, false);
            }
        }
        else
        {
            if (_playerEnitity != null)
            {
                //切换成IDLE状态
                _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
            }
        }
	}
}
