using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
    实例基类

*/
public class BaseEnitity  {

    public int _id;         //唯一标识id
    public string file;     //模型路径文件或者预设路径文件

    public StateMachine _stateMachine;

    public BaseMode _mode;

    public GameObject _rootObj;

    public bool isMove = false;
    public float moveSpeed = 10.0f;

    public BaseEnitity()
    {
        _id = GlobalParams.gameObjId;
        GlobalParams.gameObjId++;

        //创建状态机
        StateMachine stateMachine = new StateMachine(this);
        setStateMachine(stateMachine);

        //注册消息机制
        MessageDispatcher.getInstance().registerEntity(this);
    }

    public void setRootObj(GameObject rootObj)
    {
        _rootObj = rootObj;
    }

    //初始化数据
    virtual public void intDatas()
    { 
        //必须重载
    }

    public GameObject getGameObject(string prefabPath, string name, GameObject parentObj ,Vector3 pos)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = name;
        obj.transform.localPosition = pos;
        obj.transform.parent = parentObj.transform;
        return obj;
    }

    
    virtual public void initGameObject()
    { 
    
    }


    public void setStateMachine(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public bool handleMessage(Message msg)
    {
        if (_stateMachine != null)
        {
             return _stateMachine.handleMessage(msg);
        }
        return false;
    }

    public void changeState(BaseState state, params object[] values)
    {
        if (_stateMachine != null)
        {
            _stateMachine.changeState(state, values);
        }
    }
}
