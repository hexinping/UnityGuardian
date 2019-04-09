
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

        if (!_viewDict.ContainsKey(viewName))
        {
            BaseView view = new BaseView();
            GameObject obj = (GameObject)Resources.Load(prefabName);
            view.initUI(obj, viewName);

            //页面管理
            _viewDict[viewName] = view;
        }
        else
        {
            Debug.Log("已经打开过" + viewName + " 调整到最顶层");


            BaseView view = _viewDict[viewName];
            
            Transform parentTransform = _viewLayer.transform;

            int count = parentTransform.childCount;
            Transform childTransform = view._viewRoot.transform;
            view._viewRoot.SetActive(false);
            //int zorder = 0;
            //for (int i = 0; i < count;i++ )
            //{
            //    Transform  t = parentTransform.GetChild(i);
               
            //}
            //参数为物体在当前所在的子物体列表中的顺序
            //count-1指把child物体在当前子物体列表的顺序设置为最后一个，0为第一个
            childTransform.SetSiblingIndex(count - 1);
            view._viewRoot.SetActive(true);

        }
      

    }

    public void popView()
    { 
    
    }


}
