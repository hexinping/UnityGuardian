using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_sigleAB : MonoBehaviour {


    //非GameObject资源测试
    public GameObject goCubeChangeTexture1;

    private string _URL1;
    private string _assetTexName1;
    private Texture _texObj;
    private GameObject _gameObj;
    private Object _tmpObj;

	//引用类
        private SingleABLoader _LoadObj = null;
        /*  依赖AB包名称  */
        private string _ABDependName1 = "scene_1/textures.ab"; //贴图AB包
        private string _ABDependName2 = "scene_1/materials.ab";//材质AB包
        //AB包名称
        private string _ABName1 = "scene_1/prefabs.ab";
        //AB包中资源名称
        private string _AssetName1 = "TestCubePrefab.prefab";
		private string _AssetName2 = "Capsule.prefab";


        #region 简单（无依赖包）预设的加载
        private void Start()
        {
            //非GameObject资源测试 ==》ok
            _URL1 = PathTool.GetWWWPath() + "/scene_1/textures.ab"; // 

            //AB包内部资源名称，就是原始资源的名称
            _assetTexName1 = "unitychan_tile6";
            StartCoroutine(LoadNoeObjectFramAB(_URL1, goCubeChangeTexture1, _assetTexName1));
            //_LoadObj = new SingleABLoader(_ABDependName1, LoadComplete);
            //StartCoroutine(_LoadObj.LoadNoeObjectFramAB(_URL1, goCubeChangeTexture1, _assetTexName1));


           //加载非依赖AB包 ok
           //_LoadObj = new SingleABLoader(_ABName1, LoadComplete);
           //StartCoroutine(_LoadObj.LoadAssetBundle());

		    //依赖包 ==>ok 
			SingleABLoader _LoadDependObj = new SingleABLoader(_ABDependName1, LoadDependComplete1);
            //加载AB依赖包
            StartCoroutine(_LoadDependObj.LoadAssetBundle());
        }

        /// <summary>
        /// 回调函数（一定条件下自动执行）
        /// </summary>
        /// <param name="abName"></param>
        private void LoadComplete(string abName)
        {
           //加载AB包中的资源
           UnityEngine.Object tmpObj=_LoadObj.LoadAsset(_AssetName2,false);
           //克隆对象
           Instantiate(tmpObj);
        }

        #endregion

        //依赖回调函数1
        private void LoadDependComplete1(string abName)
        {
            Debug.Log("依赖包1（贴图包）加载完毕，加载依赖包2（材质包）");
            SingleABLoader _LoadDependObj2 = new SingleABLoader(_ABDependName2, LoadDependComplete2);
            //加载AB依赖包
            StartCoroutine(_LoadDependObj2.LoadAssetBundle());
        }

        //依赖回调函数2
        private void LoadDependComplete2(string abName)
        {
            Debug.Log("依赖包2（材质包）加载完毕，开始正式加载预设包");
            _LoadObj = new SingleABLoader(_ABName1, LoadCompleteDepend);
            //加载AB依赖包
            StartCoroutine(_LoadObj.LoadAssetBundle());
        }

        /// <summary>
        /// 回调函数（一定条件下自动执行）
        /// </summary>
        /// <param name="abName"></param>
        private void LoadCompleteDepend(string abName)
        {
            //加载AB包中的资源
            UnityEngine.Object tmpObj = _LoadObj.LoadAsset(_AssetName1, false);
            _tmpObj = tmpObj;
            //克隆对象
            _gameObj = (GameObject)Instantiate(tmpObj);

            /*  查询包中的资源*/
            string[] strArray = _LoadObj.RetrivalAllAssetName();
            foreach (string str in strArray)
            {
                Debug.Log(str);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //_LoadObj.UnLoadAsset(_texObj, true);  //释放资源内存
               
                //_LoadObj.UnLoadAsset(_tmpObj,true);
                /*
                 * // 只能释放对应的预设体，不能删除预设引用的材质以及材质中的贴图
                     _tmpObj = null;
                    Resources.UnloadUnusedAssets();
                 */

                //Destroy(_gameObj); //*********************仅仅释放克隆体的内存，如果要释放对应的预设体资源，需要预设体对象为null
                Debug.Log("释放镜像内存资源，与内存资源");
                
                 //_LoadObj.Dispose();//释放镜像内存资源
                _LoadObj.DisposeALL();//释放镜像内存资源，与内存资源（gameObject）//只能释放GameObject类型资源(预设体)，不能释放非内存资源（纹理，材质。。。。）
                
            }
        }

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
                        _texObj = (Texture)ab.LoadAsset(assetName);
                        goShowObj.GetComponent<Renderer>().material.mainTexture = _texObj;
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
}
