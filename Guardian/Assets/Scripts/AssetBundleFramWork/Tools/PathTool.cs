using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTool {

	public const string AB_RESROOTPATH = "AB_Res";

	//得到AB资源的输入目录
	public static string GetABResourcesPath()
	{
		return Application.dataPath + "/" + AB_RESROOTPATH;
	}

	//获取AB输出路径
	public static string GetABOutPath()
	{
		return GetPlatformPath() + "/" + GetPlatformName();
	}

	/// 获取平台的路径
	private static string GetPlatformPath()
	{
		string strReturnPlatformPath = string.Empty;
		switch (Application.platform)
		{	
			//Application.streamingAssetsPath 只是可读，不能删除原始zip文件以及加密
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXEditor:
				strReturnPlatformPath = Application.streamingAssetsPath;
				break;
			//移动平台，Application.persistentDataPath为可读可写目录 
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
				strReturnPlatformPath = Application.persistentDataPath;
				/*
					Application.persistentDataPath
						android :   "/storage/emulated/0/Android/data/package name/files"
						ios:        "/var/mobile/Containers/Data/Application/app sandbox/Documents"

					Application.streamingAssetsPath  
						android: jar:file:///data/app/package name-1/base.apk!/assets    
						ios:  /var/containers/Bundle/Application/app sandbox/test.app/Data/Raw 
				 */
				
				break;
			default:
				break;
		}

		return strReturnPlatformPath;
	}

	/// 获取平台的名称
	public static string GetPlatformName()
	{
		string strReturnPlatformName = string.Empty;

		switch (Application.platform)
		{
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				strReturnPlatformName = "Windows";
				break;
			case RuntimePlatform.IPhonePlayer:
				strReturnPlatformName = "Iphone";
				break;
			case RuntimePlatform.Android:
				strReturnPlatformName = "Android";
				break;
			case RuntimePlatform.OSXEditor:
				strReturnPlatformName = "MACOS";
				break;
			default:
				break;
		}

		return strReturnPlatformName;
	}

	/// 获取WWW协议下载（AB包）路径
	public static string GetWWWPath()
	{
		//返回路径字符串
		string strReturnWWWPath = string.Empty;

		switch (Application.platform)
		{
			//Windows 主平台
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
				strReturnWWWPath = "file://" + GetABOutPath();
				break;
			//Android 平台
			case RuntimePlatform.Android:
				strReturnWWWPath = "jar:file://" + GetABOutPath();
				break;
			//IOS 平台
			case RuntimePlatform.IPhonePlayer:
				strReturnWWWPath = GetABOutPath()+"/Raw/";
				break;
			default:
				break;
		}

		return strReturnWWWPath;
	}



}
