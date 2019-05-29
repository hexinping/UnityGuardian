using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kernal;

/*
    实例基类

*/
public class BaseEnitity  {

    public int _id;         //唯一标识id
    public string file;     //模型路径文件或者预设路径文件

    public StateMachine _stateMachine;
    public List<BaseState> _stateList;

    //public BaseMode _mode;

    public GameObject _rootObj;      //父节点
    public GameObject _gameObject;   //自身节点

    public bool isMove = false;
    public float moveSpeed = 10.0f;

    public bool isAttacking = false;
    public bool isDead = false;
    public bool isHurt = false;

    public BaseView rootView;

    public BaseEnitity moveTarget;
    public BaseEnitity attackTarget;
    public Vector3 moveTargetPos;

    public float posX;
    public float posY;

    public Dictionary<string, object> _params;

    private GameObject _prefabDamageLabe1_1;

    public Animator _animator;


    public float damageLabelOffsetY = 0.0f;
    public BaseEnitity()
    {
        _id = GlobalParams.gameObjId;
        GlobalParams.gameObjId++;

        initModeData();

        //创建状态机
        StateMachine stateMachine = new StateMachine(this);
        setStateMachine(stateMachine);

        _stateList = new List<BaseState>();
        addBaseState();

        //注册消息机制
        MessageDispatcher.getInstance().registerEntity(this);

        _params = new Dictionary<string,object>();

        _prefabDamageLabe1_1 = Resources.Load<GameObject>("Prefabs/View/DamageLabel");
    }

    //必须重载
    virtual public void initModeData()
    {

    }

    //必须重载
    virtual public void addBaseState()
    { 
    
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
        //Debug.Log("attackTargetHurt=====attackerID:"+this._id + "targetID:"+target._id);
        float damage = countDamage(target);
        target.beDamage(damage);

    }

    public void damageLabelMove(float value)
    {
        //使用缓冲池创建一个
        GameObject obj = PoolManager.PoolsArray[GlobalParams.DamageLabelPool].GetGameObjectByPool(_prefabDamageLabe1_1,
            _gameObject.transform.position, Quaternion.identity);
        DamageLabelMove lableMove = obj.GetComponent<DamageLabelMove>();
        lableMove.setTextValue(value);
        lableMove.startMove(_gameObject, damageLabelOffsetY);
    }
    public void beDamage(float damamge)
    {
        //Debug.Log("受到伤害 id:" + this._id );
        damamge = Mathf.Abs(damamge);
        damageLabelMove(damamge);
        float curHp = getHpValue();
        curHp = Mathf.Max(0, curHp - damamge);
        _params.Add(BaseMode.c_hp, curHp);
        updateMode();
        if (curHp <= 0)
        {
            isDead = true;
            onDestory();
        }
        else
        {
            isHurt = true;
        }
    }

    virtual public void onDestory()
    {

    }

    virtual public float countDamage(BaseEnitity target)
    {
        return 0;
    }

    virtual public void updateHP()
    {
        
    }

    virtual public float getAtkValue()
    {
        return 0;
    }

    virtual public float getDefenceValue()
    {
        return 0;
    }

    virtual public float getDexterityValue()
    {
        return 0;
    }

    virtual public float getHpValue()
    {
        return 0;
    }


    virtual public void updateMode()
    {
        
    }



}
