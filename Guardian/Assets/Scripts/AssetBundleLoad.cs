using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoad : MonoBehaviour {


    //非GameObject资源测试
    public GameObject goCubeChangeTexture1;
    public GameObject goCubeChangeTexture2;

    private string _URL1;
    private string _assetName1;
    private string _assetName2;

    private Coroutine _IELoadNoeObjectFramAB;


    //prefab测试
    public Transform _showTransform;
    private string _prefabName;
    private string _URLPrefab;


    void Awake()
    { 
        //PC端前要加file:://
        //要加载的AB包，一个包里可以放多个资源
        _URL1 = "file://" + Application.streamingAssetsPath + "/texture1"; // 

        //AB包内部资源名称，就是原始资源的名称
        _assetName1 = "unitychan_tile3";
        _assetName2 = "unitychan_tile6";


        _URLPrefab = "file://" + Application.streamingAssetsPath + "/prefabs";
        _prefabName = "prefabCube";
    }
	// Use this for initialization
	void Start () {
        //_IELoadNoeObjectFramAB = StartCoroutine(LoadNoeObjectFramAB(_URL1, goCubeChangeTexture1, _assetName1));
        //this.Invoke("testABLoad",2.0f);

        StartCoroutine(LoadPrefabFramAB(_URLPrefab, _prefabName, _showTransform));
	}

    void testABLoad()
    {
        StopCoroutine(_IELoadNoeObjectFramAB);
        StartCoroutine(LoadNoeObjectFramAB(_URL1, goCubeChangeTexture2, _assetName2));
    }
    

    //加载预设资源
    IEnumerator LoadPrefabFramAB(string ABURL, string assetName, Transform showTransfrom = null)
    { 
        //参数检查
        if (string.IsNullOrEmpty(ABURL))
        {
            Debug.LogError(GetType() + "/LoadPrefabFramAB()/输入的参数不合法，请检查");
        }

        using (WWW www = new WWW(ABURL)) //使用using语法，当www对象离开using大括号时就自动卸载了
        {
            yield return www;
            AssetBundle ab = www.assetBundle;  //下载ab包
            if (ab != null)
            {
                if (assetName == "")
                {
                    //加载主资源
                    if (showTransfrom != null)
                    {
                        GameObject goCloneObj = (GameObject)Instantiate(ab.mainAsset);
                        goCloneObj.transform.localPosition = showTransfrom.localPosition;
                    }
                    else
                    { 
                        //克隆加载的预设
                        Instantiate(ab.mainAsset);
                    }
                }
                else
                {
                    //实例化指定资源
                    if (showTransfrom != null)
                    {
                        GameObject goCloneObj = (GameObject)Instantiate(ab.LoadAsset(assetName));  //加载ab包
                        goCloneObj.transform.localPosition = showTransfrom.localPosition;
                        goCloneObj.name = assetName;
                    }
                    else
                    {
                        //克隆加载的预设
                        Instantiate(ab.LoadAsset(assetName));
                    }
                }

                //卸载资源(只卸载AB包本身)
                ab.Unload(false);
            }
            else
            {
                Debug.LogError(GetType() + "/LoadPrefabFramAB()/WWW 下载错误，请检查 URL: " + ABURL + " 错误信息:" + www.error);
            }
        }
    }

    //加载 “非GameObject”资源
    /// <summary>
    /// 加载“非GameObject”资源 (贴图 材质 音频....)
    /// </summary>
    /// <param name="ABURL">AB包URL</param>
    /// <param name="goShowObj">操作且显示的对象</param>
    /// <param name="assetName">加载资源的名称</param>
    /// <returns></returns>
    IEnumerator LoadNoeObjectFramAB(string ABURL, GameObject goShowObj, string assetName)
    {
        //参数检查
        if (string.IsNullOrEmpty(ABURL) || goShowObj == null)
        {
            Debug.LogError(GetType() + "/LoadNonObjectFromAB()/输入的参数不合法，请检查");
        }

        using (WWW www = new WWW(ABURL))
        {
            yield return www;
            AssetBundle ab = www.assetBundle;
            if (ab != null)
            {
                if (assetName == "")
                {
                    goShowObj.GetComponent<Renderer>().material.mainTexture = (Texture)ab.mainAsset;
                }
                else
                {
                    goShowObj.GetComponent<Renderer>().material.mainTexture = (Texture)ab.LoadAsset(assetName);
                }

                //卸载资源(只卸载AB包本身)
                ab.Unload(false);
            }
            else
            {
                Debug.LogError(GetType() + "/LoadNonObjectFromAB()/WWW 下载错误，请检查 URL: " + ABURL + " 错误信息:" + www.error);
            }
        }
    }



    //加载AB包（不提取资源）
    IEnumerator LoadFromAB(string ABURL)
    {
        //参数检查
        if (string.IsNullOrEmpty(ABURL))
        {
            Debug.LogError(GetType() + "/LoadFromAB()/输入的参数不合法，请检查");
        }

        using (WWW www = new WWW(ABURL))
        {
            yield return www;
            AssetBundle ab = www.assetBundle;
            if (ab == null)
            {
                Debug.LogError(GetType() + "/LoadNonObjectFromAB()/WWW 下载错误，请检查 URL: " + ABURL + " 错误信息:" + www.error);
            }
        }
    }
}
