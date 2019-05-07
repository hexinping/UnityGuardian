using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 
 1 Resources.UnloadAsset() 只能释放非"GameObjet"类型资源，“GameObject”类型资源需要使用Resources.UnloadUnusedAssets();
		2 Resources.UnloadAsset() 
		{
			texture,shader,audioClip,animationClip, 这几种类型直接调用就能释放
			
			释放sprite
			{
				无引用：先释放对应的纹理，再释放自身
					Texture tex = _sprObj1.texture;
		            Resources.UnloadAsset(tex);
		            Resources.UnloadAsset(_sprObj1); //单纯是靠引用计数来删除的

	            有引用: 先把引用删除，再释放对应的纹理，最后释放自身
		            _sp1.GetComponent<SpriteRenderer>().sprite = null;// Destroy(_sp1);
					Texture tex = _sprObj.texture;
	            	Resources.UnloadAsset(tex);
					Resources.UnloadAsset(_sprObj1);
			}

			释放材质
			{
				无引用：直接释放
				有引用：先删除引用，再释放
					_cube.GetComponent<Renderer>().material = null;//Destroy(_cube);
            		Resources.UnloadAsset(_materialObj);
			}

			释放带贴图的材质
			{
				无引用
					Texture tex = _materialTexObj.mainTexture; //只释放主贴图
		            Resources.UnloadAsset(tex);  //一定要先释放贴图再释放材质
		            Resources.UnloadAsset(_materialTexObj); 

		        有引用：
			        _cube.GetComponent<Renderer>().material = null;//Destroy(_cube)
		            Texture tex = _materialTexObj.mainTexture;
		            Resources.UnloadAsset(tex);
		            Resources.UnloadAsset(_materialTexObj); 

			}
		}

		3 释放预设体 必须使用Resources.UnloadUnusedAssets():
		{
			//一定要删除引用再调用Resources.UnloadUnusedAssets()方法
			 _prefabObj = null; 
            Resources.UnloadUnusedAssets();
		}

		4 不管里全局变量还是局部变量，不删除资源，会一直保存在内存中
        _sprObj1 = Resources.Load<Sprite>("Textures/login_select"); //不删除，会一直存在内存中
        //Sprite t_sprObj1 = Resources.Load<Sprite>("Textures/login_select"); //不删除，会一直存在内存中
 
        5 Resources.UnloadAsset ==>不管资源是否在Resources文件下，都会被调用删除逻辑
 
 
 */
public enum ResourceType 
{
   //非GameObject类型
    Texture,
    Sprite,
    Material,
    Shader,
    AudioClip,
    AnimationClip,
			
    //GameObject类型
    Prefab
};



public class ResourceObj
{
    public Object obj;
    public ResourceType type;
    public List<string> refList;       //属于哪个界面 :一对多的关系(一个界面还可以引用多次)
    public string pathName;             //资源的路径
    public int refCount = 0;            //资源的引用计数

    //为了跟Unity自身的材质和shader区分，加标记
    public bool isMaterailCustom ;   
    public bool isShaderCustom;

    public ResourceObj(Object rObj, ResourceType rtype, string rPahName, bool customMaterail, bool customShader)
    {
        obj = rObj;
        type = rtype;
        pathName = rPahName;
        refList = new List<string>();
        isMaterailCustom = customMaterail;
        isShaderCustom = customShader;
    }

    public void addRefCount(string refKey)
    {
        refCount++;
        refList.Add(refKey);
       
    }

    public void reduceRefCount(string refKey)
    {
        refCount--;
        refList.Remove(refKey);//相同的元素也只会删除一个
    }

}

/*
    针对Resource文件加下的的资源管理
 * 
 * 仅仅用资源路径名称作为key存储缓存，
 *  ：一个界面使用多个相同资源==》只会存储一份资源内存
 *    多个界面使用同一份资源  ==》只会存储一份资源内存
 *    
 *  viewName 是为了来做引用计数管理的
 */
