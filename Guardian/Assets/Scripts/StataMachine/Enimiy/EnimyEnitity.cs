using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimyEnitity : BaseEnitity {


    public EnimyMode _mode;

    private PlayerEnitity _playerEnitiy;
    public EnimyEnitity()
    {
        _mode = new EnimyMode();

      

        //状态机todo
    }


    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitiy = enitity;
    }
   

    override public void initGameObject()
    {

        if (_rootObj)
        {
            GameObject playerGameObject = _playerEnitiy._gameObject;
            //测试代码
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "EnimyTest";
            cube.tag = "Enimy";
            cube.transform.parent = _rootObj.transform;
            cube.transform.position = playerGameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);

            _gameObject = cube;

        }
    }

}
