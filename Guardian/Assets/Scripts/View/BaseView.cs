
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

public class BaseView : MonoBehaviour
{

    private string _name;            
    private string _prefabName;

    private GameObject _prefab;

    //view的根节点
    private GameObject _rootViewGameObject;

    //dialog的根节点，dialog是基于view的
    private GameObject _rootDialogGameObject;

    private ViewManager _viewManager;

    //public BaseView()
    //{ 
    
    //}

    public void Awake()
    {
        //_rootViewGameObject = new GameObject("rootViewNode");
        //_rootDialogGameObject = new GameObject("rootDialogNode");
        ////默认取屏幕中点
        //Vector3 displayPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        //setPos(displayPos);

        //_viewManager = ViewManager.getInstance();

    }

    //public void Start()
    //{
    //    initUI();
    //}

    public void initUI(GameObject prefab, string name)
    {
        _rootViewGameObject = new GameObject("rootViewNode");
        _rootDialogGameObject = new GameObject("rootDialogNode");
        //默认取屏幕中点
        Vector3 displayPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        setPos(displayPos);

        _viewManager = ViewManager.getInstance();

        _name = name;
        _prefab = prefab;
        GameObject view = Instantiate(_prefab);
        view.transform.parent = _rootViewGameObject.transform;

        //页面dailog的层级要高于View
        _rootDialogGameObject.transform.parent = _rootViewGameObject.transform;

    }

    public void setPos(Vector3 displayPos)
    {
        _rootViewGameObject.transform.localPosition = displayPos;
    }

    public GameObject getViewRoot()
    {
        return _rootViewGameObject;
    }

    public string getViewName()
    {
        return _name;
    }

    

}
