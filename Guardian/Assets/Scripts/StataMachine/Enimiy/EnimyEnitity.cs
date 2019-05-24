using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class EnimyEnitity : BaseEnitity {


    public EnimyEnitiyMode _mode;
    private PlayerEnitity _playerEnitiy;
    public EnimyEnitity()
    {
        initDatas();

        //状态机todo
    }

    void initDatas()
    {
        _mode = new EnimyEnitiyMode();
    }
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitiy = enitity;
    }
   

    override public void initGameObject()
    {

        if (_rootObj)
        {
            GameObject playerGameObject = _playerEnitiy._gameObject;
            //测试代码
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "EnimyTest";
            cube.tag = "Enimy";
            cube.transform.parent = _rootObj.transform;
            cube.transform.position = playerGameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
            _gameObject = cube;

        }
    }

    override public void onDestory()
    {

        //暂时直接消失，后面要切到死亡状态 todo

        _gameObject.AddComponent<BurnHelper>();
        LevelOneView view = (LevelOneView)rootView;
        view.removeFromEnimyList(this);
    }

    IEnumerator dissoveDead()
    {
        while (true)
        {
            yield return null;
        
        }
        
    }

    override public float countDamage(BaseEnitity target)
    {
        return _mode.countDamage(target);
    }


    override public float getAtkValue()
    {
        return _mode.getAtkValue();
    }

    override public float getDefenceValue()
    {
        return _mode.getDefenceValue();
    }

    override public float getHpValue()
    {
        return _mode.getHpValue();
    }

    override public void updateMode()
    {
        _mode.update(_params);
        _params.Clear();
    }


}
