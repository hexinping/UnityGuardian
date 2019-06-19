using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;  //引入Unity编辑器命名空间
using System.IO;

public class BuildAssetBundle  {

    //所有基于编辑器的开发都必须是静态方法
    [MenuItem("AssetBundleTools/BuildAllAssetBundle")]
    public static void BuildAllAB()
    { 
        //打包输出路径
        string strABoutPath = string.Empty;

        strABoutPath = PathTool.GetABOutPath();

        //判断生成输出目录
        if(!Directory.Exists(strABoutPath))
        {
            Directory.CreateDirectory(strABoutPath);
        }

        //打包生成
        string strReturnPlatformPath = string.Empty;
		switch (Application.platform)
		{

			case RuntimePlatform.WindowsEditor:
                BuildPipeline.BuildAssetBundles(strABoutPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
                break;
			case RuntimePlatform.OSXEditor:
				BuildPipeline.BuildAssetBundles(strABoutPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
				break;
			default:
				break;
		}
        
    }
}
