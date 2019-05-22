using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
           // cube.transform.position = playerGameObject.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
            cube.transform.position = playerGameObject.transform.position + new Vector3(2.0f, 0.0f, 2.0f);

            _gameObject = cube;

            //测试文字

            GameObject prefab = Resources.Load<GameObject>("Prefabs/View/DamageLabel");
            GameObject obj = GameObject.Instantiate(prefab);
            obj.name = "DamageLabel";

            //obj.transform.parent = _gameObject.transform;
            //obj.transform.position = Vector3.zero;


            Vector3 tarPos = cube.transform.position;
            RectTransform rectTrains = obj.GetComponent<RectTransform>();
            ////将世界坐标转化成屏幕坐标
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tarPos);
            //rectTrains.position = pos;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(tarPos); 
         


            DamageLabelMove lableMove = obj.GetComponentInChildren<DamageLabelMove>();
            lableMove.setTxtPostion(screenPos);
            ////lableMove.startMove(new Vector3(-128, 100, 0));


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
