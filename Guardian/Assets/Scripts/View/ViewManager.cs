
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

    private BaseView _curShowView;
    public GameObject _viewLayer;
    public GameObject _rootScene;

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

            if (_curShowView)
            {
                _curShowView.onHide();
            }
            
            string prefabName = "Prefabs/View/" + viewName;
            GameObject obj = (GameObject)Resources.Load(prefabName);
            GameObject viewObj = GameObject.Instantiate(obj);
            viewObj.transform.parent = _viewLayer.transform;

            viewObj.name = viewName;

            //默认取屏幕中点
            Vector3 displayPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            viewObj.transform.localPosition = displayPos;

            GameObject rootDialogGameObject = new GameObject("rootDialogNode");
            rootDialogGameObject.transform.parent = viewObj.transform;

            BaseView baseView = viewObj.GetComponent<BaseView>();
            baseView._name = viewName;
            baseView._prefabName = prefabName;
            baseView._rootDialogGameObject = rootDialogGameObject;

            _curShowView = baseView;
            _curShowView.onTop();
            //页面管理
            _viewDict.Add(viewName, baseView);
           
          
        }
        else
        {
            BaseView view = _viewDict[viewName];
            string name = _curShowView._name;
            if (name == viewName)
            {
                Debug.Log("已经打开过" + viewName + " 已经在最顶层");
                return;
            }

            Debug.Log("已经打开过" + viewName + " 调整到最顶层");
            Transform parentTransform = _viewLayer.transform;

            int count = parentTransform.childCount;


            GameObject _viewObj = view.gameObject;
            Transform childTransform = _viewObj.transform;

            _curShowView.onHide();
            _viewObj.SetActive(false);
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
            _viewObj.SetActive(true);
            _curShowView = view;
            _curShowView.onTop();

        }
      

    }

    public void popView()
    {
        //把顶部那个view给pop调
        if (_curShowView != null)
        {
            string name = _curShowView._name;
            _viewDict.Remove(name);

            //立即删除当前脚本组件，回调用gameobjec上组件的OnDestroy方法
            DestroyImmediate(_curShowView.gameObject);
            _curShowView = null;
           
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

             }
        }

    }

    public void swithView(string showViewName)
    {
        if (_curShowView)
        {
            string name = _curShowView._name;
            if (name == showViewName)
            {
                Debug.Log(GetType() + ":swithView " + showViewName + " 已经在最顶层");
                return;
            }
        }
        
        popView();
        showView(showViewName);
    }


}
