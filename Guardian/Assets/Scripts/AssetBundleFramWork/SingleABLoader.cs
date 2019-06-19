using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	加载单一AB包

 */
public class SingleABLoader : System.IDisposable {

	//引用类： 资源加载类
	private AssetLoader _AssetLoader;
	//委托：
	private DelLoadComplete _LoadCompleteHandle;
	//AssetBundle 名称
	private string _ABName;
	//AssetBundle 下载路径
	private string _ABDownLoadPath;

	//构造函数
	public SingleABLoader(string abName,DelLoadComplete loadComplete)
	{
		_AssetLoader = null;
		_ABName = abName;
		//委托初始化
		_LoadCompleteHandle = loadComplete;
		//AB包下载路径（初始化）
		_ABDownLoadPath = PathTool.GetWWWPath() + "/" + _ABName;            
	}

	//加载AssetBundle 资源包
	public IEnumerator LoadAssetBundle()
	{
		using (WWW www=new WWW(_ABDownLoadPath))
		{
			yield return www;
			//WWW下载AB包完成
			if (www.progress>=1)
			{
				//获取AssetBundle的实例
				AssetBundle abObj = www.assetBundle;
				if (abObj!=null)
				{
					//实例化引用类
					_AssetLoader = new AssetLoader(abObj);
					//AssetBundle 下载完毕，调用委托
					if (_LoadCompleteHandle!=null)
					{
						_LoadCompleteHandle(_ABName);
					}

				}
				else {
					Debug.LogError(GetType()+ "/LoadAssetBundle()/WWW 下载出错，请检查！ AssetBundle URL: "+ _ABDownLoadPath+" 错误信息： "+www.error);
				}
			}
		}//using_end            
	}

	
	/// 加载（AB包内）资源
	public UnityEngine.Object LoadAsset(string assetName,bool isCache)
	{
		if (_AssetLoader!=null)
		{
			return _AssetLoader.LoadAsset(assetName,isCache);
		}
		Debug.LogError(GetType()+ "/LoadAsset()/ 参数_AssetLoader==null  ,请检查！");
		return null;
	}

	
	/// 卸载（AB包中）资源
	public void UnLoadAsset(UnityEngine.Object asset,bool isRemoveUnUseAssets = false)
	{
		if (_AssetLoader != null)
		{
            _AssetLoader.UnLoadAsset(asset, isRemoveUnUseAssets);
		}
		else {
			Debug.LogError(GetType()+ "/UnLoadAsset()/参数 _AssetLoader==Null , 请检查！");
		}
	}

	
	/// 释放资源
	public void Dispose()
	{
		if (_AssetLoader != null)
		{
			_AssetLoader.Dispose();
			_AssetLoader = null;
		}
		else
		{
			Debug.LogError(GetType() + "/Dispose()/参数 _AssetLoader==Null , 请检查！");
		}
	}

	
	/// 释放当前AssetBundle资源包,且卸载所有资源
	public void DisposeALL()
	{
		if (_AssetLoader != null)
		{
			_AssetLoader.DisposeALL();
			_AssetLoader = null;
		}
		else
		{
			Debug.LogError(GetType() + "/DisposeALL()/参数 _AssetLoader==Null , 请检查！");
		}
	}

	
	/// 查询当前AssetBundle包中所有的资源
	public string[] RetrivalAllAssetName()
	{
		if (_AssetLoader != null)
		{
			return _AssetLoader.RetriveAllAssetName();
		}
		Debug.LogError(GetType() + "/RetrivalAllAssetName()/参数 _AssetLoader==Null , 请检查！");
		return null;
	}
}
