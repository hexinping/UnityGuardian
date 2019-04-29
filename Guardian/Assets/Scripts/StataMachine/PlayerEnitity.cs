using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEnitity:BaseEnitity  {


    public PlayerMode _mode;
    private GameObject _rootObj;
    public GameObject _gameObject;

    private List<string> _animationNameList;

    private List<BaseState> _stateList;

    private Animation _animation;

    public PlayerEnitity()
    {
        _mode = new PlayerMode();
        _mode._file = "Models/SwordsMan/GreateWarriorNew";

        _stateList = new List<BaseState>();

        
        BaseState playerIdleState = new PlayerIdleState(this);
        BaseState playerRunState = new PlayerRunState(this);
        BaseState playerAttackState = new PlayerAttackState(this);
        BaseState playerDeadState = new PlayerDeadState(this);

        _stateList.Add(playerIdleState);
        _stateList.Add(playerRunState);
        _stateList.Add(playerAttackState);
        _stateList.Add(playerDeadState);

        //状态机设置
        //changeState(playerIdleState);
        _stateMachine.setCurrentState(playerIdleState);
        
        _animationNameList = new List<string>();
        
     
    }
    override public void initGameObject()
    {
        _rootObj = GameObject.Find("StartGame");
        if (_rootObj)
        {
            _gameObject = getGameObject(_mode._file, "GreateWarriorNew", _rootObj, Vector3.zero);
            _gameObject.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);
            _gameObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0);

        }

        //动作添加
        if (_gameObject != null)
        {
            AddAinimainClips();
            Animation animation = _gameObject.GetComponent<Animation>();
            if(animation == null)
            {
                _gameObject.AddComponent<Animation>();
                animation = _gameObject.GetComponent<Animation>();
            }

            _animation = animation;
            foreach(string clipName in _animationNameList)
            {
                string path = "Models/SwordsMan/SwordsManResources/Animations/StoneKing@" + clipName;
                AnimationClip clip = Resources.Load<AnimationClip>(path);
                _animation.AddClip(clip, clipName);
            }

            _gameObject.AddComponent<PlayerEvent>(); //动画帧事件要放到一个与Animation同级的脚本里
            changeAniamtion(_animationNameList[0], 1.0f, true);
            //测试代码
            addBtnListener();
        }
    }


    public void AddAinimainClips()
    {
        _animationNameList.Add("Idle");
        _animationNameList.Add("Run");
        _animationNameList.Add("Attack2");
        _animationNameList.Add("Death");
    }

    public void onClick(Button btn)
    { 
        //点击按钮切换不同的状态
        string name = btn.name;
        int stateIndex = 0;
        bool[] isLoopArr = { true, true ,false, false};
        float[] speedArr = { 1.0f, 1.0f, 1.0f, 1.0f };
        for (int i = 0; i < _animationNameList.Count; i++)
        {
            if (name.Equals(_animationNameList[i]))
            {
                stateIndex = i;

                break;
            }
        }


        //切换状态
        BaseState state = _stateList[stateIndex];
        float speed = speedArr[stateIndex];
        bool isloop = isLoopArr[stateIndex];
        changeState(state, name, speed, isloop);

        //切换动作
        //string animatinName = name;
        //changeAniamtion(animatinName);
        
    }


    public void changeAniamtion(string animatinName, float speed = 1.0f, bool isLoop = false)
    {

        /*
         * AnimationClip:
                frameRate  帧率:1秒多少帧
         *      length     长度（秒）
         *      AddEvent 添加帧事件
         *      
         * 
         * AnimationState：
         *      clip
         *      length 长度（秒）
         *      speed  播放倍速
         *      time  好像可以回放 。。。不确定
         
         */
        if (_animation != null)
        {
            //_animation.CrossFade(animatinName);

            foreach (AnimationState state in _animation)
            {
                if (animatinName.Equals(state.name))
                {
                    state.speed = speed;
                    AnimationClip clip = state.clip;

                    if (isLoop)
                    {
                        //循环播放
                        _animation.wrapMode = WrapMode.Loop;
                    }
                    else
                    {
                        _animation.wrapMode = WrapMode.Once;
                    }

                    //Debug.Log(state.name+ "/ frameRate:" + state.clip.frameRate + "/ length:"+state.length);


                    if (animatinName == "Attack2")
                    {
                        AddAnimationEvent(state.clip, 89, "PrintEvent");
                       
                        //方法1：通过发消息实现 不太方便 需要注册，主要针对于状态，得再攻击状态里注册消息
                        //Dictionary<string, object> exInfo = new Dictionary<string, object>();
                        //float frameRate = clip.frameRate; //1秒都少帧
                        //float frameInterval = 1.0f / frameRate;
                        //int frameIndex = 27;
                        //float time = frameIndex * frameInterval * 1000;
                        //exInfo.Add("msgParams", "测试消息1========");
                        //MessageDispatcher.getInstance().dispatchMessages(time, 0, 0, MessageCustomType.msg1, exInfo);

                        //方法2：调用注册事件回调
                        float frameRate = clip.frameRate; //1秒都少帧
                        float frameInterval = 1.0f / frameRate;
                        int frameIndex = 89;
                        float time = frameIndex * frameInterval;
                        float p = GlobalParams.totalTime + time;
                        Debug.Log("注册时间：" + GlobalParams.totalTime + " / 预测回调时间：" + p + " 当前帧数：" + GlobalParams.frameCount + " 等待时间:" + time);
                        DelayCall delayCall = new DelayCall(time, GlobalParams.frameCount, testEvent, this);
                        GlobalParams.addDelayCall(delayCall);
                    }
                    _animation.CrossFade(animatinName);
                    //_animation.Play(animatinName);
                    break;
                }
            }

        }


        
    }

    public void testEvent(BaseEnitity eniity)
    {
        Debug.Log("testEvent======成功回调========" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);

    }



    public void AddAnimationEvent(AnimationClip clip, int frameIndex, string funcName)
    {
        float frameRate = clip.frameRate; //1秒都少帧
        float frameInterval = 1.0f / frameRate;
        float time = frameIndex * frameInterval;
        AnimationEvent evt = new AnimationEvent();


        float len = clip.length;

        evt.intParameter = 12345;
        evt.time = time;
        evt.functionName = funcName;
        Debug.Log("当前时间:" + Time.time);
        Debug.Log("clip length:"+ len+ " AddAnimationEvent======回调等待时间:" + time +" 当前逻辑时间:" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);
        clip.AddEvent(evt);
    }

    //以下是测试代码
    public void addBtnListener()
    {
        GameObject canvas = GameObject.Find("Canvas");
        //拿到该对象上（包括子对象）所有的按钮
        Button[] btns = canvas.GetComponentsInChildren<Button>();

        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(delegate()
            {
                this.onClick(btn);
            });
        }

    }

  

}
