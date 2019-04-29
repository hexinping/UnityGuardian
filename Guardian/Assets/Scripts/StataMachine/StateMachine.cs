using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine  {
     
    public BaseState _curState;                 //当前状态
    public BaseState _globalState;              //全局状态
    public BaseState _previousState;            //前置状态

    public BaseEnitity _enitity;                 //拥有实例

    public StateMachine(BaseEnitity enitity)
    {
        _enitity = enitity;
        _curState = null;
        _globalState = null;
        _previousState = null;
    }

    public void setCurrentState(BaseState state)
    {
        _curState = state;
    }

    public void setGlobalState(BaseState state)
    {
        _globalState = state;
    }

    public void setPreState(BaseState state)
    {
        _previousState = state;
    }

    //返回上一个状态
    public void revertToPreviousState()
    {
        if (_previousState != null)
        {
            changeState(_previousState);
        }
    }

    public void changeState(BaseState state, params object[] values)
    {
        if (_curState == state)
        {
            return;
        }

        //保存之前状态
        _previousState = _curState;

        //退出当前状态
        _curState.exit(values);

        //设置当前状态
        _curState = state;

       //进入当前状态
        _curState.enter(values);
    
    }

    public void update(float dt, params object[] values)
    {
        //全局状态刷新
        if (_globalState != null)
        {
            _globalState.excute(values);
        }

        //当前状态刷新
        if (_curState != null)
        {
            _curState.excute(values);
        }
    
    }

    //消息处理
    public bool handleMessage(Message msg)
    {
        //当前状态先监听
        if (_curState != null)
        {
              return _curState.onMessage(_enitity, msg);
        }

        //全局状态监听
        if (_globalState != null)
        {
              return _globalState.onMessage(_enitity, msg);
        }

        return false;
        
    }

}
