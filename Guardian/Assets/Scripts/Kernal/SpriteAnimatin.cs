using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimatin : MonoBehaviour {


    public float _speed;    //播放速度 默认1倍速
    public bool _isLoop;    //是否循环
    public bool _isStop;    //是否停止
    public string _filePath;

    private Dictionary<int, Dictionary<int, List<Sprite>>> _spritesContainer;
    private SpriteRenderer _render;
    private string _file;
    private int _motion;
    private int _direction;
    private float _interval = 0.033F;  //默认间隔时间为33毫秒

    //初始化显示精灵帧
    public Sprite _sprite;

    private AltasManager _manger;

    void Awake()
    {
        _manger = AltasManager.getInstance();

        //精灵帧容器  动作组 方向 精灵帧
        _spritesContainer = new Dictionary<int, Dictionary<int, List<Sprite>>>();
    
        _speed = 1.0F;
        _isLoop = true;
        _render = GetComponent<SpriteRenderer>();
    }

    public List<Sprite> addFileResource(string file)
    {
         List<AltasObj> _atlas = _manger.loadPlistResource(file);
         _file = _atlas[0].name;
         List<Sprite> spList = new List<Sprite>();
         for (int i = 1; i < _atlas.Count; i++)
         {
             AltasObj altasObj = _atlas[i];
             Sprite sp = (Sprite)altasObj.obj;
             spList.Add(sp);

         }
         return spList;
    }
	void Start () {
        _render.sprite = _sprite;

        List<Sprite> _atlas = addFileResource(_filePath);

        Dictionary<int, List<Sprite>> dict = null;
        List<Sprite> list = null;
        for (int i = 0; i < _atlas.Count; i++)
        {
            Sprite s = _atlas[i];
            string name = s.name;

            string[] str = name.Split('_');
            int motion = int.Parse(str[1]);
            int direction = int.Parse(str[2]);

            if (_motion == 0)
            {
                 //初始化动作和方向
                _motion = motion;
                _direction = direction;
            }
           

            if (!_spritesContainer.ContainsKey(motion))
            {
                if (dict != null)
                {
                    dict = null;
                }
                dict = new Dictionary<int, List<Sprite>>();
                 _spritesContainer[motion] = dict;
                if (!dict.ContainsKey(direction))
                {
                    if (list != null)
                    {
                        list = null;
                    }
                    list = new List<Sprite>();
                    dict[direction] = list;
                }
            }

            list.Add(s);
        }

        //改变动画
        //changeMotion(3);
        StartCoroutine("PlayAnimationForwardIEnum");
	}
	

    private IEnumerator PlayAnimationForwardIEnum()
    {
        int index = 0;//可以用来控制起始播放的动画帧索引
        gameObject.SetActive(true);
        while (true)
        {
            int count = getCountSpriteInMotionDir(_motion, _direction);
            if (index > count - 1)
            {
                index = 0;
                if (!_isLoop)
                {
                    _isStop = true;
                }
            }
            if (!_isStop)
            {
                _render.sprite = getCurrentSprite(index);
                index++;
            }
            yield return new WaitForSeconds(_interval * 1.0F/_speed);//等待间隔  控制动画播放速度
        }
    }

    public Sprite getCurrentSprite(int index)
    { 
        Sprite s = _spritesContainer[_motion][_direction][index];
        return s;
    }

    public int getCountSpriteInMotionDir(int motion, int dir)
    {
        int count = _spritesContainer[motion][dir].Count;
        return count;
    }


    public void setMotion(int motion)
    {
        _motion = motion;
    }

    public void setDir(int dir)
    {
        _direction = dir;
    }

    public void changeMotion(int motion, int dir = 1)
    {
        setMotion(motion);
        setDir(dir);
    }

    public void setSpeed(float speed)
    {
        _speed = speed;
    }

    public void stop()
    {
        _isStop = true;
    }

    public void setLoop(bool isLoop)
    {
        _isLoop = isLoop;
    }
}