public class ResourcesManager  {

    public static ResourcesManager _instance;

    private int count = (int)ResourceType.Prefab;
    private List<Dictionary<string, ResourceObj>> _resourcesList;
    private Dictionary<string, List<ResourceObj>> _resourcesDict; // 根据view来存储资源


    private ResourcesManager()
    { 
        _resourcesList = new List<Dictionary<string, ResourceObj>>();
        for (int i = 0; i <= count; i++)
        {
            Dictionary<string, ResourceObj> dict = new Dictionary<string, ResourceObj>();
            _resourcesList.Add(dict);
        }

        _resourcesDict = new Dictionary<string, List<ResourceObj>>();

    }
    static public ResourcesManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new ResourcesManager();
        }
        return _instance;
    }


    //添加资源
    //public void addResource(ResourceType type, string path, string viewName)
    //{
    //    string path = getResourceRootPath(type);
    //    path = path + name + "_" + viewName;

    //    int index = (int)type;
    //    if (!_resourcesList[index].ContainsKey(path))
    //    {
    //        Object obj = Resources.Load<Object>(path);
    //        ResourceObj resObj = new ResourceObj(obj, type, path, viewName);
    //        _resourcesList[index].Add(path, resObj);
    //    }
    //}

    //手动添加引用计数
    public void addResourceRefCount(ResourceType type, string name, string viewName)
    {
        int index = (int)type;
        string path = getResourceKey(type, name);
        if (_resourcesList[index].ContainsKey(path))
        {
            ResourceObj resObj = _resourcesList[index][path];
            resObj.addRefCount(viewName);
        }
    }

    //手动减少引用计数
    public void reduceResourceRefCount(ResourceType type, string name, string viewName)
    {
        int index = (int)type;
        string path = getResourceKey(type, name);
        if (_resourcesList[index].ContainsKey(path))
        {
            ResourceObj resObj = _resourcesList[index][path];
            resObj.reduceRefCount(viewName);
        }
    }
    /*
        一个界面引用多个name相同的资源     --》ok
     *  多个界面引用同一个资源             --》ok
     *  一个界面以后用多个不同资源         --》ok
     *  多个界面以后用多个不同资源         --》ok
     */
    //获取一个资源
    public Object getResouce(ResourceType type, string name, string viewName, bool materailCustom = true, bool shaderCustom = true)
    {
        int index = (int)type;
        string path = getResourceKey(type, name);
        ResourceObj resObj = null;
        if (_resourcesList[index].ContainsKey(path))
        {
            resObj = _resourcesList[index][path];
        }
        else
        {
            Object obj = null;
            if (type == ResourceType.Sprite)
            {
                obj = Resources.Load<Sprite>(path);
            }
            else
            {
                obj = Resources.Load<Object>(path);
            }

            resObj = new ResourceObj(obj, type, path, materailCustom, shaderCustom);
            _resourcesList[index].Add(path, resObj);
        }
        //引用管理
        resObj.addRefCount(viewName);

        //页面资源管理
        addViewCache(viewName, resObj);
        return resObj.obj;
    }

    private void addViewCache(string viewName, ResourceObj resObj)
    {
        //页面资源管理
        if (!_resourcesDict.ContainsKey(viewName))
        {
            List<ResourceObj> listResObj = new List<ResourceObj>();
            _resourcesDict.Add(viewName, listResObj);
            listResObj.Add(resObj);
        }
        else
        {
            List<ResourceObj> listResObj = _resourcesDict[viewName];
            if (!listResObj.Contains(resObj)) //页面的list里只存一份资源引用
            {
                listResObj.Add(resObj);
            }

        }
    }

    private void deleteViewCache(string viewName, ResourceObj resObj)
    {
        //页面资源管理
        if (_resourcesDict.ContainsKey(viewName))
        {
            List<ResourceObj> listResObj = _resourcesDict[viewName];
            if (listResObj.Contains(resObj))
            {
                listResObj.Remove(resObj);
            }
        }

    }

    //删除一个资源 (shader和材质只删除自定义的)
    public void removeResouce(ResourceType type, string name, string viewName)
    {
        int index = (int)type;
        string path = getResourceKey(type, name);
        if (_resourcesList[index].ContainsKey(path))
        { 
           
            ResourceObj resObj = _resourcesList[index][path];
            //引用管理
            resObj.reduceRefCount(viewName);

           
            if (resObj.refCount <= 0)
            {
                deleteViewCache(viewName, resObj);
                //真正的卸载资源
                unLoadResouce(resObj);
            }
        }
    }

    //删除某个view的所有资源,如果其他view有引用，资源不会删除
    public void removeResoucesByView(string viewName)
    {
        //listResObj只存储了一份资源引用
        List<ResourceObj> listResObj = _resourcesDict[viewName];
        int len = listResObj.Count;
        for (int i = len - 1; i >= 0; i--)
        {
            ResourceObj resObj = listResObj[i];
            resObj.reduceRefCount(viewName);
            if (resObj.refCount <= 0)
            {
                deleteViewCache(viewName, resObj);
                //真正的卸载资源
                unLoadResouce(resObj);
            }
        }

    }

    private void unLoadResouce(ResourceObj resObj)
    {
        ResourceType type = resObj.type;
        int index = (int)type;
        string path = resObj.pathName;

        //当引用计数小于0的时候才卸载资源
        _resourcesList[index].Remove(path);
        Object obj = resObj.obj;
       
        
        if (type == ResourceType.Prefab)
        {
            obj = null;
            Resources.UnloadUnusedAssets();
        }
        else
        {
            if (type == ResourceType.Texture || type == ResourceType.Shader || type == ResourceType.AudioClip || type == ResourceType.AnimationClip)
            {
                if (type != ResourceType.Shader)
                {
                    Resources.UnloadAsset(obj);
                }
                else
                {
                    bool isCustomShader = resObj.isShaderCustom;
                    if (isCustomShader)
                    {
                        Resources.UnloadAsset(obj);
                    }
                }
                
            }
            else if (type == ResourceType.Sprite)
            {
                //先把关联贴图删除再删除sprite
                Sprite sp = (Sprite)obj;
                Texture tex = sp.texture;
                Resources.UnloadAsset(tex);
                Resources.UnloadAsset(obj);
            }
            else if (type == ResourceType.Material)
            {
                bool isCustomMaterial = resObj.isMaterailCustom;
                bool isCustomShader = resObj.isShaderCustom;
               
                if (isCustomMaterial)
                {
                    Material ma = (Material)obj;

                    //先把关联贴图删除再删除material,暂时只支持删除主贴图
                    Texture tex = ma.mainTexture;
                    if (tex != null)
                    {
                        Resources.UnloadAsset(tex);
                    }

                    //把相关shader也删除
                    if (isCustomShader)
                    {
                        Shader shader = ma.shader;
                        if (shader != null)
                        {
                            Resources.UnloadAsset(shader);
                        }
                    }
                    Resources.UnloadAsset(obj);
                }

            }
        }
    }

    //返回一个资源存储的key值
    private string getResourceKey(ResourceType type, string name)
    {
        string path = getResourceRootPath(type);
        path = path + name;
        return path;
    }
    private string getResourceRootPath(ResourceType type)
    {
        string path = string.Empty;
        switch (type)
        {
            case ResourceType.Texture:
            case ResourceType.Sprite:
                path = "Textures/";
                break;
            case ResourceType.Material:
                path = "Materials/";
                break;
            case ResourceType.Shader:
                path = "Shaders/";
                break;
            case ResourceType.AudioClip:
                path = "Audios/";
                break;
            case ResourceType.AnimationClip:
                path = "Animations/";
                break;
            case ResourceType.Prefab:
                path = "Prefabs/";
                break;
            default:
                break;
        }
        return path;
    }


    
}
