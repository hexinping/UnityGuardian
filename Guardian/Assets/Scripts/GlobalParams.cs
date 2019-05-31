using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*定义全局委托*/

public delegate void delayFunc(BaseEnitity enity, string animationName, bool isMove = false);
public delegate void LoadingEndCallback();                                      //定义进度条结束回调函数
public delegate void HeroAttackInputHandle(PlayerStateEnum stateEnum);          //定义英雄的攻击输入委托


public class DelayCall
{
    public float time;
    public int frameCount;
    public delayFunc _callBack;
    public BaseEnitity _enitity;
    public bool _isMove = false;   //是否移动
    public string _animationName = string.Empty;

    public DelayCall(float t, int frameC, delayFunc callBack, BaseEnitity enitity, string animationName = "", bool isMove = false)
    {
        time = t + GlobalParams.totalTime;
        frameCount = frameC;
        _callBack = callBack;
        _enitity = enitity;
        _isMove = isMove;
        _animationName = animationName;
    }
};

//定义全局常量

public static class GlobalParams
{

    public static int gameObjId = 0;

    public static float interval = 1.0f / 30;

    public static int frameCount = 0;
    public static float totalTime = 0.0f;

    public static List<DelayCall> _delayCallList = new List<DelayCall>();

    public static bool isWindow = Application.platform == RuntimePlatform.WindowsPlayer;

    //Input输入常量定义
    public static string Horizontal     = "Horizontal";
    public static string Vertical       = "Vertical";
    public static string NormalAttack   = "NormalAttack";
    public static string MagicTrickA    = "MagicTrickA";
    public static string MagicTrickB    = "MagicTrickB";



    //层级设定
    public static int DamageLabelOrder      = -40;
    public static int HPOrder               = -20;
    public static int ViewOrder             = 0;
    public static int GlobalDialogOrder     = 20;
    public static int TipsOrder             = 40;
    public static int NoticeOrder           = 60;
    public static int LockOrder             = 80;
    public static int NetWorkOrder          = 100;
 

    //缓冲池名字
    public static string DamageLabelPool    = "_DamageLabelLayer";
    public static string HPPool             = "_HpLayer";
    public static string SkillGroundPool    = "SkillGround";
    public static string SkillPool          = "Skill";


    //敌人各种状态名称
    public static string state_enimyIdle        = "Idle";
    public static string state_enimyRun         = "Run";
    public static string state_enimyAttack      = "Attack";
    public static string state_enimyDead        = "Dead";
    public static string state_enimyHurt        = "Hurt";


    //主角动作名称
    public static string anim_player_idle       = "Idle";
    public static string anim_player_run        = "Run";
    public static string anim_player_death      = "Death";
    public static string anim_player_skillA     = "Attack1";
    public static string anim_player_skillB     = "Attack2";
    public static string anim_player_skillC     = "Attack3";
    public static string anim_player_skillD     = "Attack4";
    public static string anim_player_normalAtk1 = "Attack3-1";
    public static string anim_player_normalAtk2 = "Attack3-2";
    public static string anim_player_normalAtk3 = "Attack3-3";


    //敌人1动作名称
    public static string anim_ennimy1_idle          = "idle";
    public static string anim_ennimy1_run           = "run";
    public static string anim_ennimy1_death         = "death";
    public static string anim_ennimy1_normalAttack  = "attack_slash";
    public static string anim_ennimy1_hurt          = "damage_right";



    public static void addDelayCall(DelayCall delayCall)
    {
        _delayCallList.Add(delayCall);
        _delayCallList.Sort(compare); //按降序排
    }

    public static int compare(DelayCall obj1, DelayCall obj2)
    {
        if (obj1.time < obj2.time)
        {
            return 1;
        }
        else if (obj1.time > obj2.time)
        {
            return -1;
        }
        return 0;
    }

    public static void update(float totalTime)
    {
        int count = _delayCallList.Count;
        if (count > 0)
        {
            for (int i = count - 1; i >= 0; i--)
            {
                DelayCall delay = _delayCallList[i];
                float time = delay.time;
                if (time <= totalTime)
                {
                    delayFunc call = delay._callBack;
                    bool isMove = delay._isMove;
                    string animationName = delay._animationName;
                    BaseEnitity enitity = delay._enitity;
                    call(enitity, animationName, isMove);
                    _delayCallList.Remove(delay);
                }
            }
        }
    }

};


