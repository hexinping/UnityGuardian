
/***
 *
 *  Title: "Guardian" 项目
 *         描述：
 *
 *  Description:
 *        功能：
 *       
 *
 *  Date: 2019
 * 
 *  Version: 1.0
 *
 *  Modify Recorder:
 *     
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseView:MonoBehaviour
{

    public string _name;            
    public string _prefabName;
    //dialog的根节点，dialog是基于view的
    public GameObject _rootDialogGameObject;


    protected ViewManager _viewManager;
    protected AudioManager _audioManager;
    protected GameObject _mainCamera;
    protected GameObject _uiCamera;
    protected GameObject _sceneNode;
       
    //场景节点
    protected GameObject _sceneBgNode;                //场景背景层
    protected GameObject _sceneSkillGroundNode;       //场景地面技能效果层
    protected GameObject _sceneDecorateNode;          //场景装饰物层
    protected GameObject _sceneRoleNode;              //场景人物物层
    protected GameObject _sceneSkillNode;             //场景技能效果层

    //参数列表
    protected object[] paramsValue;


    public void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _viewManager = ViewManager.getInstance();
        _audioManager = AudioManager.getInstance();
        _sceneNode = _viewManager._rootScene;
        _uiCamera = GameObject.FindGameObjectWithTag("UICamera");

        _sceneBgNode            = _sceneNode.transform.Find("BG").gameObject;
        _sceneSkillGroundNode   = _sceneNode.transform.Find("SkillGround").gameObject;
        _sceneDecorateNode      = _sceneNode.transform.Find("Decorate").gameObject;
        _sceneRoleNode          = _sceneNode.transform.Find("Role").gameObject;
        _sceneSkillNode         = _sceneNode.transform.Find("Skill").gameObject;
    }


    public void Start()
    {
        setUICamera(gameObject, "Canvas");
        _audioManager.transform.localPosition = new Vector3(0,0,0);
    }
    virtual public void onHide()
    {
        Debug.Log("hide viewName:  " + _name);
    }

    virtual public void onTop()
    {
        Debug.Log("top viewName:  " + _name);
    }

    public void OnDestory()
    {
        Debug.Log("Destory viewName:  " + _name);

        /*
         unity销毁的物体有2种方式。
        1.DestroyImmediate立即对对像进行销毁并从内存中移除；
        2.Destroy销毁场景中的物体，但内存中还存在，当令它需要销毁时，只是给一个标识。而内存中它依然是存在的，只有当内存不够，或一段时间没有再次被引用时（或者更多合理的条件满足），机制才会将它销毁并释放内存。
        这样做的目的就是为了避免频繁对内存的读写操作。回收器会定时清理一次内存中引用计数为0的对象，很可能你的要销毁的对象在其他地方还有引用而你自己不清楚，直接销毁可能导致其他地方空引用错误。

         
         */
    }

    public GameObject initScene(string name)
    {
        //GameObject obj = (GameObject)Resources.Load("Prefabs/" + name);

        GameObject obj = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, name, this._name);
        GameObject scene = GameObject.Instantiate(obj); //初始化是世界坐标位置是随机的
        scene.name = name;
        scene.transform.parent = _sceneBgNode.transform;
        return scene;
    }

    public void setUICamera(GameObject obj, string canvasName)
    {
        Canvas canvas = obj.transform.Find(canvasName).GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = _uiCamera.GetComponent<Camera>();
    }

    public void setViewParams(params object[] values)
    {
        paramsValue = values;
    }

    virtual public void onClick(Button btn)
    {
        
    }

    public void addBtnListener()
    {
        //拿到该对象上（包括子对象）所有的按钮
        Button[] btns = gameObject.GetComponentsInChildren<Button>();

        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(delegate()
            {
                this.onClick(btn);
            });
        }

    }

    public GameObject initGameObject(string prefabPath, string name, GameObject parent, Vector3 pos)
    {
        if (prefabPath == null || prefabPath.Length == 0)
        {
            return null;
        }
        string prefabName = prefabPath;
        GameObject obj = (GameObject)Resources.Load(prefabName);
        GameObject objClone = GameObject.Instantiate(obj);
        objClone.transform.parent = parent.transform;
        objClone.transform.localPosition = pos;

        if (name.Length != 0)
        {
            objClone.name = name;
        }

        return objClone;

    }

    virtual public void updatePlayerInfo()
    {

    }

}
