
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

    //参数列表
    protected object[] paramsValue;


    public void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _viewManager = ViewManager.getInstance();
        _audioManager = AudioManager.getInstance();
        _sceneNode = _viewManager._rootScene;
        _uiCamera = GameObject.FindGameObjectWithTag("UICamera");
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
        GameObject obj = (GameObject)Resources.Load("Prefabs/" + name);
        GameObject scene = GameObject.Instantiate(obj); //初始化是世界坐标位置是随机的
        scene.name = name;
        scene.transform.parent = _sceneNode.transform;
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


}
