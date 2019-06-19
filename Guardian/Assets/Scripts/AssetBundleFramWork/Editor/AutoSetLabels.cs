using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


/*
	开发思路:
		1 定义需要打包资源的文件夹根目录
		2 遍历每个“场景”文件夹
		   A: 遍历本场景目录下所有的目录或者文件
		      如果是目录，则继续“递归”访问里面的文件，直到定位到文件
		   B: 找到文件，则使用AssetImporter类，标记“包名” 与 “后缀名”
 */
public class AutoSetLabels  {

	// 设置AB包名称
	[MenuItem("AssetBundleTools/Set AB Label")]
	public static void setABLable()
	{
		string strNeedSetLabelRoot = string.Empty;

		//目录信息:根目录下所有的目录信息(只能遍历一级目录下，二级子目录不能遍历)
		DirectoryInfo[] dirScenesDIRArray = null;

		//清空无用AB标记
		AssetDatabase.RemoveUnusedAssetBundleNames();
		
		//文件夹根目录
		strNeedSetLabelRoot = PathTool.GetABResourcesPath();

		DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);

		//只能获得子目录，不能获取子子目录
		dirScenesDIRArray = dirTempInfo.GetDirectories();
		
		//遍历每个“场景”文件夹（目录)
		foreach (DirectoryInfo currentDIR in dirScenesDIRArray)
		{
			 //2.1 遍历本场景目录下所有的目录或者文件。
                //如果是目录，则继续“递归”访问里面的文件，直到定位到文件
			 
			 // currentDIR 就是场景文件夹，todo 可以根据项目需求修改
			 string tmpScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name; //全路径
			 int tmpIndex = tmpScenesDIR.LastIndexOf("/");
			 string tmpScenesName = tmpScenesDIR.Substring(tmpIndex+1);

			 //Debug.Log("currentDIR.Name====="+currentDIR.Name);
			 //Debug.Log("tmpScenensName====="+tmpScenensName);
			 // 2.2  递归调用方法， 找到文件，则使用AssetImporter类，标记“包名”与“后缀名”
          JudgeDIRorFileByRecursive(currentDIR, tmpScenesName);
		}


		AssetDatabase.Refresh();
		Debug.Log("AssetBundle 本次操作设置标记完成");
	}

	private static void JudgeDIRorFileByRecursive(FileSystemInfo fileSysInfo , string scenesName)
	{
			if (!fileSysInfo.Exists)
			{
				 Debug.Log("文件或者目录名称： "+ fileSysInfo+" 不存在，请检查");
				 return;
			}

			//得到当前目录下一级的文件信息集合
			//文件信息转换为目录信息
			DirectoryInfo dirInfoObj = fileSysInfo as DirectoryInfo;
			FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();
			foreach (FileSystemInfo fileInfo in fileSysArray)
			{
				 FileInfo fileinfoObj = fileInfo as FileInfo;
				 if (fileinfoObj!=null)
				 {
					//文件类型
					//修改此文件的AssetBundle标签
               SetFileABLabel(fileinfoObj,scenesName);
				 }
				 else
				 {
					//目录信息
					JudgeDIRorFileByRecursive(fileInfo,scenesName);
				 }
			}

	}

	// 对指定的文件设置“AB包名称”
	private static void SetFileABLabel(FileInfo fileinfoObj, string scenesName)
	{
		string strABName = string.Empty;
      //文件路径（相对路径）
      string strAssetFilePath = string.Empty;
		//参数检查（*.meta 文件不做处理）
      if (fileinfoObj.Extension == ".meta") return;

		//得到AB包名称
		strABName=GetABName(fileinfoObj,scenesName);
		//获取资源文件的相对路径
		int tmpIndex = fileinfoObj.FullName.IndexOf("Assets");
		strAssetFilePath = fileinfoObj.FullName.Substring(tmpIndex);                    //得到文件相对路径
		//给资源文件设置AB名称以及后缀
		AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
		tmpImporterObj.assetBundleName = strABName;
		if (fileinfoObj.Extension == ".unity")
		{
				//定义AB包的扩展名
				tmpImporterObj.assetBundleVariant = "u3d";
		}
		else
		{
				tmpImporterObj.assetBundleVariant = "ab";
		}

	}

	/// 获取AB包的名称
	/*
		AB 包形成规则：
        ///     文件AB包名称=“所在二级目录名称”（场景名称）+“三级目录名称”（下一级的“类型名称”）
	 */
	private static string  GetABName(FileInfo fileinfoObj, string scenesName)
	{
		//返回AB包名称
		string strABName = string.Empty;

		//Win路径
		string tmpWinPath = fileinfoObj.FullName;                  //文件信息的全路径（Win格式）
		//Unity路径
		string tmpUnityPath = tmpWinPath.Replace("\\","/");        //替换为Unity字符串分割符
		//定位“场景名称”后面字符位置 
      int tmpSceneNamePostion = tmpUnityPath.IndexOf(scenesName)+ scenesName.Length;
		//AB包中“类型名称”所在区域  得到 ==》“Textures/xxxx”
      string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePostion+1);
		if (strABFileNameArea.Contains("/"))
		{
				string[] tempStrArray = strABFileNameArea.Split('/');
				//AB包名称正式形成
				strABName = scenesName + "/" + tempStrArray[0];
		}
		else {
				//定义*.Unity 文件形成的特殊AB包名称
				strABName = scenesName + "/" + scenesName;
		}

		return strABName;
	}
}
