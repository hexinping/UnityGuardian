using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class EnimyEnitity : BaseEnitity {


    public EnimyEnitiyMode _mode;

    private GameObject _prefabDamageLabe1_1;

    private PlayerEnitity _playerEnitiy;
    public EnimyEnitity()
    {
        initDatas();
        _prefabDamageLabe1_1 = Resources.Load<GameObject>("Prefabs/View/DamageLabel");

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


            //使用缓冲池创建一个
            GameObject obj = PoolManager.PoolsArray[GlobalParams.DamageLabelPool].GetGameObjectByPool(_prefabDamageLabe1_1,
                cube.transform.position, Quaternion.identity);
            DamageLabelMove lableMove = obj.GetComponent<DamageLabelMove>();
            lableMove.startMove(cube);
 
        }
    }

    override public void onDestory()
    {
        GameObject.Destroy(_gameObject);
        LevelOneView view = (LevelOneView)rootView;
        view.removeFromEnimyList(this);
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
