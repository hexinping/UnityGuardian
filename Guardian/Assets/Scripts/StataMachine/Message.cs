using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Message  {

    public MessageCustomType _messageId;                              //消息ID
    public int _senderId;                               //发送者ID
    public int _reviererId;                             //接收者ID
    public DateTime _dispatchTime;                           //发送时间
    public Dictionary<string, object> _extraInfo;       //额外参数


    public Message(MessageCustomType messageId, int senderId, int reviererId, DateTime dispatchTime, Dictionary<string, object> extraInfo)
    {
        _messageId = messageId;
        _senderId = senderId;
        _reviererId = reviererId;
        _dispatchTime = dispatchTime;
        _extraInfo = extraInfo;
    }
}


//消息ID统一管理
public enum MessageCustomType
{ 
    msg1 = 1000,
    msg2 = 1001,

}
