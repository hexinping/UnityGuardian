using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageDispatcher  {

    private static MessageDispatcher _instance;

    public Dictionary<int, BaseEnitity> _enitityDic;
    public List<Message> _delayMsgList;

    private MessageDispatcher()
    {
        _enitityDic = new Dictionary<int, BaseEnitity>();
        _delayMsgList = new List<Message>();
    }

    static public MessageDispatcher getInstance()
    {
        if (_instance == null)
        {
            _instance = new MessageDispatcher();
        }
        return _instance;
    }

   
    public void registerEntity(BaseEnitity enitity)
    {
        if (!_enitityDic.ContainsKey(enitity._id))
        {
            _enitityDic.Add(enitity._id, enitity);
        }
    }

    /*
        -- 发送消息
        -- delay<=0为立即发送 单位为毫秒
        -- 发送者ID  接收者ID  消息ID  额外信息
     */
    public void dispatchMessages(float delay, int senderID, int reviererID, MessageCustomType msgID, Dictionary<string, object> extraInfo)
    {
        BaseEnitity sender = _enitityDic[senderID];
        BaseEnitity revierer = _enitityDic[reviererID];
        if (sender == null || revierer == null)
        {
            return;
        }
        DateTime time = DateTime.Now;  //暂时用系统时间代替 后面换成逻辑时间

        DateTime dispatchTime = time;
        Message msg = new Message(msgID, senderID, reviererID, dispatchTime, extraInfo);
        Debug.Log("当前时间:" + dispatchTime);
        if (delay <= 0.0f)
        {
            Debug.Log("立即发送消息 时间:" + dispatchTime + " 发送方：" + senderID + "  接收方：" + reviererID + " 消息ID：" + msgID);
            discharge(revierer, msg);
        }
        else
        {
            TimeSpan span = new TimeSpan(0,0,0,0,(int)delay);
            msg._dispatchTime += span;

            //加入延迟发送消息列表
            _delayMsgList.Add(msg);
            Debug.Log("延迟发送消息 时间:" + msg._dispatchTime + " 发送方：" + senderID + "  接收方："+ reviererID +" 消息ID："+ msgID);
        }
 
    }

    //立即发送消息
    public void discharge(BaseEnitity revierer, Message msg)
    {
        if (revierer != null)
        {
            if (!revierer.handleMessage(msg))
            {
                Debug.Log("消息接收处理不成功====="+ msg._messageId);
            }
        }
    }

    //延时发送消息
    public void dispatchDelayedMessages()
    {
        DateTime time = DateTime.Now;
        int len = _delayMsgList.Count;
        if (len > 0)
        {
            //倒序遍历 一边遍历 一边删除
            for (int i = len - 1; i >= 0; i--)
            {
                Message msg = _delayMsgList[i];
                DateTime dispatchTime = msg._dispatchTime;
                if (dispatchTime != null)
                {
                    TimeSpan span = dispatchTime - time;
                    if (span.TotalMilliseconds <= 0)
                    {
                        BaseEnitity revierer = _enitityDic[msg._reviererId];
                        discharge(revierer, msg);
                        _delayMsgList.Remove(msg);
                    }
                }
            }
                
        }
        
    }
}
