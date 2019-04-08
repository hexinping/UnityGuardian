
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

public class ViewManager : MonoBehaviour
{

    public Dictionary<string, BaseView> _viewDict = null;

    private static ViewManager _instance = null;

    public GameObject _viewLayer;


    public void Awake()
    {
        _instance = this;
        _viewDict = new Dictionary<string, BaseView>();
    }

    static public ViewManager getInstance()
    {
        return _instance;
    }

    public void showView(string prefabName, string viewName)
    {
        BaseView view = new BaseView();
        GameObject obj = (GameObject)Resources.Load(prefabName);
        view.initUI(obj, viewName);

        //页面管理
        _viewDict[viewName] = view;

    }

    public void popView()
    { 
    
    }


}
