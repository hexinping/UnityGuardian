using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEnitity:BaseEnitity  {


    public PlayerMode _mode;
    //private GameObject _rootObj;
    public GameObject _gameObject;

    private List<string> _animationNameList;
    private List<string> _comobAnimationNameList; //组合动作

    public List<BaseState> _stateList;

    private Animation _animation;

    private int _nomarlAttackComobIndex = 0;  //组合动作的序号


    public PlayerEnitity()
    {
        _mode = new PlayerMode();
        _mode._file = "Models/SwordsMan/GreateWarrior";

        _stateList = new List<BaseState>();

        
        BaseState playerIdleState = new PlayerIdleState(this);
        BaseState playerRunState = new PlayerRunState(this);
        BaseState playerAttackState = new PlayerAttackState(this);
        BaseState playerDeadState = new PlayerDeadState(this);
        BaseState playerMagicTrickAState = new PlayerMagicTrickAState(this);
        BaseState playerMagicTrickBState = new PlayerMagicTrickBState(this);

        _stateList.Add(playerIdleState);
        _stateList.Add(playerRunState);
        _stateList.Add(playerDeadState);
        _stateList.Add(playerMagicTrickAState);
        _stateList.Add(playerMagicTrickBState);

        _stateList.Add(playerAttackState);

        //状态机设置
        //changeState(playerIdleState);
        _stateMachine.setCurrentState(playerIdleState);
        
        _animationNameList = new List<string>();
        _comobAnimationNameList = new List<string>();
        
     
    }
    override public void initGameObject()
    {
        //_rootObj = GameObject.Find("_Manager/_ViewManager/_Scene/Role");
        if (_rootObj)
        {
            _gameObject = getGameObject(_mode._file, "GreateWarrior", _rootObj, Vector3.zero);
            _gameObject.transform.localScale = new Vector3(30.0f, 30.0f, 30.0f);
            _gameObject.transform.localPosition = new Vector3(76.9f, -13.02f, -48.27f);
    

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

            //组合动作
            foreach (string clipName in _comobAnimationNameList)
            {
                string path = "Models/SwordsMan/SwordsManResources/Animations/StoneKing@" + clipName;
                AnimationClip clip = Resources.Load<AnimationClip>(path);
                _animation.AddClip(clip, clipName);
            }

            _gameObject.AddComponent<PlayerEvent>(); //动画帧事件要放到一个与Animation同级的脚本里
            changeAniamtion(_animationNameList[0], 1.0f, true);

        }
    }


    public void AddAinimainClips()
    {
        _animationNameList.Add("Idle");
        _animationNameList.Add("Run");
        _animationNameList.Add("Death");

        //技能动作
        _animationNameList.Add("Attack1");
        _animationNameList.Add("Attack4");

        //普通攻击组合动作
        _comobAnimationNameList.Add("Attack3-1");
        _comobAnimationNameList.Add("Attack3-2");
        _comobAnimationNameList.Add("Attack3-3");
    }


    public void changeStateByIndex(PlayerStateEnum playerstateEm, float tSpeed = 1.0f, bool tIsLoop = false)
    {
        int stateIndex = (int)playerstateEm;
        BaseState state = _stateList[stateIndex];
        string name = getAnimationName(playerstateEm);

        bool isCheckSameState = true;
        if (playerstateEm == PlayerStateEnum.NORMALATTACK)
        {
            isCheckSameState = false;
            Debug.Log("_nomarlAttackComobIndex===========" + _nomarlAttackComobIndex);
        }
        
        changeState(state, isCheckSameState, name, tSpeed, tIsLoop);

    }


    private string getAnimationName(PlayerStateEnum playerstateEm)
    {
        string resultName = string.Empty;
       
        if (playerstateEm != PlayerStateEnum.NORMALATTACK)
        {
            int stateIndex = (int)playerstateEm;
            resultName = _animationNameList[stateIndex];
            //return resultName;
        }
        else
        { 
            //给普通攻击添加多个动作
            resultName = _comobAnimationNameList[_nomarlAttackComobIndex];
            Debug.Log("resultName=======" + resultName);
        }
        return resultName;
    
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
                    _animation.CrossFade(animatinName);
              
                    break;
                }
            }

        }


        
    }

    public float getAnimaitionPlayTime(PlayerStateEnum playerstateEm)
    {
        float time = 0.0f;
        string name = string.Empty;
        if (playerstateEm == PlayerStateEnum.NORMALATTACK)
        {
            name = _comobAnimationNameList[_nomarlAttackComobIndex];
        }
        else
        {
            name = getAnimationName(playerstateEm);
        }
        
        foreach (AnimationState state in _animation)
        {
            if (state.name == name)
            {
                time = state.clip.length;
                break;
            }
        
        }
        return time;
    }

    public int getCurrComIndex()
    {
        return _nomarlAttackComobIndex;
      
    }

    public void AddComobIndex()
    {
        //组合动作序号+1
        _nomarlAttackComobIndex++;
        int count = _comobAnimationNameList.Count;
        if (_nomarlAttackComobIndex == count) _nomarlAttackComobIndex = 0;
    }

    public void reduceComobIndex()
    {
        _nomarlAttackComobIndex--;
        int count = _comobAnimationNameList.Count;
        if (_nomarlAttackComobIndex <= 0) _nomarlAttackComobIndex = count -1;
    }


}
