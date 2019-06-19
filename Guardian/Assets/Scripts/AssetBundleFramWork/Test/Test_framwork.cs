
/*
 
 整体框架测试
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_framwork : MonoBehaviour {

    //非GameObject资源测试：texture
    public GameObject goCubeChangeTexture1;
    private string _assetTexBundleName = "scene_1/textures.ab";
    private string _assetTexName = "unitychan_tile6";

    //GameObject资源测试：prefab
    //场景名称
    private string _sceneName = "scene_1";

    private string _assetBundleName = "scene_1/prefabs.ab";

    private string _assetName = "TestCubePrefab.prefab";
    private string _assetName1 = "Plane.prefab";

	// Use this for initialization
	void Start () {
        Debug.Log("开始测试框架=========");
       
        StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(_sceneName, _assetBundleName, LoadAllABComplete)); //包加载完设置回调
        //StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(_sceneName, _assetBundleName)); //仅仅只做包加载
	}



    private void LoadAllABComplete(string abName)
    {

        Debug.Log("所有AB包都加载完毕=========");
        //加载有依赖的ab包资源
        Object tObj = (Object)AssetBundleMgr.GetInstance().LoadAsset(_sceneName,_assetBundleName,_assetName);
        if (tObj != null)
        {
            Instantiate(tObj);
        }

        //加载没有有依赖的ab包资源
        //对于加载过的ab包，内存里不能再次添加，可以直接调用即可
        Object tObj1 = (Object)AssetBundleMgr.GetInstance().LoadAsset(_sceneName, _assetBundleName, _assetName1);
        if (tObj1 != null)
        {
            Instantiate(tObj1);
        }

        //非gameObject资源
        Texture tex1 = (Texture)AssetBundleMgr.GetInstance().LoadAsset(_sceneName, _assetTexBundleName, _assetTexName);
        if (tex1 != null)
        {
            goCubeChangeTexture1.GetComponent<Renderer>().material.mainTexture = tex1;
        }
    }

    private void LoadAllABComplete1(string abName)
    {

   
        Object tObj = (Object)AssetBundleMgr.GetInstance().LoadAsset(_sceneName, _assetBundleName, _assetName1);
        if (tObj != null)
        {
            Instantiate(tObj);
        }


    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("测试销毁资源");
            AssetBundleMgr.GetInstance().DisposeAllAssets(_sceneName);
        }
	}
}
