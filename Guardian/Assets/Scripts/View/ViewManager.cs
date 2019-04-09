
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

    private BaseView _curShowView;

    public int viewIndex = 0;

    public void Awake()
    {
        _instance = this;
        _viewDict = new Dictionary<string, BaseView>();
    }

    static public ViewManager getInstance()
    {
        return _instance;
    }

    public void showView(string viewName)
    {

        if (!_viewDict.ContainsKey(viewName))
        {
            BaseView view = new BaseView();

            //BaseView view = this.gameObject.AddComponent<BaseView>();

            string prefabName = "Prefabs/View/" + viewName;
            GameObject obj = (GameObject)Resources.Load(prefabName);
            view.initUI(obj, viewName);

            //view.addListener();

            _curShowView = view;

            //页面管理
            _viewDict.Add(viewName, view);
       
        }
        else
        {
           
            BaseView view = _viewDict[viewName];
            string name = view.getViewName();
            if (name == viewName)
            {
                Debug.Log("已经打开过" + viewName + " 已经在最顶层");
                return;
            }

            Debug.Log("已经打开过" + viewName + " 调整到最顶层");
            Transform parentTransform = _viewLayer.transform;

            int count = parentTransform.childCount;
            Transform childTransform = view._viewRoot.transform;


            view.setActive(false);
            int zorder = 0;
            for (int i = 0; i < count; i++)
            {
                Transform t = parentTransform.GetChild(i);
                if (childTransform != t)
                {
                    t.gameObject.SetActive(false);
                    t.SetSiblingIndex(zorder);
                    zorder++;
                    t.gameObject.SetActive(true);
                }

            }
            //参数为物体在当前所在的子物体列表中的顺序
            childTransform.SetSiblingIndex(count - 1);
            view.setActive(true);
            _curShowView = view;

        }
      

    }

    public void popView()
    {
        //把顶部那个view给pop调
        if (_curShowView != null)
        {
            string name = _curShowView.getViewName();
            _curShowView.onHide();
            _curShowView.destory();
            _viewDict.Remove(name);
            _curShowView = null;


            Destroy(_curShowView);
             Transform parentTransform = _viewLayer.transform;
             foreach (Transform transform in parentTransform)
            {
                transform.gameObject.SetActive(false);
                transform.gameObject.SetActive(true);
            }
            //更新_curShowView 
             int count = parentTransform.childCount;
             if (count >= 1)
             {
                 Transform t = parentTransform.GetChild(count - 1);
                 string topViewName = t.gameObject.name;
                 _curShowView = _viewDict[topViewName];
                 _curShowView.onTop();

             }
        }

    }

    public void swithView(string showViewName)
    { 
        popView();
        showView(showViewName);
    }


}
