using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/*

	状态基类
*/

public class BaseState  {

	public BaseEnitity _enitity;  //拥有实例
    public BaseState(BaseEnitity enitity)
    {
       _enitity = enitity;
    }

    //状态进入
    virtual public void enter(params object[] values)
    {
        //子类必须重载
    }

    //状态执行
    virtual public void excute(params object[] values)
    {
        //子类必须重载
    }

    //状态退出
    virtual public void exit(params object[] values)
    {
        //子类必须重载
    }

    //重载不同的消息处理逻辑
    virtual public bool onMessage(BaseEnitity enitity, Message msg)
    {
        return true;
    }
}
