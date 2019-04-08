
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

public class ViewManager
{

    public Dictionary<string, BaseView> _viewDict = null;

    private static ViewManager _instance = null;

    public GameObject _viewLayer;


    public ViewManager()
    {
        _viewDict = new Dictionary<string, BaseView>();
    }

    static public ViewManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new ViewManager();
        }
        return _instance;
    }

    public void showView(GameObject prefab, string viewName)
    {
        BaseView view = new BaseView();
        view.initUI(prefab, viewName);
        GameObject viewRootNode = view.getViewRoot();
        viewRootNode.transform.parent = _viewLayer.transform;

        //页面管理
        _viewDict[viewName] = view;

    }

    public void popView()
    { 
    
    }


}
