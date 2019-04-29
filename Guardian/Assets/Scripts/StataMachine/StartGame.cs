using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListObj
{
    public int t;
    public string name;

    public ListObj(int at, string aName)
    {
        t = at;
        name = aName;
    }
};



public class StartGame : MonoBehaviour {


    private Dictionary<int, BaseEnitity> _enitityDic;

   // private float _interval = 0.033f;
    private MessageDispatcher _msgDispatcher;

    private float totalTime = 1.0f; //刷新频率

    void Awake()
    {
        _enitityDic = new Dictionary<int, BaseEnitity>();
        _msgDispatcher = MessageDispatcher.getInstance();

        //Application.targetFrameRate = 30; 
    }
	// Use this for initialization
	void Start () {

        BaseEnitity enitity = new PlayerEnitity();
        _enitityDic.Add(enitity._id, enitity);

        //初始化显示对象
        enitity.initGameObject();


        //测试代码
        //List<int> _testList = new List<int>();
        //_testList.Add(2);
        //_testList.Add(1);
        //_testList.Add(110);
        //_testList.Add(6);

        //_testList.Sort(); //升序

        //foreach(int v in _testList)
        //{
        //    Debug.Log("v========="+v);
        //}

        //List<ListObj> _objList = new List<ListObj>();
        //_objList.Add(new ListObj(11, "hxp11"));
        //_objList.Add(new ListObj(9, "hxp9"));
        //_objList.Add(new ListObj(150, "hxp150"));
        //_objList.Add(new ListObj(1, "hxp1"));
        //_objList.Add(new ListObj(1501, "hxp1501"));
        //_objList.Add(new ListObj(1222, "hxp1222"));
        //_objList.Add(new ListObj(9, "hxp19"));
        //_objList.Add(new ListObj(0, "hxp0"));

        //_objList.Sort(compare);

        //foreach (ListObj v in _objList)
        //{
        //    Debug.Log("_objList=========" + v.t + "/" + v.name);
        //}

        //StartCoroutine("updateGame");
	}

    //public int compare(ListObj obj1, ListObj obj2)
    //{
    //    if (obj1.t < obj2.t)
    //    {
    //        return -1;
    //    }
    //    else if (obj1.t > obj2.t)
    //    {
    //        return 1;
    //    }
    //    return 0;
    //}

    public void testDispatcMsg()
    { 
        Dictionary<string, object> exInfo = new Dictionary<string, object>();

        exInfo.Add("msgParams", "测试消息1========");
        //立刻发
        _msgDispatcher.dispatchMessages(0, 0, 0, MessageCustomType.msg1, exInfo);

        //延迟发
        _msgDispatcher.dispatchMessages(5000, 0, 0, MessageCustomType.msg1, exInfo);
    }
	
	// Update is called once per frame
    //void Update()
    //{
    //    //totalTime -= Time.deltaTime;
    //    //if (totalTime <= 0.0f)

    //    //使用逻辑时间
    //    GlobalParams.frameCount++;
    //    GlobalParams.totalTime += Time.deltaTime;
    //    {
    //        foreach (KeyValuePair<int, BaseEnitity> obj in _enitityDic)
    //        {

    //            int id = obj.Key;
    //            BaseEnitity enitity = obj.Value;
    //            enitity._stateMachine.update(GlobalParams.interval);
    //        }

    //        //消息发送
    //        _msgDispatcher.dispatchDelayedMessages();

    //        //事件调用
    //        GlobalParams.update(GlobalParams.totalTime);

    //        //totalTime = 1.0f;
    //    }

    //}

    void FixedUpdate()
    {
        //使用逻辑时间
        GlobalParams.frameCount++;
        GlobalParams.totalTime += Time.deltaTime;
        {
            foreach (KeyValuePair<int, BaseEnitity> obj in _enitityDic)
            {

                int id = obj.Key;
                BaseEnitity enitity = obj.Value;
                enitity._stateMachine.update(GlobalParams.interval);
            }

            //消息发送
            _msgDispatcher.dispatchDelayedMessages();

            //事件调用
            GlobalParams.update(GlobalParams.totalTime);

            //totalTime = 1.0f;
        }

    }
    IEnumerator updateGame()
    {
        while (true)
        {

            GlobalParams.frameCount++;
            GlobalParams.totalTime += GlobalParams.interval;
            {
                foreach (KeyValuePair<int, BaseEnitity> obj in _enitityDic)
                {

                    int id = obj.Key;
                    BaseEnitity enitity = obj.Value;
                    enitity._stateMachine.update(GlobalParams.interval);
                }

                //消息发送
                _msgDispatcher.dispatchDelayedMessages();

                //事件调用
                GlobalParams.update(GlobalParams.totalTime);

                //totalTime = 1.0f;
            }
            yield return new WaitForSeconds(GlobalParams.interval);
        }
    
    }


   
}
