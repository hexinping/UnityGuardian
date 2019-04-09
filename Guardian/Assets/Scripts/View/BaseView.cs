
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

    private string _name;            
    private string _prefabName;

    private GameObject _prefab;
    public GameObject _viewRoot;

    //view的根节点
    private GameObject _rootViewGameObject;

    //dialog的根节点，dialog是基于view的
    private GameObject _rootDialogGameObject;

    private ViewManager _viewManager;

    public int _index;


    public void initUI(GameObject prefab, string name)
    {

        _viewManager = ViewManager.getInstance();

        GameObject viewLayer = _viewManager._viewLayer;

        _name = name;
        _prefab = prefab;
        GameObject view = GameObject.Instantiate(_prefab);

        view.transform.parent = viewLayer.transform;

        //默认取屏幕中点
        Vector3 displayPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        view.transform.localPosition = displayPos;

        view.name = name;

        //页面dailog的层级要高于View
        _rootDialogGameObject = new GameObject("rootDialogNode");
        _rootDialogGameObject.transform.parent = view.transform;

        _viewRoot = view;

    }


    public string getViewName()
    {
        return _name;
    }

    public GameObject getViewRoot()
    {
        return _viewRoot;
    }

    public void setActive(bool active)
    {
        _viewRoot.SetActive(active);
    }

    public void destory()
    {
        GameObject.DestroyImmediate(_viewRoot);
    }

    public void onHide()
    {
        Debug.Log("hide viewName:" + _name);
    }

    public void onTop()
    {
        Debug.Log("top viewName:" + _name);
    }

    //virtual public void addListener()
    //{

    //}
    

}
