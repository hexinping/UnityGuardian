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

    public GameObject _rootObj;      //父节点
    public GameObject _gameObject;   //自身节点

    public bool isMove = false;
    public float moveSpeed = 10.0f;

    public bool isAttacking = false;
    public bool isDead = false;

    public BaseView rootView;

    public BaseEnitity moveTarget;
    public BaseEnitity attackTarget;

    public float maxHp;
    public float hp;
    public float atk;

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

    public void setRootView(BaseView view)
    {
        rootView = view;
        
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

    public void changeState(BaseState state, bool isCheckSameState = true, params object[] values)
    {
        if (_stateMachine != null)
        {
            _stateMachine.changeState(state, isCheckSameState, values);
        }
    }

     virtual public void faceToTarget()
    {
        
    }

    public void attackTargetHurt(BaseEnitity target)
    {
        Debug.Log("attackTargetHurt=====attackerID:"+this._id + "targetID:"+target._id);
        target.beDamage(this.atk);

    }
    public void beDamage(float damamge)
    {
        Debug.Log("受到伤害 id:" + this._id );
        damamge = Mathf.Abs(damamge);
        hp = Mathf.Max(0, hp - damamge);

        if (hp <= 0)
        {
            isDead = true;
            onDestory();
        }
    }

    virtual public void onDestory()
    {

    }
}
